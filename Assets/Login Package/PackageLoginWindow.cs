using System;
using UnityEditor;
using UnityEngine;

public class PackageLoginWindow : EditorWindow
{
    [MenuItem("Immersive Interactive/Package Authentication/Setup")]
    public static void TrySetup()
    {
        if (Utils.IsUserLoggedIn())
            NpmLogin.UpdateManifestJson();
        else
            (GetWindow(typeof(PackageLoginWindow), false, "Immersive Authentication") as PackageLoginWindow).Setup();
    }

    [MenuItem("Immersive Interactive/Package Authentication/Setup", true)]
    public static bool IsUserLoggedIn() => !Utils.IsUserLoggedIn() || !Utils.ManifestHasImmersiveScopedRegistry();

    private static string username = "";
    private static string password = "";

    public void Setup()
    {
        EditorGUI.FocusTextInControl("UserField");
        minSize = new Vector2(300, 85);
        maxSize = new Vector2(600, 85);
        password = "";
        Show();
    }

    private async void AttemptLogin()
    {
        try
        {
            bool success = await NpmLogin.Authenticate(username, password);
            if (success)
            {
                UnityEditor.PackageManager.Client.Resolve();
                Close();
                Debug.Log("Login successful.");
                EditorUtility.DisplayDialog("Success", "Login successful.", "Ok");
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            string guiName = GUI.GetNameOfFocusedControl();

            if (guiName == "PassField")
                AttemptLogin();

            return;
        }

        // Username
        GUI.SetNextControlName("UserField");
        username = EditorGUILayout.TextField("Username", username);

        // Password
        GUI.SetNextControlName("PassField");
        password = EditorGUILayout.PasswordField("Password", password);

        // Buttons
        if (GUILayout.Button("Confirm"))
            AttemptLogin();
        if (GUILayout.Button("Cancel"))
            Close();
    }
}
