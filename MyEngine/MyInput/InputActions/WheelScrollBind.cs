namespace MyEngine.MyInput.InputActions;

public class WheelScrollBind : InputAction
{
    private bool _isDown;
    private float _lastRememberedWheelDeltaSinceStart;
    
    private Action _onScrolledCorrectWay;
    
    public WheelScrollBind(string name, bool isDown) : base(name)
    {
        _isDown = isDown;
        _lastRememberedWheelDeltaSinceStart = MouseWheel.DeltaSinceStart;
    }

    protected override bool ProcessIsActive()
    {
        if (_lastRememberedWheelDeltaSinceStart == MouseWheel.DeltaSinceStart)
            return false;

        _lastRememberedWheelDeltaSinceStart = MouseWheel.DeltaSinceStart;
        return (MouseWheel.LastDelta < 0) == _isDown;
    }

    public void AddCallback(Action action)
        => _onScrolledCorrectWay += action;
    public void ResetCallbacks(Action newValue = null)
        => _onScrolledCorrectWay = newValue;
    
    public override void Resolve()
    {
        if (IsActive)
            _onScrolledCorrectWay?.Invoke();
    }
}