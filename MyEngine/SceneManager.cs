using MyEngine.Nodes;
using MyEngine.Utils;

namespace MyEngine;

public class SceneManager
{
    private Dictionary<string, SceneNode> _scenes;
    
    private List<(string name, SceneNode scene)> _scenesToAdd;

    public SceneManager()
    {
        _scenesToAdd = new();
        _scenes = new();
    }

    private Dictionary<string, SceneNode>.ValueCollection Values 
        => _scenes.Values;
    
    public void Render()
    {
        foreach (SceneNode scene in Values)
        {
            if (scene.IsRendered)
                scene.RenderScene();
        }
    }
    
    public void Update(FrameTiming timing)
    {
        foreach (SceneNode scene in Values)
        {
            if(scene.IsProcessed) 
                scene.UpdateScene(timing);
        }

        RemoveKilledScenes();
        AddScenesToAdd();
    }

    private void AddScenesToAdd()
    {
        while (_scenesToAdd.Count > 0)
        {
            _scenes.Add(_scenesToAdd[0].name, _scenesToAdd[0].scene);
            _scenesToAdd.SwapRemoveAt(0);
        }
    }

    private void RemoveKilledScenes()
    {
        for (int i = 0; i < _scenes.Count; i++)
        {
            if (!Values.ElementAt(i).IsKilled) 
                continue;
            Values.ElementAt(i).KillImmediately();
            _scenes.Remove(_scenes.ElementAt(i).Key);
            i--;
        }
    }

    public void ProcessInput()
    {
        foreach (SceneNode scene in Values)
        {
            if (scene.IsProcessed)
                scene.ProcessInput();
        }
    }

    public void Add(string name, SceneNode scene)
        => _scenesToAdd.Add((name, scene));

    public void KillAllScenes()
    {
        foreach (SceneNode scene in Values)
            scene.Kill();
    }
    
    public SceneNode this[string name]
    {
        get
        {
            foreach ((string name, SceneNode scene) tuple in _scenesToAdd)
            {
                if (name == tuple.name)
                    return tuple.scene;
            }
            
            if (_scenes.ContainsKey(name)) 
                return _scenes[name];

            return null;
        }
    } 
}