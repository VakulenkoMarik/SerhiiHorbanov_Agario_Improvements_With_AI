using System.Data;
using System.Reflection;
using MyEngine.Utils;

namespace MyEngine.ConfigSystem;

public static class ConfigLoader
{
    public static ConfigValueParser ValueParser { private get; set; } = new ConfigValueParser();

    public static void LoadIntoDictionary(string fileName, Dictionary<string, string> dictionary)
    {
        if (!File.Exists(fileName))
            ThrowNoFileException(fileName);
        
        StreamReader stream = new(fileName);

        while (!stream.EndOfStream)
        {
            string line = ReadLineWithoutComments(stream);
            
            if (line.Contains('='))
                AddAssignmentToDictionary(line, dictionary);
        }
    }

    public static List<string> LoadList(string filePath)
    {
        if (!File.Exists(filePath))
            ThrowNoFileException(filePath);
        
        StreamReader stream = new(filePath);
        
        List<string> result = new();
        
        while (!stream.EndOfStream)
            result.Add(ReadLineWithoutComments(stream));

        return result;
    }
    
    private static void AddAssignmentToDictionary(string line, Dictionary<string, string> dictionary)
    {
        string[] split = line.Split('=');
        dictionary.Add(split[0].Trim(), split[1].Trim());
    }
    
    public static T LoadFromFile<T>(string pathName) where T : new()
    {
        string path = FilePathsLibrary.GetPath(pathName);
        
        if (!File.Exists(path))
            ThrowNoFileException(path);
        
        StreamReader file = new(path);

        T result = new();
        
        while (!file.EndOfStream)
        {
            string line = ReadLineWithoutComments(file);
            
            if (line.Contains('='))
                TryAssignField(ref result, line);
        }

        return result;
    }

    private static void ThrowNoFileException(string fileName)
        => throw new Exception($"No file {fileName}");
    
    private static void TryAssignField<T>(ref T assignTo, string line)
    { 
        (string fieldName, string value) = GetNameAndValueOfAssignment(line);

        Type type = typeof(T);
        FieldInfo field = type.GetField(fieldName);
        
        if (field == null)
            ThrowNoFieldException(fieldName, type);
        
        TryAssignField(ref assignTo, field, value);
    }
    
    private static void TryAssignField<T>(ref T assignTo, FieldInfo field, string value)
    {
        object parsedValue = ValueParser.ParseFieldValue(field.FieldType, value);
        
        field.SetValue(assignTo, parsedValue);
    }
    
    private static (string fieldName, string value) GetNameAndValueOfAssignment(in string assignment)
    {
        string[] split = assignment.Split('=');
        string fieldName = split[0].Trim();
        string value = split[1].TrimEnd().TrimStart();
        
        return (fieldName, value);
    }
    
    public static void LoadStaticFieldsFromFile(Type type, string pathName)
    {
        string path = FilePathsLibrary.GetPath(pathName);    
        
        if (!File.Exists(path))
            ThrowNoFileException(path);
        
        StreamReader file = new(path);

        while (!file.EndOfStream)
        {
            string line = ReadLineWithoutComments(file);
            
            if (line.Contains('='))
                TryAssignStaticField(type, line);
        }
    }

    // Assignment is supposed to look like this: "FieldName = value"
    private static void TryAssignStaticField(Type type, string assignment)
    {
        (string fieldName, string value) = GetNameAndValueOfAssignment(assignment);
        
        TryAssignStaticField(type, fieldName, value);
    }
    
    private static void TryAssignStaticField(Type type, string fieldName, string value)
    {
        FieldInfo field = type.GetField(fieldName);

        if (field == null)
            ThrowNoFieldException(fieldName, type);
        if (!field.IsStatic)
            throw new Exception($"Field {fieldName} in type {type} is not static. Only static fields can be assigned with ConfigLoader");

        object parsedValue = ValueParser.ParseFieldValue(field.FieldType, value);
        field.SetValue(null, parsedValue);
    }

    private static string ReadLineWithoutComments(StreamReader reader)
        => reader.ReadLine().CutOffAfter("//");
    
    private static void ThrowNoFieldException(string fieldName, Type type)
        => throw new Exception($"No field matches the name {fieldName} in assignment for type {type.FullName}");
}