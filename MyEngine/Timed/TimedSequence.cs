namespace MyEngine.Timed;

public sealed class TimedSequence<T>
{
    public record struct Element(float Time, T Value)
    {
        public static implicit operator Element((float time, T val) tuple)
            => new(tuple.time, tuple.val);
    }
    
    private SortedList<long, T> _elements;

    public Action<T> OnElementDue;
    public Action OnFinished;
    
    private bool _isPlaying;
    private long _now;
    private long _tickOfStart;
    private long _tickOfNextCalled;
    private int _indexOfNextCalled;

    public TimedSequence()
    {
        _elements = new();
        _isPlaying = false;
        OnFinished = Stop;
    }

    // DOES NOT copy OnFinished
    public TimedSequence(TimedSequence<T> other)
    {
        _elements = new(other._elements);
        _isPlaying = other._isPlaying;
        OnElementDue = other.OnElementDue;
    }
    
    public TimedSequence(List<Element> elements) : this()
        => elements.ForEach(AddElement);
    public TimedSequence(Action<T> onElementDue) : this()
        => OnElementDue = onElementDue;
    public TimedSequence(List<Element> elements, Action<T> onElementDue) : this(elements)
        => OnElementDue = onElementDue;
    
    
    public void AddElement(Element element)
    {
        long timeSinceStart = (long)(element.Time * TimeSpan.TicksPerSecond);
        _elements.Add(timeSinceStart, element.Value);
    }

    public void Play()
    {
        _isPlaying = true;
        _tickOfStart = DateTime.Now.Ticks;
        UpdateTickOfNextCalled(); 
    }
    
    public void Stop()
        => _isPlaying = false;

    public void Restart()
    {
        SetTime(0);
        Play();
    }
    
    public void SetTime(float t)
    {
        long ticks = (long)(t * TimeSpan.TicksPerSecond);
        _tickOfStart = _now - ticks;

        _indexOfNextCalled = GetFrameIndexByTick(ticks);
        _tickOfNextCalled = _elements.Keys[_indexOfNextCalled];
        
        UpdateTickOfNextCalled();
    }
    
    public void Update()
    {
        if (!_isPlaying)
            return;
        
        _now = DateTime.Now.Ticks;
        
        while (ShouldProceed())
            Proceed();
    }

    private int GetFrameIndexByTick(long ticks)
    {
        int i = 0;
        while (i < _elements.Count)
        {
            if (_elements.Keys[i] > ticks)
            {
                i--;
                break;
            }

            i++;
        }

        return ClampIndex(i);
    }

    private int ClampIndex(int i)
    {
        if (i < 0)
            return 0;
        if (i >= _elements.Count)
            return _elements.Count - 1;
        return i;
    }

    private bool ShouldProceed() 
        => _tickOfNextCalled < _now && _isPlaying;

    private void Proceed()
    {
        InvokeCurrent();

        _indexOfNextCalled++;

        if (_indexOfNextCalled >= _elements.Count)
        {
            Stop();
            OnFinished?.Invoke();
            return;
        }
        
        UpdateTickOfNextCalled(); 
    }

    private void UpdateTickOfNextCalled()
    {
        _tickOfNextCalled = _tickOfStart + _elements.Keys[_indexOfNextCalled];
    }

    private void InvokeCurrent() 
        => OnElementDue?.Invoke(_elements.Values[_indexOfNextCalled]);
}