using Microsoft.VisualBasic.CompilerServices;

namespace MyEngine;

public sealed class FrameTiming 
{
    public float TargetFps
    {
        set
        {
            _targetFps = value;
            _targetDeltaTicks = (long)(TimeSpan.TicksPerSecond / _targetFps);
            TargetDeltaSeconds = _targetDeltaTicks / (float)TimeSpan.TicksPerSecond;
        }
        get => _targetFps;
    }

    public FrameTiming(float targetFps = 60)
        => TargetFps = targetFps;

    public float TargetDeltaSeconds { get; private set; }
    public float DeltaSeconds { get; private set; }

    private long _targetDeltaTicks; 
    private long _deltaTicks;
    
    private float _targetFps; 
    private long _lastTimingTick;
     
    public void Timing()
    {
        UpdateDeltas();

        if (ShouldSleep())
        {
            Thread.Sleep(GetSleepMilliseconds());
            UpdateDeltas();
        }
        
        UpdateLastTimingTick(); 
    }
    
    public void UpdateLastTimingTick() 
        => _lastTimingTick = DateTime.Now.Ticks; 
    
    private void UpdateDeltas()
    {
        _deltaTicks = DateTime.Now.Ticks - _lastTimingTick;
        DeltaSeconds = (float)_deltaTicks / TimeSpan.TicksPerSecond; 
    }

    private bool ShouldSleep()
        => _targetDeltaTicks > _deltaTicks;
    
    private int GetSleepMilliseconds()
        => (int)(GetSleepTicks() / TimeSpan.TicksPerMillisecond);
    private long GetSleepTicks()
        => _targetDeltaTicks - _deltaTicks;
}