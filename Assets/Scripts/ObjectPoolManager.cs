using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Header("Cake Pool")]
    [SerializeField] private GameObject _cakePrefab;

    private IObjectPool<GameObject> _cakePool;
    public IObjectPool<GameObject> CakePool { get => _cakePool; }

    //Throw an exception if trying to return an existing item, already in the pool.
    [SerializeField] private bool _cakeCollectionCheck = true;
    //Extra options to control the pool capacity and maximum size.
    [SerializeField] private int _cakeDefaultCapacity = 20;
    [SerializeField] private int _cakePoolMaxSize = 100;



    [Header("Obstacle Pool")]
    [SerializeField] private GameObject [] _obstaclePrefabs;

    // Dictionary to store pools with ID as keys.
    private Dictionary<int, IObjectPool<GameObject>> _obstaclePoolsDictionary = new Dictionary<int, IObjectPool<GameObject>>();
    public Dictionary<int, IObjectPool<GameObject>> ObstaclePoolsDictionary { get { return _obstaclePoolsDictionary; } }
    
    [SerializeField] private bool _obstacleCollectionCheck = true;
    [SerializeField] private int _obstacleDefaultCapacity = 20;
    [SerializeField] private int _obstaclePoolMaxSize = 100;
    
    private int _ID = 0;

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

        InitializePools();
    }

    private void InitializePools()
    {
        CreatePool(_cakePrefab, _cakeDefaultCapacity, _cakePoolMaxSize, _cakeCollectionCheck);
        _cakePool = _obstaclePoolsDictionary[0];

        //Reset pools dictionary, don't want any cakes in obstacle pools.
        _ID = 0;
        _obstaclePoolsDictionary = new();

        for (int i = 0; i < _obstaclePrefabs.Length; i++)
        {
            CreatePool(_obstaclePrefabs[i], _obstacleDefaultCapacity, _obstaclePoolMaxSize, _obstacleCollectionCheck);
        }
    }

    private void CreatePool(GameObject prefab, int defaultCapacity, int maxSize, bool collectionCheck)
    {
        int ID = _ID;
        _ID++;
        
        if (!_obstaclePoolsDictionary.ContainsKey(ID))
        {
            //Create a new pool for the prefab.
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => {

                    GameObject obj = CreateObject(prefab);
                    Obstacle obstacle = obj.GetComponent<Obstacle>();

                    if (obstacle != null)
                    {
                        //Set obstacles ID for releasing to pool.
                        obstacle.id = ID;
                    }
                    else
                    {
                        Debug.LogWarning("Obstacle component not found on prefab: " + prefab.name);
                    }
                        
                    return obj;
                },
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                collectionCheck,
                defaultCapacity,
                maxSize);

            _obstaclePoolsDictionary[ID] = pool;

        }
        else 
        {
            Debug.LogWarning("Pool already exists for prefab: " + prefab.name);
        }
           
        
    }

    //Release an object back to its pool.
    public void ReleaseObject(GameObject obj, int id)
    {
        if (_obstaclePoolsDictionary.ContainsKey(id))
        {
            _obstaclePoolsDictionary[id].Release(obj);
        }
        else
        {
            Debug.LogWarning("No pool found for the released object: " + obj.name);
        }
           
    }

    //Create new object for the pool.
    private GameObject CreateObject(GameObject prefab)
    {
        return Instantiate(prefab);
    }

    #region Callback functions

    private void OnGetFromPool(GameObject obj) 
    {
        obj.SetActive(true);
    }
    private void OnReleaseToPool(GameObject obj) 
    {
        obj.SetActive(false);
    }
    private void OnDestroyPooledObject(GameObject obj) 
    {
        Destroy(obj);
    }
    #endregion
}
