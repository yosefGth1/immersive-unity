using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class NpmLogout : EditorWindow
{
    [MenuItem("Immersive Interactive/Package Authentication/Log out", true)]
    public static bool CanLogOut() => Utils.IsUserLoggedIn();

    [MenuItem("Immersive Interactive/Package Authentication/Log out")]
    public static void AttemptLogout()
    {
        // Logout from npm
        string filePath = Utils.FindUpmConfigPath();
        if(File.Exists(filePath))
            File.Delete(filePath);

        // Check if the scoped registry is in the manifest before trying to remove it
        if (!Utils.ManifestHasImmersiveScopedRegistry()) return;

        // Find immersive scoped registries and remove them from the manifest
        Utils.ScopedRegistry[] registries = Utils.GetScopedRegistries();
        List<Utils.ScopedRegistry> remaining = new();
        foreach (Utils.ScopedRegistry registry in registries)
        {
            if(registry == null) continue;
            bool keep = true;
            foreach(string scope in registry.scopes)
            {
                if (scope == Utils.PACKAGE_SCOPE)
                {
                    keep = false;
                    break;
                }
                    
            }
            if(keep) remaining.Add(registry);
        }

        // Create new scoped registries JSON
        string newScopedRegistries = JsonUtility.ToJson(new Utils.PackageManifest() {
            scopedRegistries = remaining.ToArray()
        }, true);

        // Formats it correctly
        newScopedRegistries = Utils.CompletePrettifyJson(newScopedRegistries);

        // Replace old scoped registries with the new scoped registries
        string manifestFilePath = Utils.FindPackageManifestPath();
        string manifest = File.ReadAllText(manifestFilePath);
        string manifestNew = Regex.Replace(manifest, Utils.SCOPED_REGISTRIES_REGEX, newScopedRegistries);

        File.WriteAllText(manifestFilePath, manifestNew.ToString());
    }
}
