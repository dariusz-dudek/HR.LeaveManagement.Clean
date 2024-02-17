namespace HR.LeaveManagement.Application.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) : base($"{name} ({key} was not found)")
    {
        
    }

    public NotFoundException(string name, string key) : base($"{name} ({key} was not found)")
    {
        
    }
    public NotFoundException(Type entityType, string key) : base($"{entityType.Name} ({key} was not found)")
    {
        
    }

    public NotFoundException(Type entityType, string[] keys) 
        : base($"{entityType.Name} (one of thous keys: {string.Join(", ", keys)} were not found)")
    {
        
    }
}
