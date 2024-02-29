using JetBrains.Annotations;
using MediatR;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Employees;

public static class Create
{
    public record Command(Args Args) : IRequest;
    
    [PublicAPI]
    public class Args
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }
    }
    
    public class Handler(IEmployeesManager employeesManager)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var args = new Employee()
            {
                Id = new EmployeeId(request.Args.Id),
                Role = request.Args.Role
            };
            await employeesManager.Create(args);
        }
    }
}