using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypeDeteilsQuery, LeaveTypeDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
    {
        _mapper = mapper;
        _leaveTypeRepository = leaveTypeRepository;
    }
    public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDeteilsQuery request, CancellationToken cancellationToken)
    {
        // Query the database
        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);

        // verify that record exists
        if (leaveType == null)
        {
            throw new NotFoundException(nameof(LeaveType), request.Id);
        }
            
        // convert data object to DTO object
        var data = _mapper.Map<LeaveTypeDetailsDto>(leaveType);

        // return DTO object
        return data;
    }
}
