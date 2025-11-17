using SFML.Audio;

namespace MyEngine.SoundSystem;

public class SoundOptions
{
    public bool Loop;
    public float Pitch;
    public float Volume;
    public TimeSpan Offset;

    public SoundOptions(TimeSpan offset, bool loop = false, float pitch = 1, float volume = 100)
    {
        Offset = offset;
        Loop = loop;
        Pitch = pitch;
        Volume = volume;
    }

    public SoundOptions(bool loop = false, float pitch = 1, float volume = 100) : 
        this(TimeSpan.Zero, loop, pitch, volume)
    { }
    
    public virtual Sound ApplyToSound(Sound sound)
    {
        sound.Loop = Loop;
        sound.Pitch = Pitch;
        sound.Volume = Volume;

        return sound;
    }
    
    private Music ApplyToMusic(Music music)
    {
        music.Loop = Loop;
        music.Pitch = Pitch;
        music.Volume = Volume;

        return music;
    }
    
    public Sound ApplyWithOffsetToSound(Sound sound)
    {
        ApplyToSound(sound);
        sound.PlayingOffset = Offset;

        return sound;
    }

    public Music ApplyWithOffsetToMusic(Music music)
    {
        ApplyToMusic(music);
        music.PlayingOffset = Offset;

        return music;
    }
};