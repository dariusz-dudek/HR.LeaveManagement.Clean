using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _allocationRepository;
        private readonly ILeaveTypeRepository _typeRepository;

        public CreateLeaveAllocationCommandHandler(IMapper mapper, ILeaveAllocationRepository allocationRepository, ILeaveTypeRepository typeRepository)
        {
            _mapper = mapper;
            _allocationRepository = allocationRepository;
            _typeRepository = typeRepository;
        }
        public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationCommandValidator(_typeRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid Leave Allocation Request", validationResult);
            }

            // Get leave type for allocations
            var leaveType = await _typeRepository.GetByIdAsync(request.LeaveTypeId);

            // Get Employees

            // Get Period

            // Assign Allocations
            var leaveAllocation = _mapper.Map<Domain.LeaveAllocation>(request);
            await _allocationRepository.CreateAsync(leaveAllocation);

            return Unit.Value;
        }
    }
}
