using System.Collections;
using UnityEngine;


public class Throw : MonoBehaviour
{
    [SerializeField] private float _throwSpeed;

    public void CakeThrow(Vector3 target)
    {
        StartCoroutine(ThrowCake(target, _throwSpeed));
    }

    private IEnumerator ThrowCake(Vector3 target, float speed)
    {
        // Get a cake from the object pool.
        GameObject cake = ObjectPoolManager.Instance.CakePool.Get();
        cake.transform.position = transform.position;

        Vector3 direction = (target - transform.position).normalized;
        AudioManager.Instance.PlaySound("JustWhoosh3_Swoosh_Rod_Pole_007", true);
        // Shoot the cake towards the target with the specified speed.
        while (Vector3.Distance(cake.transform.position, target) > 0.1f)
        {
            cake.transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }

        // Release the cake back to the object pool.
        ObjectPoolManager.Instance.CakePool.Release(cake);
    }
}
