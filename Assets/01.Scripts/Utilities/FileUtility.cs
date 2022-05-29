using UnityEngine;

public static class FileUtility
{
    public const string MAP_PATH = "Data/Level";

    private const string DATA_PATH = "Data/Player";
    private const string DATA_NAME = "DataObject";
    public const string ASSET_EXTENTION = ".asset";
    
    private const string ASSET_PREFIX = "Assets/Resources";
    private const string RESOURCE_PREFIX = "Resources";
    
    public static string GetDefaultMapPath ()
    {
        return Application.dataPath + '/' + RESOURCE_PREFIX + '/' + MAP_PATH;
    }

    public static string GetMapPathFromAssetsFolder ()
    {
        return ASSET_PREFIX + "/" + MAP_PATH;
    }
    
    public static string GetDataPathFromAssetsFolder () => ASSET_PREFIX + "/" + DATA_PATH + "/" + DATA_NAME;

    public static string GetDataPath () => DATA_PATH + "/" + DATA_NAME;
}
