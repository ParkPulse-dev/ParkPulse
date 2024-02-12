using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{

    public Color startColor = Color.green;
    public Color endColor = Color.black;
    [Range(0,10)]
    public float speed = 1f;

    Renderer ren;

    void Awake()
    {
        ren = GetComponent<Renderer>();
    }

    void Update()
    {
       ren.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1)); 
    }
}
