using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistence;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Shared
{
    public class BaseLeaveRequestValidator : AbstractValidator<BaseLeaveRequest>
    {
        private readonly ILeaveTypeRepository _typeRepository;

        public BaseLeaveRequestValidator(ILeaveTypeRepository typeRepository)
        {
            _typeRepository = typeRepository;

            RuleFor(p => p.StartDate)
                .LessThan(p => p.EndDate)
                .WithMessage("{PropertyName} must be before {ComparisonValue}");

            RuleFor(p => p.EndDate)
                .GreaterThan(p => p.StartDate)
                .WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(LeaveTypeMustExist)
                .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken arg2)
        {
            var leaveType = await _typeRepository.GetByIdAsync(id);

            return leaveType != null;
        }
    }
}
