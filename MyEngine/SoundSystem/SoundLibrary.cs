using MyEngine.ConfigSystem;
using SFML.Audio;

namespace MyEngine.SoundSystem;

public class SoundLibrary
{
    private static readonly Dictionary<string, SoundBuffer> Buffers = new();
    private static readonly Dictionary<string, string> MusicNameToPath = new();
    private static readonly Dictionary<string, SoundOptions> StoredSoundOptions = new();
    
    public static void LoadAndStoreSoundFromPathsLibrary(string name)
        => Buffers.Add(name, new(FilePathsLibrary.GetPath(name)));
    public static void LoadAndStoreSound(string path, string name)
        => Buffers.Add(name, new(path));
    public static SoundBuffer GetSound(string name)
        => Buffers[name];
    
    public static void StoreMusicFromPathsLibrary(string name)
        => MusicNameToPath.Add(name, new(FilePathsLibrary.GetPath(name)));
    public static void StoreMusic(string path, string name)
        => MusicNameToPath.Add(name, path);
    public static string GetMusicPath(string name)
        => MusicNameToPath[name];

    public static void StoreSoundOptions(SoundOptions options, string name)
        => StoredSoundOptions.Add(name, options);
    public static SoundOptions GetSoundOptions(string soundOptionsName)
        => StoredSoundOptions[soundOptionsName];
}