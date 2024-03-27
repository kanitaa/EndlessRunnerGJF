using UnityEngine;
using UnityEngine.Pool;

public class CakeProjectile : MonoBehaviour
{
    private IObjectPool<CakeProjectile> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<CakeProjectile> ObjectPool { set => objectPool = value; }


}
