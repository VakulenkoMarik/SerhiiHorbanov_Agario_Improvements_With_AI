using MyEngine.Utils;
using SFML.Audio;

namespace MyEngine.SoundSystem;

public static class SoundManager
{
    public static List<Sound> PlayingSounds = new();
    public static List<Music> PlayingMusic = new();

    public static Music CreateMusic(string name)
    {
        Music result = new(SoundLibrary.GetMusicPath(name));
        PlayingMusic.Add(result);
        
        return result;
    }
    
    public static Sound CreateSound(string name)
    {
        SoundBuffer buffer = SoundLibrary.GetSound(name);
        Sound sound = new Sound(buffer);
        
        PlayingSounds.Add(sound);
        
        return sound;
    }

    public static void UpdatePlayingSounds()
    {
        for (int i = 0; i < PlayingSounds.Count; i++)
        {
            if (PlayingSounds[i].ShouldBeRemoved())
                PlayingSounds.SwapRemoveAt(i--);
        }

        for (int i = 0; i < PlayingMusic.Count; i++)
        {
            if (PlayingMusic[i].ShouldBeRemoved())
                PlayingMusic.SwapRemoveAt(i--);
        }
    }

    private static bool ShouldBeRemoved(this Sound sound)
        => sound.Status == SoundStatus.Stopped;
    private static bool ShouldBeRemoved(this Music music)
        => music.Status == SoundStatus.Stopped;

    public static Sound WithOptions(this Sound sound, string soundOptionsName)
        => SoundLibrary.GetSoundOptions(soundOptionsName).ApplyWithOffsetToSound(sound);
    public static Sound WithOptions(this Sound sound, SoundOptions options)
        => options.ApplyWithOffsetToSound(sound);
    
    public static Music WithOptions(this Music music, string soundOptionsName)
        => SoundLibrary.GetSoundOptions(soundOptionsName).ApplyWithOffsetToMusic(music);
    public static Music WithOptions(this Music music, SoundOptions options)
        => options.ApplyWithOffsetToMusic(music);
    
    public static Sound WithRandomizedPitch(this Sound sound, float min, float max)
    {
        float pitch = MyRandom.GetFloatInRange(min, max);
        sound.Pitch = pitch;

        return sound;
    }
}