using SFML.Window;

namespace MyEngine.MyInput.InputActions;

public class KeyBind : HoldableAction
{
    public Keyboard.Key Key;

    public KeyBind(string name, Keyboard.Key key) : base(name)
    {
        Key = key;
    }

    protected override bool ProcessIsActive()
        => Keyboard.IsKeyPressed(Key);
}