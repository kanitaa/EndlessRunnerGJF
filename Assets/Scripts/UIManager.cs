using System.Collections;
using TMPro;
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


    [SerializeField] private GameObject _gameOverlayPanel;
    [SerializeField] private TextMeshProUGUI _scoreTMP;

    [SerializeField] private GameObject _heartsContainer;
    [SerializeField] private GameObject _heartPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeLives();
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
        _gameOverlayPanel.SetActive(true);
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
        _gameOverlayPanel.SetActive(false);
    }

    public void UpdateScore(int amount)
    {
        _scoreTMP.text = amount.ToString() +"x";
    }
    private void InitializeLives()
    {
        for (int i = 0; i < GameManager.Instance.PlayerHealth; i++)
        {
            Instantiate(_heartPrefab, _heartsContainer.transform);
        }

        _gameOverlayPanel.SetActive(false);
    }
    public void UpdateLives()
    {
        for(int i = _heartsContainer.transform.childCount; i>0; i--)
        {
            Debug.Log(i);
            if(i>GameManager.Instance.PlayerHealth)
                _heartsContainer.transform.GetChild(i-1).GetChild(0).GetComponent<Image>().enabled = false;
        }
    }
}
