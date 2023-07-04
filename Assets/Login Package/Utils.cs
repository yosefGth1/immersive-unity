using System;
using System.Net.Http;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public static class Utils
{
    public const string SCOPED_REGISTRIES_REGEX = "\"scopedRegistries\\\": \\[(.|\\n)*\\]";

    public const string PACKAGE_SCOPE = "com.immersiveinteractive";

    internal const string IP_ADDRESS = "https://code.immersiveinteractive.co.uk:4873/";
    private const string TOML_FILE = ".upmconfig.toml";

    internal static readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri(IP_ADDRESS),
    };

    /// <summary>
    /// Finds the full path for the upmconfig.toml file
    /// </summary>
    /// <returns>Full path for the upmconfig.toml file</returns>
    public static string FindUpmConfigPath()
        => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/" + TOML_FILE;

    /// <summary>
    /// Finds the full path for the manifest.json file
    /// </summary>
    /// <returns></returns>
    public static string FindPackageManifestPath()
        => Path.GetFullPath(Application.dataPath + "/../Packages/manifest.json");

    private const int NPM_AUTH_PREFIX_LENGTH = 10;
    private const int NPM_AUTH_SUFFIX_LENGTH = 2;

    private const int NPM_TOKEN_SETTING_LENGTH = 10;

    public static bool IsUserLoggedIn()
    {
        string configPath = FindUpmConfigPath();
        if (!File.Exists(configPath))
            return false;

        string contents = File.ReadAllText(configPath);
        string[] lines = contents.Split("\n");

        string npmAuthIP = lines[0];
        npmAuthIP = npmAuthIP.Remove(0, NPM_AUTH_PREFIX_LENGTH);
        npmAuthIP = npmAuthIP.Remove(npmAuthIP.Length - NPM_AUTH_SUFFIX_LENGTH);

        if(npmAuthIP != IP_ADDRESS) return false;

        string token = lines[1];
        if (token.Length == NPM_TOKEN_SETTING_LENGTH) return false;

        return true;
    }

    [Serializable]
    public class HttpRequestCallback
    {
        public string error;
        public string ok;
        public string token;
    }

    [Serializable]
    public class ScopedRegistry
    {
        public string name;
        public string url;
        public string[] scopes;
    }

    [Serializable]
    public class PackageManifest
    {
        public ScopedRegistry[] scopedRegistries;
    }

    public static ScopedRegistry[] GetScopedRegistries()
    {
        // Create an array of ScopedRegistry from manifest.json contents using JsonUtility
        string manifestFilePath = FindPackageManifestPath();
        string manifestFileContents = File.ReadAllText(manifestFilePath);
        var manifest = JsonUtility.FromJson<PackageManifest>(manifestFileContents);
        if (manifest.scopedRegistries == null)
            return new ScopedRegistry[0];
        return manifest.scopedRegistries;
    }

    /// <summary>
    /// Used to take JSON generated from JsonUtility.ToJson with prettify enabled, and modify it further to match the json in manifest.json
    /// </summary>
    /// <param name="json">Generated json</param>
    /// <returns>The JSON with formatting that matches manifest.json</returns>
    public static string CompletePrettifyJson(string json)
    {
        // Keeps the formatting the same as the rest of the manifest.json file and removes extra brackets
        string newJson = Regex.Replace(json, "  ", " ");
        newJson = newJson.Remove(0, 4);
        newJson = newJson.Remove(newJson.Length - 2);
        return newJson;
    }

    /// <summary>
    /// Tries to find the package scope inside the manifest.json
    /// </summary>
    /// <returns>Returns true if the scopedRegistry has been set up.</returns>
    public static bool ManifestHasImmersiveScopedRegistry()
    {
        string manifestPath = FindPackageManifestPath();
        string manifest = File.ReadAllText(manifestPath);
        return manifest.Contains(PACKAGE_SCOPE);
    }

    /// <summary>
    /// Tries to find the scopedRegistries array inside the manifest.json
    /// </summary>
    /// <returns>Returns true if there are any scoped registries.</returns>
    public static bool ManifestHasScopedRegistries()
    {
        string manifestPath = FindPackageManifestPath();
        string manifest = File.ReadAllText(manifestPath);
        return manifest.Contains("scopedRegistries");
    }
}
