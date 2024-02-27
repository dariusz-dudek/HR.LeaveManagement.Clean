using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails
{
    public class GetLeaveAllocationDetailsHandler : IRequestHandler<GetLeaveAllocationDetailsQuery, LeaveAllocationDetailsDto>
    {
        private readonly ILeaveAllocationRepository _allocationRepository;
        private readonly IMapper _mapper;

        public GetLeaveAllocationDetailsHandler(ILeaveAllocationRepository allocationRepository, IMapper mapper)
        {
            _allocationRepository = allocationRepository;
            _mapper = mapper;
        }
        public async Task<LeaveAllocationDetailsDto> Handle(GetLeaveAllocationDetailsQuery request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await _allocationRepository.GetLeaveAllocationWithDetails(request.Id);

            if (leaveAllocation == null)
            {
                throw new NotFoundException(nameof(LeaveRequest), request.Id);
            }

            return _mapper.Map<LeaveAllocationDetailsDto>(leaveAllocation);
        }
    }
}
