using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Start,
        Running,
        End
    }
    private GameState _myGameState;
    public GameState MyGameState { get => _myGameState; }

    private bool _isPaused = false;
    public bool IsPaused { get => _isPaused; }

    private int _score = 0;
    public int Score { get => _score; }


    [SerializeField] List<GameObject> _areas = new();

    private int [] _currentAreas = new int[2];
    private int _firstArea = 0;


    [SerializeField] private int _minPathValue;
    public int MinPathValue { get => _minPathValue; set => _minPathValue = value; }

    [SerializeField] private int _maxPathValue;
    public int MaxPathValue { get => _maxPathValue; set => _maxPathValue = value; }

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
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    #region Game States
    public void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                InitializeGame();
                break;
            case GameState.Running:
                RunGame();
                break;
            case GameState.End:
                EndGame();
                break;
            default:
                break;
       }
    }

    private void InitializeGame()
    {
        _currentAreas[0] = 0;
        _currentAreas[1] = -1;
        _firstArea = 0;
        TogglePause(true);
    }

    private void RunGame()
    {
        _myGameState = GameState.Running;
        AudioManager.Instance.PlayMusic("LOOP_Happy Quest");
        TogglePause(false);
    }

    private void EndGame()
    {
        UIManager.Instance.GameOver();
        PlayFabManager.Instance.SendLeaderboard(_score);
        TogglePause(true);
    }
    #endregion


    #region Pause, Restart, Score
    public void TogglePause(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        _isPaused = isPaused;
    }

    public void RestartLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
        UIManager.Instance.UpdateScore(_score);
    }
    #endregion


    public void AddArea(Transform currentAreaEndPoint)
    {
        int randomIndex = Random.Range(0, _areas.Count);

        //Make sure one of the current areas isnt the one being added.
        while (_currentAreas.Contains(randomIndex))
        {
            randomIndex = Random.Range(0, _areas.Count);

        }

        //Move randomized area at the end of current area.
        _areas[randomIndex].transform.position = currentAreaEndPoint.position;

        //Keep track of which areas are currently visible for player.
        if(_firstArea == _currentAreas[0])
        {
            _currentAreas[1] = randomIndex;
            _firstArea = _currentAreas[1];
        }
        else if(_firstArea == _currentAreas[1])
        {
            _currentAreas[0] = randomIndex;
            _firstArea = _currentAreas[0];
        }

        Debug.Log("New area added." + _areas[randomIndex].name);
    }

}
