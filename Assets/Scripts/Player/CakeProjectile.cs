using UnityEngine;
using UnityEngine.Pool;

public class CakeProjectile : MonoBehaviour
{
    private IObjectPool<CakeProjectile> _cakePool;

    //Give the projectile a reference to its ObjectPool.
    public IObjectPool<CakeProjectile> CakePool { set => _cakePool = value; }


}
