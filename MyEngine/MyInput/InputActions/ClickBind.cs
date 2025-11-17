using SFML.Window;

namespace MyEngine.MyInput.InputActions;

public class ClickBind : HoldableAction
{
    public Mouse.Button Button;

    public ClickBind(string name, Mouse.Button button) : base(name)
    {
        Button = button;
    }

    protected override bool ProcessIsActive()
    {
        return Mouse.IsButtonPressed(Button);
    }
}