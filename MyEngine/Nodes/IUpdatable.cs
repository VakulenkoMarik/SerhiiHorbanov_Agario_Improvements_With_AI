namespace MyEngine.Nodes;

public interface IUpdatable
{
    UpdateLayer UpdateLayer { get; }
    public void Update(in UpdateInfo info);
}