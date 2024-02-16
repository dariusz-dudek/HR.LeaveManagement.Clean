using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;
public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;
    }

    public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        // retrive domain entity object
        var leaveTypeToDelete = await _leaveTypeRepository.GetByIdAsync(request.Id); 

        // Verify thad record exist
        if(leaveTypeToDelete == null)
        {
            throw new NotFoundException(nameof(LeaveType), request.Id);
        }

        // remove from database
        await _leaveTypeRepository.DeleteAsync(leaveTypeToDelete);

        // Return unit value
        return Unit.Value;
    }
}
