using System.Collections;
using MyEngine.Utils;

namespace MyEngine.Nodes;

public class Node : IEnumerable<Node>
{
    private readonly List<Node> _children;
    public Node Parent { get; private set; }

    private bool _isKilled;

    internal bool IsInScene;
    
    private bool IsRoot
        => Parent == this;
    public bool IsKilled
        => _isKilled;

    protected Node()
    {
        IsInScene = this is SceneNode;
        _isKilled = false;
        _children = new();
        Parent = this;
    }

    public static Node CreateNode()
        => new Node();

    public Node CreateChildNode()
        => AdoptChild(CreateNode());
    
    public bool HasChild(Node child)
        => _children.Contains(child);

    public int CountDescendants()
    {
        int result = 0;
        
        foreach (Node each in _children)
            result += each.CountDescendants();
        
        return result;
    }

    public T GetDescendantOfType<T>() where T : Node
    {
        foreach (Node child in _children)
        {
            if (child is T childAsT)
                return childAsT;
            
            T found = child.GetDescendantOfType<T>();
            if (found != null)
                return found;
        }

        return null;
    }

    public List<T> GetDescendantsOfType<T>() where T : Node
    {
        List<T> result = new();
        
        foreach (Node child in _children)
        {
            result.TryAdd(child);

            List<T> found = child.GetDescendantsOfType<T>();
            if (found != null)
                result.AddRange(found);
        }

        return result;
    }
    
    public T GetChildOfType<T>() where T : Node
    {
        foreach (Node child in _children)
        {
            if (child is T result)
                return result;
        }

        return null;
    }

    public List<T> GetChildrenOfType<T>() where T : Node
    {
        List<T> result = new();
        
        foreach (Node child in _children)
        {
            if (child is T node)
                result.Add(node);
        }

        return result;
    }
    
    public void DetachChild(Node child)
    {
        if (child == this)
            return;
        if (child.Parent != this)
            return;

        if (IsInScene)
            (GetRootNode() as SceneNode)?.UnregisterNodeAndChildren(child);
        
        child.Parent = child;
        _children.Remove(child);
    }

    public void Orphan()
        => Parent.DetachChild(this);

    public void Kill()
        => _isKilled = true;

    public void KillImmediately()
    {
        Orphan();
        
        while (_children.Any())
            _children[0].KillImmediately();
        
        (this as IDisposable)?.Dispose();
    }
    
    protected void KillNodesToKill()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            Node child = _children[i];

            if (child._isKilled)
            {
                child.KillImmediately();
                i--;
                continue;
            }
            
            child.KillNodesToKill();
        }
    }
    
    public Node GetRootNode()
    {
        Node current = this;
        
        while (!current.IsRoot)
            current = current.Parent;

        return current;
    }
    
    public T AdoptChild<T>(T child) where T : Node
    {
        if (child == this || child.Parent == this)
            return child;
        
        child.Parent.DetachChild(child);
        child.Parent = this;
        _children.Add(child);

        if (!child.IsInScene && IsInScene)
            (GetRootNode() as SceneNode)?.RegisterNodeAndChildren(child);
        
        return child;
    }

    public IEnumerator<Node> GetEnumerator()
        => _children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _children.GetEnumerator();
}