using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _typeRepository;

        public CreateLeaveAllocationCommandValidator(ILeaveTypeRepository typeRepository)
        {
            _typeRepository = typeRepository;

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(LeaveTypeMustExist)
                .WithMessage("{PropertyName} doesn't exist.");
        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken arg2)
        {
            var leaveType = await _typeRepository.GetByIdAsync(id);

            return leaveType != null;
        }
    }
}
