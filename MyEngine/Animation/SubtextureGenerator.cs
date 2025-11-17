using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

public struct SubtextureGenerator
{
    private readonly Vector2i _size;
    private readonly Vector2i _offset;
    private readonly Vector2i _delta;
    //rows not used when set to 0
    private readonly int _rowLength;

    public SubtextureGenerator(Vector2i size, Vector2i offset, int rowLength, Vector2i delta)
    {
        _size = size;
        _offset = offset;
        _rowLength = rowLength;
        _delta = delta;
    }
    
    public SubtextureGenerator(Vector2i size, Vector2i offset, int rowLength = 0) : 
        this(size, offset, rowLength, size) 
    { }

    public Subtexture GetFrame(Texture texture, int frame)
    {
        int row = 0;
        int column = frame;
        
        if (_rowLength != 0)
        {
            row = frame / _rowLength;
            column %= _rowLength;
        }
            
        Vector2i localOffset = _offset + new Vector2i(column * _delta.X, row * _delta.Y);
        IntRect rect = new(_offset + localOffset, _size);
        
        return new(texture, rect);
    }
}