using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _startPanel;
    [SerializeField] private Button _startButton;


    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _resumeButton, _restartButton, _quitButton;


    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _returnMenuButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        SetButtonClicks();
    }

    private void SetButtonClicks()
    {
        _startButton.onClick.AddListener(RunGame);

        _resumeButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(RestartLevel);
        _quitButton.onClick.AddListener(ReturnMenu);

        _returnMenuButton.onClick.AddListener(ReturnMenu);
    }
   

    private void RunGame()
    {
        _startPanel.SetActive(false);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Running);
    }

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
        GameManager.Instance.TogglePause(true);
        SceneManager.LoadScene("MenuScene");
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }
}
