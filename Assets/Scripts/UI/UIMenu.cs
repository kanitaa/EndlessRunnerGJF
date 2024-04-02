using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _startButton, _optionsButton, _leaderboardButton, _creditsButton, _quitButton;

    [Header("Options")]
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] Slider _audioSlider;
    [SerializeField] private TextMeshProUGUI _volumeAmount;
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TextMeshProUGUI _usernamePlaceholder;
    [SerializeField] private Button _backButtonOptions;


    [Header("Credits")]
    [SerializeField] private GameObject _creditsPanel;
    private Credits _credits;
    [SerializeField] private Button _backButtonCredits;


    [Header("Leaderboard")]
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private TextMeshProUGUI _highscoreList;
    [SerializeField] private GameObject _fireworks;
    [SerializeField] private Button _backButtonLeaderboard;


    private void Start()
    {
        InitializeMenuValues();
        AudioManager.Instance.PlayMusic("LOOP_Welcome to Indie Game");
    }
    private void InitializeMenuValues()
    {
        //Menu
        _startButton.onClick.AddListener(StartGame);
        _optionsButton.onClick.AddListener(ToggleOptions);
        _leaderboardButton.onClick.AddListener(ToggleLeaderboard);
        _creditsButton.onClick.AddListener(ToggleCredits);
        _quitButton.onClick.AddListener(QuitGame);

        //Options
        _audioSlider.onValueChanged.AddListener(ChangeVolume);
        _audioSlider.value = AudioManager.Instance.Volume;
        ChangeVolume(AudioManager.Instance.Volume);

        _usernameInput.onSubmit.AddListener(OnUserNameChanged);
        _usernameInput.onEndEdit.AddListener(OnUserNameChanged);

        if (PlayerPrefs.HasKey("Username"))
        {
            _usernamePlaceholder.text = PlayerPrefs.GetString("Username");
        }
   
        _backButtonOptions.onClick.AddListener(ToggleOptions);

        //Credits
        _creditsPanel.SetActive(false);
        _credits = _creditsPanel.GetComponent<Credits>();
        _backButtonCredits.onClick.AddListener(ToggleCredits);

        //Leaderboard
        //Get leaderboard at start if possible (because of small delay in server requests).
        if (PlayFabManager.Instance.LoggedIn)
        {
            PlayFabManager.Instance.GetLeaderboard();
        }
        _backButtonLeaderboard.onClick.AddListener(ToggleLeaderboard);

    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    #region Options
    public void ToggleOptions()
    {
        _optionsPanel.SetActive(!_optionsPanel.activeSelf);
    }
    private void ChangeVolume(float value)
    {
        AudioManager.Instance.SetVolume(value);

        //Mute audio if settings are at lowest value.
        if (value == _audioSlider.minValue)
        {
            AudioManager.Instance.IsMuted = true;
        }

        //Convert value to percentage-like value for readability.
        float normalizedValue = ((value - _audioSlider.minValue) / (_audioSlider.maxValue - _audioSlider.minValue)) * (100 - 0) + 0;
        _volumeAmount.text = Mathf.RoundToInt(normalizedValue).ToString();

    }
    private void OnUserNameChanged(string input)
    {
        _startButton.interactable = true;

        PlayFabManager.Instance.SendUserName(input);
        PlayerPrefs.SetString("Username", input);

    }
    #endregion


    public void ToggleCredits()
    {
        _creditsPanel.SetActive(!_creditsPanel.activeSelf);

        if (_creditsPanel.activeSelf)
        {
            _credits.PlayCredits();
        }    
    }

    #region Leaderboard
    public void ToggleLeaderboard()
    {
        _leaderboardPanel.SetActive(!_leaderboardPanel.activeSelf);

        if (_leaderboardPanel.activeSelf)
        {
            _fireworks.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySound("CrowdCheer", true);
        }
        else
        {
            _fireworks.SetActive(false);
            _fireworks.SetActive(true);
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

    private void QuitGame()
    {
        Application.Quit();
    }

}
