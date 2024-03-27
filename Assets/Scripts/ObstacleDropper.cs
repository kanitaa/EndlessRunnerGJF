using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleDropper : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField] private Obstacle _obstaclePrefab;
    private IObjectPool<Obstacle> _objectPool;
    [SerializeField] private bool collectionCheck = true;
    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;
    private void Awake()
    {
        _objectPool = new ObjectPool<Obstacle>(CreateProjectile,
        OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
        collectionCheck, defaultCapacity, maxSize);

    }
    private void Start()
    {
        StartCoroutine(DropObstacle());
    }
    #region ObjectPool
    // invoked when creating an item to populate the object pool
    private Obstacle CreateProjectile()
    {
        Obstacle projectileInstance = Instantiate(_obstaclePrefab);
        projectileInstance.ObjectPool = _objectPool;
        return projectileInstance;
    }
    // invoked when returning an item to the object pool
    private void OnReleaseToPool(Obstacle pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Obstacle pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }
    // invoked when we exceed the maximum number of pooled items (i.e. 
    // destroy the pooled object)
    private void OnDestroyPooledObject(Obstacle pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion

    private IEnumerator DropObstacle()
    {
        // Define the step
        int step = 2;

        // Calculate the number of steps needed to cover the range
        int numSteps = (GameManager.Instance.MaxPathValue - GameManager.Instance.MinPathValue) / step + 1;

        // Initialize the current position
        int currentPositionIndex = 0;

        while (true)
        {
            // Calculate the current x position
            int currentXPosition = GameManager.Instance.MinPathValue + currentPositionIndex * step;

            // Ensure the current position is within the range
            if (currentXPosition <= GameManager.Instance.MaxPathValue)
            {
                Obstacle obstacle = _objectPool.Get();
                Vector3 obstaclePosition = transform.position;
                obstaclePosition.x = currentXPosition;
                obstacle.transform.position = obstaclePosition;
            }

            // Move to the next position index
            currentPositionIndex = (currentPositionIndex + 1) % numSteps;

            yield return new WaitForSeconds(2);
        }
    }
}
