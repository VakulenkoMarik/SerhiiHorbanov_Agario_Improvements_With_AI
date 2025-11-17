namespace MyEngine.MyInput.InputActions;

public abstract class InputAction
{
    public readonly string Name; 
    protected bool IsActive;
    
    protected InputAction(string name)
    {
        Name = name;
        IsActive = false;
    }

    protected abstract bool ProcessIsActive();

    public virtual void Update()
        => IsActive = ProcessIsActive();

    public abstract void Resolve();
}