using MyEngine.ConfigSystem;
using MyEngine.MyInput;
using SFML.Graphics;
using MyEngine.SoundSystem;

namespace MyEngine;

public abstract class Game
{
    protected RenderWindow Window { get; private set; }
    protected SceneManager Scenes { get; private set; }
    protected InputSystem Input { get; private set; }
    protected FrameTiming Time { get; private set; }

    public void Run()
    {
        Initialization();
        
        while (ContinueGame())
        {
            Render();
            ProcessInput();
            Update();
            Timing();
        }
    }

    private void Initialization()
    {
        InitializeConfigsPaths();
        InitializeWindow();
        InitializeSceneManager();
        InitializeInput();
        InitializeTiming();
        
        GameSpecificInitialization();
    }

    private void InitializeConfigsPaths()
        => FilePathsLibrary.LoadPaths();

    private void InitializeSceneManager()
    {
        Scenes = new SceneManager();
    }

    private void InitializeTiming()
    {
        Time = new();
        Time.UpdateLastTimingTick();
    }

    private void InitializeInput()
    {
        Input = InputSystem.CreateInputSystem();
        MouseWheel.AddListenerTo(Window);
        MouseInput.AddListenerTo(Window);
    }

    private void InitializeWindow()
    {
        WindowConfigs configs = ConfigLoader.LoadFromFile<WindowConfigs>("window configs");
        
        Window = new (new(configs.Size.X, configs.Size.Y), configs.Name);
        Window.Closed += (sender, args) => Window.Close();
    }

    protected abstract void GameSpecificInitialization();
    
    private bool ContinueGame()
        => Window.IsOpen;

    private void Render()
    {
        Window.Clear();
        Scenes.Render();
        Window.Display();
    }

    private void ProcessInput()
    {
        Window.DispatchEvents();
        
        Input.Update();
        
        Scenes.ProcessInput();
    }

    private void Update()
    {
        Scenes.Update(Time);
        Input.ResolveCallbacks();
        SoundManager.UpdatePlayingSounds();
    }

    private void Timing()
        => Time.Timing();
}