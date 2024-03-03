using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestValidator: AbstractValidator<UpdateLeaveRequestCommand>
    {
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly ILeaveRequestRepository _requestRepository;

        public UpdateLeaveRequestValidator(ILeaveTypeRepository typeRepository,
            ILeaveRequestRepository requestRepository)
        {
            _typeRepository = typeRepository;
            _requestRepository = requestRepository;

            Include(new BaseLeaveRequestValidator(_typeRepository));

            RuleFor(p => p.Id)
                .NotNull()
                .MustAsync(LeaveRequestMustExist)
                .WithMessage("{PropertyName} must be present");
        }

        private async Task<bool> LeaveRequestMustExist(int id, CancellationToken arg2)
        {
            var leaveRequest = await _requestRepository.GetByIdAsync(id);

            return leaveRequest != null;
        }
    }
}
