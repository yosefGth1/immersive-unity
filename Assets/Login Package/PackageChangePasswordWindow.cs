using UnityEditor;
using UnityEngine;
using System;

public class PackageChangePasswordWindow : EditorWindow
{
    [MenuItem("Immersive Interactive/Package Authentication/Change password")]
    public static void ShowPasswordChanger()
    {
        (GetWindow(typeof(PackageChangePasswordWindow), false, "Change password") as PackageChangePasswordWindow).Setup();
    }

    [MenuItem("Immersive Interactive/Package Authentication/Change password", true)]
    public static bool IsUserLoggedIn() => Utils.IsUserLoggedIn();

    private string currentPassword = "";
    private string newPassword = "";
    private string confirmPassword = "";

    public void Setup()
    {
        currentPassword = "";
        newPassword = "";
        confirmPassword = "";
        minSize = new Vector2(300, 100);
        maxSize = new Vector2(600, 100);
        Show();
    }

    private async void AttemptChangePassword()
    {
        try
        {
            if (string.IsNullOrEmpty(currentPassword))
                throw new Exception("Current password is required.");

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                throw new Exception("New passwords are required.");

            if(newPassword != confirmPassword)
                throw new Exception("New passwords do not match.");

            bool success = await NpmChangePassword.ChangePassword(currentPassword, newPassword);
            if (success)
            {
                Debug.Log("Password changed successfully!");
                EditorUtility.DisplayDialog("Success", "Password changed successfully! Please login using your new password.", "Ok");
                Close();
                PackageLoginWindow.TrySetup();
            }
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Error", e.Message, "Ok");
            Debug.LogError(e.Message);
        }
    }

    private void OnGUI()
    {
        // Enter key
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            string guiName = GUI.GetNameOfFocusedControl();

            if (guiName == "NewPassField2")
                AttemptChangePassword();

            return;
        }

        // Current password
        GUI.SetNextControlName("CurrentPassField");
        currentPassword = EditorGUILayout.PasswordField("Current Password", currentPassword);

        // New password
        GUI.SetNextControlName("NewPassField1");
        newPassword = EditorGUILayout.PasswordField("New Password", newPassword);

        // Confirm password
        GUI.SetNextControlName("NewPassField2");
        confirmPassword = EditorGUILayout.PasswordField("Confirm Password", confirmPassword);

        // Buttons
        if (GUILayout.Button("Confirm"))
            AttemptChangePassword();
        if (GUILayout.Button("Cancel"))
            Close();
    }
}
