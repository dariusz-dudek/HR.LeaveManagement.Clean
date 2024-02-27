using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand, Unit>
    {
        private readonly ILeaveAllocationRepository _allocationRepository;

        public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository allocationRepository)
        {
            _allocationRepository = allocationRepository;
        }
        public async Task<Unit> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await _allocationRepository.GetByIdAsync(request.Id);

            if (leaveAllocation == null)
            {
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);
            }

            await _allocationRepository.DeleteAsync(leaveAllocation);

            return Unit.Value;
        }
    }
}
