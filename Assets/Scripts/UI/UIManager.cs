using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Game Start")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_InputField _userNameInput;

    [Header("Pause")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _resumeButton, _restartButton, _quitButton;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _scoreAmount;
    [SerializeField] private Button _tryAgainButton, _returnMenuButton;

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
            
    }
    private void OnUserNameChanged(string input)
    {
        //Allow player to start the game after writing something.
        if (!_startButton.interactable)
        {
            _startButton.interactable = true;
        }
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


    #region Game Overlay
    public void InitializeLives(int lives)
    {
        _heartImages = new Image[lives];

        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(_heartPrefab, _heartsContainer);
            _heartImages[i] = heart.transform.GetChild(0).GetComponent<Image>();
        }

        _gameOverlayPanel.SetActive(false);
    }
    public void UpdateLives(int lives)
    {
        //Loop through hearts.
        for(int i = _heartsContainer.childCount -1; i >= 0; i--)
        {
            //Disable the heart if its index is greater than the remaining lives.
            _heartImages[i].enabled = (i < lives);
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
