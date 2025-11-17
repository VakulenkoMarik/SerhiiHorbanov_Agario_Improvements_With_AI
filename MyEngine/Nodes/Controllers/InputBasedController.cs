using MyEngine.MyInput;

namespace MyEngine.Nodes.Controllers;

public class InputBasedController<T> : Controller<T>, IDisposable where T : Node
{
    protected InputListener Input;

    protected InputBasedController(InputListener input)
        => Input = input;
    
    public virtual void Dispose()
        => Input?.Dispose();
}