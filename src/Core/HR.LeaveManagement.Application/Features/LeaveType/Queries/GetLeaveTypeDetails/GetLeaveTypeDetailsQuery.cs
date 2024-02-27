using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
public record GetLeaveTypeDetailsQuery(int Id) : IRequest<LeaveTypeDetailsDto>;
//public class GetLeaveTypeDetailsQuery : IRequest<LeaveTypeDetailsDto>
//{
//    public int Id { get; set; }
//}
