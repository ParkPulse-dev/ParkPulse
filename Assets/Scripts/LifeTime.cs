using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{

    public float lifeTime = 0.3f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
        
    }
}
