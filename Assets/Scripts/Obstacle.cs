using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Obstacle : MonoBehaviour
{
    private IObjectPool<Obstacle> objectPool;

    // public property to give the obstacle a reference to its ObjectPool
    public IObjectPool<Obstacle> ObjectPool { set => objectPool = value; }
}
