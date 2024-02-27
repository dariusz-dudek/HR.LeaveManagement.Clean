using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly ILeaveAllocationRepository _allocationRepository;
        private readonly IEmailSender _emailSender;

        // ReSharper disable once ConvertToPrimaryConstructor
        public CancelLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository, ILeaveAllocationRepository allocationRepository, IEmailSender emailSender)
        {
            _requestRepository = requestRepository;
            _allocationRepository = allocationRepository;
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _requestRepository.GetByIdAsync(request.Id);

            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            leaveRequest.Cancelled = true;
            await _requestRepository.UpdateAsync(leaveRequest);

            if (leaveRequest.Approved == true)
            {
                var daysRequired = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = await _allocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId,
                    leaveRequest.LeaveTypeId);
                allocation.NumberOfDays += daysRequired;

                await _allocationRepository.UpdateAsync(allocation);
            }

            //send configuration email

            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body =
                        $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been cancelled successfully",
                    Subject = "Leave Request Cancelled"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception e)
            {
                //Log error
                throw;
            }

            return Unit.Value;
        }
    }
}
