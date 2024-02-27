using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly ILeaveAllocationRepository _allocationRepository;

        public UpdateLeaveAllocationCommandValidator(ILeaveTypeRepository typeRepository, ILeaveAllocationRepository allocationRepository)
        {
            _typeRepository = typeRepository;
            _allocationRepository = allocationRepository;

            RuleFor(p => p.NumberOfDays)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must greater than {ComparisonValue}");

            RuleFor(p => p.Period)
                .GreaterThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(LeaveTypeMustExist)
                .WithMessage("{PropertyName} must exist.");

            RuleFor(p => p.Id)
                .NotNull()
                .MustAsync(LeaveAllocationMustExist)
                .WithMessage("{PropertyName} must be present");
        }

        private async Task<bool> LeaveAllocationMustExist(int id, CancellationToken arg2)
        {
            var leaveAllocation = await _allocationRepository.GetByIdAsync(id);

            return leaveAllocation != null;
        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken arg2)
        {
            var leaveType = await _typeRepository.GetByIdAsync(id);

            return leaveType != null;
        }
    }
}
