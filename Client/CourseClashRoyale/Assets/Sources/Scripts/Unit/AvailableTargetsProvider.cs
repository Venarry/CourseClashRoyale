using System.Collections.Generic;

public class AvailableTargetsProvider
{
    private readonly Dictionary<TargetType, bool> _availableTargets = new();

    public void Register(TargetType type, bool canInteract)
    {
        if(_availableTargets.ContainsKey(type) == false)
        {
            _availableTargets.Add(type, canInteract);
        }
        else
        {
            _availableTargets[type] = canInteract;
        }
    }

    public bool CanInteract(ITarget target)
    {
        TargetType targetType = target.Type;

        if (_availableTargets.ContainsKey(targetType) == false)
        {
            return false;
        }

        return _availableTargets[targetType];
    }
}
