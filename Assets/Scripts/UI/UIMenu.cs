using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _startButton, _optionsButton, _leaderboardButton, _creditsButton, _quitButton;

    [Header("Options")]
    [SerializeField] private GameObject _options;
    [SerializeField] Slider _audioSlider;
    [SerializeField] private TextMeshProUGUI _volumeAmount;
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TextMeshProUGUI _usernamePlaceholder;
    [SerializeField] private Button _backButtonOptions;


    [Header("Credits")]
    [SerializeField] private GameObject _credits;
    [SerializeField] private Button _backButtonCredits;


    [Header("Leaderboard")]
    [SerializeField] private GameObject _leaderboard;
    [SerializeField] private TextMeshProUGUI _highscoreList;
    [SerializeField] private GameObject _fireworks;
    [SerializeField] private Button _backButtonLeaderboard;


    private void Start()
    {
        SetButtonClicks();
    }
    private void SetButtonClicks()
    {
        _startButton.onClick.AddListener(StartGame);
        _optionsButton.onClick.AddListener(ToggleOptions);
        _leaderboardButton.onClick.AddListener(ToggleLeaderboard);
        _creditsButton.onClick.AddListener(ToggleCredits);
        _quitButton.onClick.AddListener(QuitGame);

        _backButtonOptions.onClick.AddListener(ToggleOptions);
        _audioSlider.onValueChanged.AddListener(ChangeVolume);

        _audioSlider.value = AudioManager.Instance.Volume;
        ChangeVolume(AudioManager.Instance.Volume);

        _usernameInput.onSubmit.AddListener(OnUserNameChanged);
        _usernameInput.onEndEdit.AddListener(OnUserNameChanged);

        _usernamePlaceholder.text = PlayerPrefs.GetString("Username");

        _backButtonCredits.onClick.AddListener(ToggleCredits);

        _credits.SetActive(false);

        _backButtonLeaderboard.onClick.AddListener(ToggleLeaderboard);

        AudioManager.Instance.PlayMusic("LOOP_Welcome to Indie Game");

        if (PlayFabManager.Instance.LoggedIn)
        {
            PlayFabManager.Instance.GetLeaderboard();
        }
       
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ToggleOptions()
    {
        _options.SetActive(!_options.activeSelf);
    }

    public void ToggleCredits()
    {
        _credits.SetActive(!_credits.activeSelf);

        if (_credits.activeSelf)
        {
            Debug.Log("Play crdits");
            _credits.GetComponent<Credits>().PlayCredits();
        }
            
    }

    public void ToggleLeaderboard()
    {
        _leaderboard.SetActive(!_leaderboard.activeSelf);

        if (_leaderboard.activeSelf)
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
        // Split the leaderboard string into lines
        string[] lines = leaderboard.Split('\n');

        // Format the first three lines with a different color
        for (int i = 0; i < Mathf.Min(3, lines.Length); i++)
        {
            lines[i] = "<color=#FF3E7F>" + lines[i] + "</color>"; // Change color as needed
        }

        // Join the lines back together with newline characters as separator
        string formattedLeaderboard = string.Join("\n", lines);

        // Assign the formatted leaderboard text to the Text component
        _highscoreList.text = formattedLeaderboard;
    }
    void ChangeVolume(float value)
    {
        AudioManager.Instance.SetVolume(value);

        //Update mute toggle in game scene, if settings are at lowest audio.
        if (value == _audioSlider.minValue)
        {
            AudioManager.Instance.IsMuted = true;
        }

        value = Mathf.Clamp(value, _audioSlider.minValue, _audioSlider.maxValue);

        //Change sliders min/max values to range from 0 to 100 to make it more player friendly to read.
        float normalizedValue = ((value - _audioSlider.minValue) / (_audioSlider.maxValue - _audioSlider.minValue)) * (100 - 0) + 0;
        _volumeAmount.text = Mathf.RoundToInt(normalizedValue).ToString();

    }
    private void OnUserNameChanged(string input)
    {
        _startButton.interactable = true;

        PlayFabManager.Instance.SendUserName(input);
        PlayerPrefs.SetString("Username", input);

    }

    private void QuitGame()
    {
        Application.Quit();
    }

}
