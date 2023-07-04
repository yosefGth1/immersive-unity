using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class NpmLogin
{
    private const string TOML_AUTH = "[npmAuth.\"IP\"]";
    private const string TOML_TOKEN = "token = \"VALUE\"";
    private const string TOML_BOOL = "alwaysAuth = true";

    private const string MANIFEST_SCOPEDREGISTRIES = "{\"name\":\"Immersive Interactive\",\"url\":\"" + Utils.IP_ADDRESS + "\",\"scopes\":[\"" + Utils.PACKAGE_SCOPE + "\"]}";

    /// <summary>
    /// C# Equivalent to the JavaScript btoa() function.
    /// </summary>
    /// <param name="input">String you want to encode</param>
    /// <returns>Encoded string</returns>
    public static string BTOA(string input)
        => Convert.ToBase64String(Encoding.GetEncoding(28591).GetBytes(input));

    [Serializable]
    private class LoginRequest
    {
        public string _id;
        public string name;
        public string password;
        public string type;
        public string[] roles;
        public string date;
    }

    /// <summary>
    /// Oh god help me
    /// </summary>
    /// <param name="username">The user's username</param>
    /// <param name="password">The user's password</param>
    /// <returns></returns>
    private static async Task<string> AttemptLogin(string username, string password)
    {
        // Create a new JSON object with the user's credentials

        LoginRequest request = new()
        {
            _id = "org.couchdb.user:" + username,
            name = username,
            password = password,
            type = "user",
            roles = new string[0],
            date = DateTime.UtcNow.ToString("o")
        };

        using StringContent jsonContent = new(
               JsonUtility.ToJson(request),
               Encoding.UTF8,
               "application/json");

        Utils.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", BTOA(username + ":" + password));

        using HttpResponseMessage response = await Utils.httpClient.PutAsync("-/user/org.couchdb.user:" + username + "/-rev/undefined", jsonContent);
        Utils.HttpRequestCallback callback = JsonUtility.FromJson<Utils.HttpRequestCallback>(await response.Content.ReadAsStringAsync());

        if (response.StatusCode == HttpStatusCode.Created)
            return callback.token;
        else if (response.StatusCode == HttpStatusCode.Conflict)
            throw new Exception("Username or password is incorrect.");
        else
            throw new Exception(callback.error);
    }

    /// <summary>
    /// Sets up the file that Unity uses to authenticate with the private package manager
    /// </summary>
    /// <param name="token"></param>
    private static void UpdateUPMConfigToml(string token)
    {
        string authLine = TOML_AUTH.Replace("IP", Utils.IP_ADDRESS);
        string tokenLine = TOML_TOKEN.Replace("VALUE", token);

        File.WriteAllText(Utils.FindUpmConfigPath(), authLine + "\n" + tokenLine + "\n" + TOML_BOOL);
    }

    /// <summary>
    /// Updates the manifest.json inside the Unity project's Packages folder to add the private package manager to scopedRegistries
    /// </summary>
    public static void UpdateManifestJson()
    {
        // Don't add scoped registry to the manifest if it's already in there.
        if (Utils.ManifestHasImmersiveScopedRegistry()) return;

        string manifestFilePath = Utils.FindPackageManifestPath();

        List<Utils.ScopedRegistry> registries = Utils.GetScopedRegistries().ToList();
        registries.Add(new()
        {
            name = "Immersive Interactive",
            url = Utils.IP_ADDRESS,
            scopes = new string[] { Utils.PACKAGE_SCOPE }
        });

        Utils.PackageManifest manifestObject = new()
        {
            scopedRegistries = registries.ToArray()
        };

        //Debug.Log(manifestObject.scopedRegistries.Length);

        string scopedRegistryJson = JsonUtility.ToJson(manifestObject, true);
        scopedRegistryJson = Utils.CompletePrettifyJson(scopedRegistryJson);

        string manifest = File.ReadAllText(manifestFilePath);
        string manifestNew = "";
        if(Utils.ManifestHasScopedRegistries())
            manifestNew = Regex.Replace(manifest, Utils.SCOPED_REGISTRIES_REGEX, scopedRegistryJson);
        else
        {
            manifestNew = manifest.Remove(manifest.Length - 2);
            manifestNew += ",\n  " + scopedRegistryJson + "\n}";
        }
        File.WriteAllText(manifestFilePath, manifestNew);
    }

    /// <summary>
    /// Tries to authenticate with the package manager using a username and password
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">User's password</param>
    /// <returns>Whether or not the login attempt was successful</returns>
    public static async Task<bool> Authenticate(string username, string password)
    {
        string token = await AttemptLogin(username, password);
        if (string.IsNullOrEmpty(token)) throw new Exception("Invalid username and password");

        UpdateUPMConfigToml(token);
        UpdateManifestJson();
        return true;
    }
}
