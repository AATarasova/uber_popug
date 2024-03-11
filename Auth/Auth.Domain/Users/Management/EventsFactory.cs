using System.Text.Json;
using System.Text.Json.Serialization;
using SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;
using SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;
using Role = Auth.Domain.Roles.Role;
using EventRole = SchemaRegistry.Schemas.Employees.Role;

namespace Auth.Domain.Users.Management;

public class EventsFactory(
    EmployeeCreatedEventSchemaRegistry employeeCreatedEventSchemaRegistry,
    EmployeeRoleChangedEventSchemaRegistry employeeRoleChangedEventSchemaRegistry)
{
    private const string LastSupportedVersion = "V1";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<string> CreateEmployeeCreatedEvent(Guid employeeId, Role role)
    {        
        var producedEvent = new EmployeeCreatedEvent_V1()
        {
            EmployeeId = employeeId,
            Role = ConvertRole(role),
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        await employeeCreatedEventSchemaRegistry.Validate(serialized, LastSupportedVersion);

        return serialized;
    }
    
    public async Task<string> CreateEmployeeRoleChanged(Guid employeeId, Role role)
    {
        var producedEvent = new EmployeeRoleChangedEvent_V1()
        {
            EmployeeId = employeeId,
            Role = ConvertRole(role),
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        await employeeRoleChangedEventSchemaRegistry.Validate(serialized, LastSupportedVersion);

        return serialized;
    }
    
    private EventRole ConvertRole(Role role) => role switch
    {
          Role.Administrator => EventRole.Administrator,
          Role.Developer => EventRole.Developer,
          Role.Manager => EventRole.Manager,
          Role.Accountant => EventRole.Accountant,
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
    };
}