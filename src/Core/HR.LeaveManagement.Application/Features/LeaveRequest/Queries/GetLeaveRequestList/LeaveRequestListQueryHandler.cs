using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList
{
    public class LeaveRequestListQueryHandler : IRequestHandler<GetLeaveRequestListQuery, List<LeaveRequestListDto>>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public LeaveRequestListQueryHandler(ILeaveRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }
        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
        {
            // Check if it is logged in employee

            var leaveRequest = await _requestRepository.GetLeaveRequestsWithDetails();
            var requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequest);

            // Fill requests with employee information
            return requests;
        }
    }
}
