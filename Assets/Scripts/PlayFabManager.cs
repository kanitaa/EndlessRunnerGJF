using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    private void Start()
    {
        Login();
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Succesful login/account create");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in.");
        Debug.Log(error.GenerateErrorReport());
    }
}
