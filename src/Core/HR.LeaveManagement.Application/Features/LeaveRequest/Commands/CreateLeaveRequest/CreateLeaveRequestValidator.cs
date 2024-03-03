using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
    {
        private readonly ILeaveTypeRepository _typeRepository;

        public CreateLeaveRequestValidator(ILeaveTypeRepository typeRepository)
        {
            _typeRepository = typeRepository;

            Include(new BaseLeaveRequestValidator(_typeRepository));
        }
    }
}
