using System.Collections;

using UnityEngine;
using UnityEngine.Pool;

public class Shoot : MonoBehaviour
{
    private int _layerMask = 0;

    [SerializeField] private float _throwSpeed;


    [Header("Object Pool")]
    [SerializeField] private CakeProjectile _cakePrefab;
    private IObjectPool<CakeProjectile> objectPool;
    [SerializeField] private bool collectionCheck = true;
    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;
    private void Awake()
    {
        objectPool = new ObjectPool<CakeProjectile>(CreateProjectile,
        OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
        collectionCheck, defaultCapacity, maxSize);
    }

    #region ObjectPool
    // invoked when creating an item to populate the object pool
    private CakeProjectile CreateProjectile()
    {
        CakeProjectile projectileInstance = Instantiate(_cakePrefab);
        projectileInstance.ObjectPool = objectPool;
        return projectileInstance;
    }
    // invoked when returning an item to the object pool
    private void OnReleaseToPool(CakeProjectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(CakeProjectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }
    // invoked when we exceed the maximum number of pooled items (i.e. 
   // destroy the pooled object)
    private void OnDestroyPooledObject(CakeProjectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion
  
   
    public void CakeThrow(Vector3 target)
    {
        StartCoroutine(ThrowCake(target, _throwSpeed));
    }

    private IEnumerator ThrowCake(Vector3 target, float speed)
    {
        // Get a cake projectile from the object pool
        CakeProjectile cake = objectPool.Get();

        // Set the cake's position to the current position of the shooter
        cake.transform.position = transform.position;

        // Calculate the direction towards the target
        Vector3 direction = (target - transform.position).normalized;

        // Shoot the cake towards the target with the specified speed
        while (Vector3.Distance(cake.transform.position, target) > 0.1f)
        {
            // Move the cake towards the target
            cake.transform.position += direction * speed * Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Release the cake back to the object pool
        objectPool.Release(cake);
    }
}
