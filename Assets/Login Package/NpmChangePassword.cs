using System.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Net;

public static class NpmChangePassword
{
    private const string TokenLine = "token = \"";
    private static string GetUserToken()
    {
        // Find toml file
        string tomlPath = Utils.FindUpmConfigPath();
        if (!File.Exists(tomlPath)) return string.Empty;

        // Read auth token on line 2
        // token = "TOKEN GOES HERE"
        string tokenLine = File.ReadAllLines(tomlPath)[1];
        tokenLine = tokenLine.Remove(0, TokenLine.Length);
        tokenLine = tokenLine.Remove(tokenLine.Length - 1, 1);
        return tokenLine;
    }


    private static async Task<bool> AttemptChangePassword(string authToken, string currentPassword, string newPassword)
    {
        string json = "{\"password\":{\"old\": \"" + currentPassword + "\", \"new\": \"" + newPassword + "\"}}";

        // Send POST request to /-/npm/v1/user
        using StringContent jsonContent = new(
           json,
           Encoding.UTF8,
           "application/json");

        Utils.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        using HttpResponseMessage response = await Utils.httpClient.PostAsync("-/npm/v1/user", jsonContent);
        Utils.HttpRequestCallback callback = JsonUtility.FromJson<Utils.HttpRequestCallback>(await response.Content.ReadAsStringAsync());

        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception(callback.error);
       
        return true;
    }

    public static async Task<bool> ChangePassword(string currentPassword, string newPassword)
    {
        string userToken = GetUserToken();
        if (string.IsNullOrEmpty(userToken)) throw new Exception("You are not logged in. If you have forgotten your password, contact Immersive Interactive.");
        bool successfulChange = await AttemptChangePassword(userToken, currentPassword, newPassword);
        return successfulChange;
    }
}
