namespace MyEngine.ConfigSystem;

public static class FilePathsLibrary
{
    private static readonly Dictionary<string, string> FilePaths = new();
    
    public static string GetPath(string name)
        => FilePaths[name];

    public static void LoadAndStorePathsFromFile(string filePath)
        => ConfigLoader.LoadIntoDictionary(filePath, FilePaths);

    public static void LoadPaths()
    {
        LoadPathConfigsPaths();
        LoadNormalConfigsPaths();
    }

    private static void LoadNormalConfigsPaths()
    {
        LoadAndStorePathsFromFile("Resources/Configs/NormalConfigsPaths.cfg");
    }

    private static void LoadPathConfigsPaths()
    {
        List<string> pathConfigFiles = ConfigLoader.LoadList("Resources/Configs/PathConfigsPaths.cfg");

        foreach (string path in pathConfigFiles)
        {
            if (path.Length == 0)
                continue;
            ConfigLoader.LoadIntoDictionary(path, FilePaths);
        }
    }
}