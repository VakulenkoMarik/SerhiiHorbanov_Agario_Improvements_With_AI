using MyEngine.Utils;

namespace MyEngine.MyInput;

public class InputSystem
{
    public InputListener GlobalListener;
    private List<InputListener> _activeListeners;
    private List<InputListener> _listenersToAdd;
    
    private InputSystem()
    {
        _listenersToAdd = new();
        GlobalListener = new();
        _activeListeners = new();
    }

    public static InputSystem CreateInputSystem()
    {
        InputSystem result = new();
        
        result.AddListener(result.GlobalListener);

        return result;
    }

    public void AddListener(InputListener listener)
        => _listenersToAdd.Add(listener);

    private void AddListenerImmediately(InputListener listener)
    {
        if (!_activeListeners.Contains(listener))
        {
            _activeListeners.Add(listener);
            listener.System = this;
        }
    }

    private void AddListenersToAdd()
    {
        while (_listenersToAdd.Count > 0)
        {
            AddListenerImmediately(_listenersToAdd[0]);
            _listenersToAdd.SwapRemoveAt(0);
        }
    }
    
    public void RemoveListener(InputListener listener)
        => _activeListeners.SwapRemove(listener);

    public void Update()
    {
        AddListenersToAdd();
        
        foreach (InputListener each in _activeListeners)
            each.Update();
    }

    public void ResolveCallbacks()
    {
        foreach (InputListener each in _activeListeners)
            each.ResolveCallbacks();
    }
}