using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestCommand : IRequest, IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
