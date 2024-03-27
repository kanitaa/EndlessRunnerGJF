using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float _minPathValue;
    public float MinPathValue { get => _minPathValue; set => _minPathValue = value; }

    [SerializeField] private float _maxPathValue;
    public float MaxPathValue { get => _maxPathValue; set => _maxPathValue = value; }




    [SerializeField] List<GameObject> _areas = new();

    [SerializeField]
    private int [] _currentAreas = new int[2];
    [SerializeField] private int _firstArea = 0;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

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
        TogglePause(false);
    }

    private void EndGame()
    {
        UIManager.Instance.GameOver();
        TogglePause(true);
        Debug.Log("Game Over");
    }

    public void TogglePause(bool isPaused)
    {
        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void AddArea(Transform currentAreaEndPoint)
    {
        int randomIndex = Random.Range(0, _areas.Count);


        while (_currentAreas.Contains(randomIndex))
        {
            randomIndex = Random.Range(0, _areas.Count);

        }

        _areas[randomIndex].transform.position = currentAreaEndPoint.position;

        if(_firstArea == _currentAreas[0])
        {
            _currentAreas[1] = randomIndex;
            _firstArea = _currentAreas[1];
        }else if(_firstArea == _currentAreas[1])
        {
            _currentAreas[0] = randomIndex;
            _firstArea = _currentAreas[0];
        }

        Debug.Log("New area added." + _areas[randomIndex].name);
    }

    public enum GameState
    {
        Start,
        Running,
        End
    }

}
