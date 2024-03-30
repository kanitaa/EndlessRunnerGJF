using PlayFab;
using PlayFab.ClientModels;
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
        //Ensure there is only one PlayfabManager.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
           
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
      
        //Store random string UserID to PlayerPrefs, and use it as Playfab userID.
        if (!PlayerPrefs.HasKey("UserID"))
        {
            _userID = RandomID(15);
            PlayerPrefs.SetString("UserID", _userID);
        }
        else
        {
            _userID = PlayerPrefs.GetString("UserID");
        }

        Login();

    }

    #region User
    private string RandomID(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Range(1, length).Select(_ => chars[Random.Range(0, chars.Length)]).ToArray());
    }

    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = _userID,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnError);
    }

    private void OnLogin(LoginResult result)
    {
        Debug.Log("Successful login!");
        _loggedIn = true;
        GetLeaderboard();
    }

    public void SendUserName(string displayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUserNameUpdate, OnUserNameError);
    }

    private void OnUserNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Successful username change.");
    }

    private void OnUserNameError(PlayFabError error)
    {
        //TO-DO notify player when name change fails.
        Debug.Log("Error while updating username.");
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion

    #region Leaderboard
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

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard update.");
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

    private void OnLeaderboardGet(GetLeaderboardResult result)
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
    #endregion


    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while requesting.");
        Debug.Log(error.GenerateErrorReport());
    }
}
