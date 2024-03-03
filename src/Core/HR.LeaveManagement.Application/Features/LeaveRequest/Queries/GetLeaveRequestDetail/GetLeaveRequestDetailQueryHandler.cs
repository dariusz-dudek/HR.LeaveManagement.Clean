using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail
{
    public class GetLeaveRequestDetailQueryHandler : IRequestHandler<GetLeaveRequestDetailQuery, LeaveRequestDetailsDto>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestDetailQueryHandler(ILeaveRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailQuery request, CancellationToken cancellationToken)
        {
            var leaveRequest =
                _mapper.Map<LeaveRequestDetailsDto>(await _requestRepository.GetLeaveRequestWithDetails(request.Id));

            // Add Employee details as needed

            return leaveRequest;
        }
    }
}
