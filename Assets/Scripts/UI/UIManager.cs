using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private PlayerHealth _health;


    [Header("Game Start")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_InputField _userNameInput;
    [SerializeField] private GameObject _userNameWarning;

    [Header("Pause")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _resumeButton, _restartButton, _quitButton;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _scoreAmount;
    [SerializeField] private Button _tryAgainButton, _returnMenuButton, _leaderboardButton, _backButtonLB;
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private TextMeshProUGUI _highscoreList;



    [Header("Game Overlay")]
    [SerializeField] private GameObject _gameOverlayPanel;
    [SerializeField] private TextMeshProUGUI _scoreTMP;
    [SerializeField] private Transform _heartsContainer;
    [SerializeField] private GameObject _heartPrefab;
    private Image[] _heartImages;
    [SerializeField] Toggle _muteToggle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (_health != null)
        {
            _health.UpdateLives += OnUpdateLives;
        }

        SetButtonClicks();
    }

    #region Game Start
    private void SetButtonClicks()
    {
        _startButton.onClick.AddListener(RunGame);
        _resumeButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(RestartLevel);
        _quitButton.onClick.AddListener(ReturnMenu);

        _muteToggle.onValueChanged.AddListener(ToggleMute);

        _tryAgainButton.onClick.AddListener(RestartLevel);
        _returnMenuButton.onClick.AddListener(ReturnMenu);

        _leaderboardButton.onClick.AddListener(ToggleLeaderboard);
        _backButtonLB.onClick.AddListener(ToggleLeaderboard);

        //If player already has a username, no need to ask for one again.
        if (PlayerPrefs.HasKey("Username"))
        {
            _userNameInput.gameObject.SetActive(false);
        }
        //Else player needs to write something for a username.
        else
        {
            _startButton.interactable = false;
            _userNameInput.onSubmit.AddListener(OnUserNameChanged);
            _userNameInput.onEndEdit.AddListener(OnUserNameChanged);
        }

        InitializeLives();
            
    }
    private void OnUserNameChanged(string input)
    {
        PlayFabManager.Instance.SendUserName(input);
    }
    public void OnUserNameChangeSuccess()
    {
        Debug.Log("Change success");
        _startButton.interactable = true;
    }
    public void OnUserNameChangeFailed()
    {
        Debug.Log("Change failed");
        _userNameWarning.SetActive(true);
        StartCoroutine(HideWarning());

    }

    private IEnumerator HideWarning()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        _userNameWarning.SetActive(false);
    }

    private void RunGame()
    {
        _startPanel.SetActive(false);
        _gameOverlayPanel.SetActive(true);

        //Save username from start panel if player doesn't already have one.
        if (!PlayerPrefs.HasKey("Username"))
        {
            string userName = _userNameInput.text;
            PlayFabManager.Instance.SendUserName(userName);
            PlayerPrefs.SetString("Username", userName);
        }
      
        GameManager.Instance.UpdateGameState(GameManager.GameState.Running);
    }
    #endregion

    #region Pause
    public void TogglePause()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        GameManager.Instance.TogglePause(_pausePanel.activeSelf);
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    private void ReturnMenu()
    {
        GameManager.Instance.TogglePause(false);
        SceneManager.LoadScene("MenuScene");
    }

    #endregion


    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
        _gameOverlayPanel.SetActive(false);
        _scoreAmount.text = "You destroyed \n<u>"+ GameManager.Instance.Score.ToString()+"</u> ghosts!";
    }
    #region Leaderboard
    private void ToggleLeaderboard()
    {
        _leaderboardPanel.SetActive(!_leaderboardPanel.activeSelf);

        if (_leaderboardPanel.activeSelf)
        {
            PlayFabManager.Instance.GetLeaderboard();
            AudioManager.Instance.PlaySound("CrowdCheer", true);
        }
      
    }
    public void UpdateLeaderboard(string leaderboard)
    {
        //Split the string into lines.
        string[] lines = leaderboard.Split('\n');

        //Change the color of first 3 lines. For safety check, if there are less than 3 lines, color them all.
        for (int i = 0; i < Mathf.Min(3, lines.Length); i++)
        {
            lines[i] = "<color=#FF3E7F>" + lines[i] + "</color>";
        }

        //Combine the lines back together.
        string newLeaderboard = string.Join("\n", lines);
        _highscoreList.text = newLeaderboard;
    }
    #endregion
    #region Game Overlay
    public void InitializeLives()
    {
        int lives = _health.Health;
        _heartImages = new Image[lives];

        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(_heartPrefab, _heartsContainer);
            _heartImages[i] = heart.transform.GetChild(0).GetComponent<Image>();
        }

        _gameOverlayPanel.SetActive(false);
    }
    private void OnUpdateLives()
    {
        //Loop through hearts.
        for(int i = _heartsContainer.childCount -1; i >= 0; i--)
        {
            //Disable the heart if its index is greater than the remaining lives.
            _heartImages[i].enabled = (i < _health.Health);
        }
    }

    public void UpdateScore(int amount)
    {
        _scoreTMP.text = amount.ToString() + "x";
    }

    private void ToggleMute(bool isOn)
    {
        AudioManager.Instance.ToggleMute();

        //Toggle gets triggered by pressing space if player hasn't clicked anything else,
        //to prevent this happening, interactable needs to be disabled for a moment.
        _muteToggle.interactable = false;
        _muteToggle.interactable = true;
    }

    #endregion
}
