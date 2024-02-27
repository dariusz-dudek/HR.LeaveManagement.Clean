using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly ILeaveAllocationRepository _allocationRepository;

        public UpdateLeaveAllocationCommandHandler(IMapper mapper, ILeaveTypeRepository typeRepository, ILeaveAllocationRepository allocationRepository)
        {
            _mapper = mapper;
            _typeRepository = typeRepository;
            _allocationRepository = allocationRepository;
        }
        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveAllocationCommandValidator(_typeRepository, _allocationRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid Leave Allocation", validationResult);
            }

            var leaveAllocation = await _allocationRepository.GetByIdAsync(request.Id);

            if (leaveAllocation == null)
            {
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);
            }

            _mapper.Map(request, leaveAllocation);

            await _allocationRepository.UpdateAsync(leaveAllocation);

            return Unit.Value;
        }
    }
}
