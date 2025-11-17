using MyEngine.Timed;

namespace MyEngine.Nodes;

public sealed class SequenceNode<T> : Node, IUpdatable
{
    public UpdateLayer UpdateLayer { get; set; }
    
    public TimedSequence<T> Sequence;

    private SequenceNode(TimedSequence<T> sequence)
    {
        Sequence = sequence;
        UpdateLayer = UpdateLayer.Normal;
    } 

    public static SequenceNode<T> CreateSequenceNode()
        => new (new());
    public static SequenceNode<T> CreateSequenceNode(List<TimedSequence<T>.Element> elements)
        => new(new(elements));
    public static SequenceNode<T> CreateSequenceNode(TimedSequence<T> sequence)
        => new(sequence);
    
    public void Update(in UpdateInfo info)
        => Sequence?.Update();
}