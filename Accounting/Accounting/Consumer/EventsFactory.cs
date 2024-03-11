using System.Text.Json;
using System.Text.Json.Serialization;
using Accounting.Consumer.EmployeeCreated;
using Accounting.Consumer.EmployeeRoleChanged;
using SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;
using SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;
using SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;
using SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

namespace Accounting.Consumer;

public class EventsFactory(
    EmployeeCreatedEventSchemaRegistry employeeCreatedEventSchemaRegistry,
    EmployeeRoleChangedEventSchemaRegistry employeeRoleChangedEventSchemaRegistry,
    TaskStatusChangedEventSchemaRegistry taskStatusChangedEventSchemaRegistry,
    TaskCreatedEventSchemaRegistry taskCreatedEventSchemaRegistry)
{
    private const string LastSupportedVersion = "V1";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters = {
            new JsonStringEnumConverter()
        }
    };
    
    public async Task<EmployeeCreatedEvent_V1> CreateEmployeeCreatedEvent(string value)
    {
        await employeeCreatedEventSchemaRegistry.Validate(value, LastSupportedVersion);
        var parsed = JsonSerializer.Deserialize<EmployeeCreatedEvent_V1>(value, _serializerOptions)
                     ?? throw new InvalidOperationException();
        
        return parsed;
    }
    
    public async Task<EmployeeRoleChangedEvent_V1> CreateEmployeeRoleChanged(string value)
    {
        await employeeRoleChangedEventSchemaRegistry.Validate(value, LastSupportedVersion);
        var parsed = JsonSerializer.Deserialize<EmployeeRoleChangedEvent_V1>(value, _serializerOptions)
                     ?? throw new InvalidOperationException();
        
        return parsed;
    }

    public async Task<TaskCreatedEvent_V1> CreateTaskCreatedEvent(string value)
    {
        await taskCreatedEventSchemaRegistry.Validate(value, LastSupportedVersion);
        var parsed = JsonSerializer.Deserialize<TaskCreatedEvent_V1>(value, _serializerOptions)
                     ?? throw new InvalidOperationException();
        
        return parsed;
    }

    public async Task<TaskStatusChangedEvent_V1> CreateTaskStatusChangedEvent(string value)
    {
        await taskStatusChangedEventSchemaRegistry.Validate(value, LastSupportedVersion);
        var parsed = JsonSerializer.Deserialize<TaskStatusChangedEvent_V1>(value, _serializerOptions)
                     ?? throw new InvalidOperationException();
        return parsed;
    }
}