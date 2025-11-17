using MyEngine.MyInput.InputActions;
using MyEngine.Utils;

namespace MyEngine.MyInput;

public class InputListener : IDisposable
{
    public InputSystem System { private get; set; }
    private readonly List<InputAction> _inputActions;
    private readonly List<InputAction> _actionsToAdd;

    public InputListener()
    {
        _inputActions = new();
        _actionsToAdd = new();
    }

    public void Update()
    {
        UpdateInputActions();
        AddActionsToAdd();
    }
    
    public void ResolveCallbacks()
    {
        foreach (InputAction each in _inputActions)
            each.Resolve();
    }
    
    private void UpdateInputActions()
    {
        foreach(InputAction each in _inputActions)
            each.Update();
    }

    public T GetAction<T>(string name) where T : InputAction
    {
        T result = GetByName<T>(name, _inputActions);
        
        if (result != null)
            return result;
        
        return GetByName<T>(name, _actionsToAdd);
    }

    private T GetByName<T>(string name, List<InputAction> list) where T : InputAction
    {
        foreach (InputAction action in list)
        {
            if (action.Name == name)
                return action as T;
        }

        return null;
    }

    public T AddAction<T>(T action) where T : InputAction
    {
        _actionsToAdd.Add(action);
        return action;
    }

    public void Dispose()
        => System.RemoveListener(this);
    
    private void AddActionsToAdd()
    {
        while (_actionsToAdd.Count > 0)
        {
            _inputActions.Add(_actionsToAdd[0]);
            _actionsToAdd.SwapRemoveAt(0);
        }
    }
}