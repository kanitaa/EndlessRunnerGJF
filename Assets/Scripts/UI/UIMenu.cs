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
    [SerializeField] private Button _backButtonOptions;


    [Header("Credits")]
    [SerializeField] private GameObject _credits;
    [SerializeField] private Button _backButtonCredits;


    [Header("Leaderboard")]
    [SerializeField] private GameObject _leaderboard;
    [SerializeField] private TextMeshProUGUI _highscoreList;
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


        _backButtonCredits.onClick.AddListener(ToggleCredits);

        _credits.SetActive(false);

        _backButtonLeaderboard.onClick.AddListener(ToggleLeaderboard);

        AudioManager.Instance.PlayMusic("LOOP_Welcome to Indie Game");
        PlayFabManager.Instance.GetLeaderboard();
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

        if(_credits.activeSelf)
            _credits.GetComponent<Credits>().PlayCredits();
    }

    public void ToggleLeaderboard()
    {
        _leaderboard.SetActive(!_leaderboard.activeSelf);
    }

    public void UpdateLeaderboard(string leaderboard)
    {
        _highscoreList.text = leaderboard;
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

    private void QuitGame()
    {
        Application.Quit();
    }

}
