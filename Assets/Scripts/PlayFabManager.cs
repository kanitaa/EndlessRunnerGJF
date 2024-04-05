using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;

    private string _userID;

    private bool _loggedIn;
    public bool LoggedIn { get => _loggedIn; }

    private bool _menuScene = true;

    private UIMenu _menuUI = null;
    private UIManager _gameUI = null;

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

        SceneManager.activeSceneChanged += OnSceneChanged;
        _menuUI = GameObject.Find("MenuUI").GetComponent<UIMenu>();


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
        PlayerPrefs.SetString("Username", result.DisplayName);

        if (!_menuScene)
        {
            _gameUI.OnUserNameChangeSuccess();
        }
    }

    private void OnUserNameError(PlayFabError error)
    {
        if (_menuScene)
        {
            _menuUI.OnUserNameChangeFailed();
        }
        else
        {
           _gameUI.OnUserNameChangeFailed();
        }

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
        string leaderboard = "";

        foreach(var item in result.Leaderboard)
        {

            int position =  int.Parse(item.Position.ToString())+1;
            leaderboard += position + ". " + item.DisplayName + " " + item.StatValue +"\n";

        }

        if (_menuScene)
        {
            _menuUI.UpdateLeaderboard(leaderboard);
        }
        else
        {
            _gameUI.UpdateLeaderboard(leaderboard);
        }

    }
    #endregion


    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while requesting.");
        Debug.Log(error.GenerateErrorReport());
    }
    //Keep track of current scene to know which UI info to update.
    private void OnSceneChanged(Scene current, Scene next)
    {
        if(next.name == "MenuScene")
        {
            _menuUI = GameObject.Find("MenuUI").GetComponent<UIMenu>();
            _menuScene = true;
        }
        else if(next.name == "GameScene")
        {
            _gameUI = GameObject.Find("GameUI").GetComponent<UIManager>();
            _menuScene = false;
        }
    }

}
