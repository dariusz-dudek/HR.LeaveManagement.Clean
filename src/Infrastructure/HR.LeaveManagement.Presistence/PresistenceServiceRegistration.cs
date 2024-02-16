using System.Net.Security;
using HR.LeaveManagement.Application.Contracts.Presistence;
using HR.LeaveManagement.Presistence.DatabaseContext;
using HR.LeaveManagement.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Presistence;
public static class PresistenceServiceRegistration
{
    public static IServiceCollection AddPresistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HrDatabaseContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("HrDatabaseConnectionString"));
            // "Server=localhost:5432;User Id=postgres;Password=postgrespw;Database=auctions"
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

        return services;
    }
}
