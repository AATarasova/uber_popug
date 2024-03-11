namespace Dashboard.AccessRights;

public class AccessRightsManager
{
    private readonly IReadOnlyCollection<Permissions> _accountant = new[] { Permissions.TasksRatingView };

    public bool HasPermission(Role role, Permissions permission) =>
        role switch
        {
            Role.Administrator => true,
            Role.Developer => false,
            Role.Manager => true,
            Role.Accountant => _accountant.Contains(permission),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
}