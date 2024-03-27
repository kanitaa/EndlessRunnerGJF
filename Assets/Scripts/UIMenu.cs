using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _startButton, _optionsButton, _creditsButton, _quitButton;

    [Header("Options")]
    [SerializeField] private GameObject _options;
    [SerializeField] private Button _backButtonOptions;


    [Header("Credits")]
    [SerializeField] private GameObject _credits;
    [SerializeField] private Button _backButtonCredits;


    private void Start()
    {
        SetButtonClicks();
    }
    private void SetButtonClicks()
    {
        _startButton.onClick.AddListener(StartGame);
        _optionsButton.onClick.AddListener(ToggleOptions);
        _creditsButton.onClick.AddListener(ToggleCredits);
        _quitButton.onClick.AddListener(QuitGame);

        _backButtonOptions.onClick.AddListener(ToggleOptions);
        _backButtonCredits.onClick.AddListener(ToggleCredits);
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
    }
    private void QuitGame()
    {
        Application.Quit();
    }

}
