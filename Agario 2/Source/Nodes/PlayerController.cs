using MyEngine.MyInput;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes;
using MyEngine.Nodes.Controllers;
using SFML.Window;

namespace Agario_2.Nodes;

public class PlayerController : InputBasedController<Player>, IProcessesInput
{
    private PlayerController() : base(new InputListener())
    { }
    
    protected override void SetControlled(Player newControlled)
    {
        base.SetControlled(newControlled);

        Input.GetAction<KeyBind>("dash").ResetOnStartedCallbacks();
        Input.GetAction<KeyBind>("body swap").ResetOnStartedCallbacks();

        if (newControlled == null)
            return;
        
        Input.GetAction<KeyBind>("dash").AddOnStartedCallback(Controlled.Dash);
        Input.GetAction<KeyBind>("body swap").AddOnStartedCallback(Controlled.SwapBodies);
    }

    public static PlayerController CreatePlayerController(InputSystem inputSystem, Player player = null)
    {
        PlayerController result = new();

        result.InitializeKeyBinds();
        inputSystem.AddListener(result.Input);

        if (player != null)
            result.Controlled = player;
        
        return result;
    }

    private void InitializeKeyBinds()
    {
        Input.AddAction(new KeyBind("dash", Keyboard.Key.Space));
        Input.AddAction(new KeyBind("body swap", Keyboard.Key.F));
    }
    
    public void ProcessInput()
    {
        if (Controlled != null) 
            Controlled.WishedDelta = MouseInput.MousePositionFromWindowCenter;
    }

    public override void Dispose()
    {
        Controlled = null;
        base.Dispose();
    }
}