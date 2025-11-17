namespace MyEngine.MyInput.InputActions;

public abstract class HoldableAction : InputAction
{
    private bool _wasPressed;

    private Action _onStarted;
    private Action _onHeld;
    private Action _onEnded;
    
    private bool IsStarted
        => !_wasPressed && IsActive;
    private bool IsHeld
        => _wasPressed && IsActive;
    private bool IsEnded
        => _wasPressed && !IsActive;

    protected HoldableAction(string name) : base(name)
    {
        _wasPressed = false;
    }

    public void AddOnStartedCallback(Action callback)
        => _onStarted += callback;
    public void ResetOnStartedCallbacks(Action newValue = null)
        => _onStarted = newValue;

    public void AddOnHeldCallback(Action callback)
        => _onHeld += callback;
    public void ResetOnHeldCallbacks(Action newValue = null)
        => _onHeld = newValue;
    
    public void AddOnEndedCallback(Action callback)
        => _onEnded += callback;
    public void ResetOnEndedCallbacks(Action newValue = null)
        => _onEnded = newValue;

    public void ResetCallbacks()
    {
        ResetOnStartedCallbacks();
        ResetOnHeldCallbacks();
        ResetOnEndedCallbacks();
    }
    
    public override void Update()
    {
        _wasPressed = IsActive;
        
        base.Update();
    }

    public override void Resolve()
    {
        if (IsStarted)
            _onStarted?.Invoke();
        if (IsHeld)
            _onHeld?.Invoke();
        if (IsEnded)
            _onEnded?.Invoke();
    }
}