namespace Accounting.AccessRights;

public class AccessRightsManager
{
    private readonly IReadOnlyCollection<Permissions> _allPermissions = Enum.GetValues<Permissions>();
    private readonly IReadOnlyCollection<Permissions> _empty = Array.Empty<Permissions>();
    
    public bool HasPermission(Role role, Permissions permission)
    {
        var permissions = role == Role.Developer ? _empty : _allPermissions;
        return permissions.Contains(permission);
    }
}