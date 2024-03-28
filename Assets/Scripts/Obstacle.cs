using UnityEngine;
using UnityEngine.Pool;

public class Obstacle : MonoBehaviour
{
    private IObjectPool<Obstacle> _obstaclePool;
    public IObjectPool<Obstacle> ObstaclePool { set => _obstaclePool = value; }

    public int id;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground") || other.CompareTag("Player") || other.CompareTag("Grass"))
            ObjectPoolManager.Instance.ReleaseObject(gameObject, id);
    }
}
