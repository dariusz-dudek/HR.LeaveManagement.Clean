using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _logger;

        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository, ILeaveTypeRepository typeRepository, IMapper mapper, IEmailSender emailSender, IAppLogger<UpdateLeaveRequestCommandHandler> logger)
        {
            _requestRepository = requestRepository;
            _typeRepository = typeRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _requestRepository.GetByIdAsync(request.Id);

            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(LeaveRequest), request.Id);
            }

            var validator = new UpdateLeaveRequestValidator(_typeRepository, _requestRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                throw new BadRequestException("Invalid Leave Request", validatorResult);
            }

            _mapper.Map(request, leaveRequest);

            await _requestRepository.UpdateAsync(leaveRequest);



            try
            {
                //Send confirmation email
                var email = new EmailMessage
                {
                    To = string.Empty, // GEt email from employee record
                    Body =
                        $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been updated successfully",
                    Subject = "Leave Request Submitted"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }

            return Unit.Value;
        }
    }
}
