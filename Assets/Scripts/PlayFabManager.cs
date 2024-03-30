using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;

    private string _userID;

    private bool _loggedIn;
    public bool LoggedIn { get => _loggedIn; }
    private void Awake()
    {
        //Ensure there is only one AudioManager.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
      
        //Store random userID to Playerprefs and use it as Playfab userID

        if (!PlayerPrefs.HasKey("UserID"))
        {
            _userID = RandomString(15);
            PlayerPrefs.SetString("UserID", _userID);

        }
        else
        {
            _userID = PlayerPrefs.GetString("UserID");
           
        }


        Login();

       

    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Range(1, length).Select(_ => chars[Random.Range(0, chars.Length)]).ToArray());
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = _userID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Succesful login/account create");
        _loggedIn = true;
        GetLeaderboard();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in.");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendUserDisplayName(string displayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUserDisplayNameUpdate, OnError);
    }

    void OnUserDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Success");
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName ="GhostScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "GhostScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        UIMenu menu = GameObject.Find("MenuUI").GetComponent<UIMenu>();

        string leaderboard = "";

        foreach(var item in result.Leaderboard)
        {

            int position =  int.Parse(item.Position.ToString())+1;
            leaderboard += position + ". " + item.DisplayName + " " + item.StatValue +"\n";

        }

        menu.UpdateLeaderboard(leaderboard);
    }
}
