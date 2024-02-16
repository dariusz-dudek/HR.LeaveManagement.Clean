using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
//public record GetLeaveTypeDeteilsQuery(int Id) : IRequest<LeaveTypeDetailsDto>;
public class GetLeaveTypeDeteilsQuery : IRequest<LeaveTypeDetailsDto>
{
    public int Id { get; set; }
}
