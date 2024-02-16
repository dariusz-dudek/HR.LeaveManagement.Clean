using System.Reflection;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
    {
        _mapper = mapper;
        _leaveTypeRepository = leaveTypeRepository;
    }
    public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        // Validate incoming data

        // Convert to domain entity object
        var leaveTypeToUpdate = _mapper.Map<Domain.LeaveType>(request);

        // add to database
        await _leaveTypeRepository.UpdateAsync(leaveTypeToUpdate);

        // Return unit value
        return Unit.Value;
    }
}
