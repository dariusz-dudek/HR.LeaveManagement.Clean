using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
// ReSharper disable ConvertToPrimaryConstructor

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveAllocationRepository _allocationRepository;
        private readonly IMapper _mapper;

        public ChangeLeaveRequestCommandHandler(
            ILeaveRequestRepository requestRepository, 
            ILeaveTypeRepository typeRepository, 
            IEmailSender emailSender, 
            ILeaveAllocationRepository allocationRepository,
            IMapper mapper)
        {
            _requestRepository = requestRepository;
            _typeRepository = typeRepository;
            _emailSender = emailSender;
            _allocationRepository = allocationRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _requestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(LeaveRequest), request.Id);
            }

            leaveRequest.Approved = request.Approved;
            await _requestRepository.UpdateAsync(leaveRequest);

            //if request is approved, get and update the employee's allocations
            if (request.Approved)
            {
                var daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = await _allocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId,
                    leaveRequest.LeaveTypeId);
                allocation.NumberOfDays -= daysRequested;

                await _allocationRepository.UpdateAsync(allocation);
            }

            // send confirmation email
            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty, //Get email from employee record
                    Body =
                        $"The approval status for your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been updated",
                    Subject = "Leave Request Approval Status Updated"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception e)
            {
                //log error
            }

            return Unit.Value;
        }
    }
}
