using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _email;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly ILeaveRequestRepository _requestRepository;

        public CreateLeaveRequestCommandHandler(IEmailSender email, IMapper mapper, ILeaveTypeRepository typeRepository, ILeaveRequestRepository requestRepository)
        {
            _email = email;
            _mapper = mapper;
            _typeRepository = typeRepository;
            _requestRepository = requestRepository;
        }
        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestValidator(_typeRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            //Get request employee's id

            //Check on employee's allocation

            //if allocations aren't enough, return validation error witch message

            //send confirmation email
            var email = new EmailMessage
            {
                To = string.Empty, //Get email from employee record
                Body =
                    $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been submitted successfully",
                Subject = "Leave Request Submitted"
            };

            await _email.SendEmail(email);

            return Unit.Value;
        }
    }
}
