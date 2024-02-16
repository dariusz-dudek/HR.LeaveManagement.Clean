namespace HR.LeaveManagement.Domain.Common;
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DaeCreated { get; set; }
    public DateTime? DateModified { get; set; }
}
