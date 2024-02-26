using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;


public class Car3controller : MonoBehaviour
{
    public float MaxSpeed = 5.0f;
    [SerializeField] float MaxSteer = 1.3f;
    [SerializeField] float Breaks = 0.15f;
    [SerializeField] float slowDown = 0.1f;
    public float speedChange = 0.05f;
    [SerializeField] float steerChange = 0.01f;
    [SerializeField] float minSteer = 0.4f;
    float Acceleration = 0.0f;
    float Steer = 0.0f;
    bool AccelFwd, AccelBwd;
    bool canMove = false;
    public bool IsFrozen = false;
    [SerializeField] public bool IsChangeDire = false;
    int forward = 1;
    int backwards = -1;

    private static Car3controller instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogWarning("Another instance of CarController already exists.");
    }

    public static Car3controller GetInstance()
    {
        return instance;
    }

    void Start()
    {

        StartCoroutine(AllowMovement());
    }
    IEnumerator AllowMovement()
    {
        yield return new WaitForSeconds(3f);
        canMove = true; // Allow movement after 3 seconds
        //IsFrozen = false;
    }

    IEnumerator AllowMovement2()
    {
        yield return new WaitForSeconds(3f);

        IsFrozen = false;
    }
    public void direCar()
    {
        if (!IsChangeDire)
        {
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.back * Steer);
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.back * Steer);

        }

    }
    public void direCarRev()
    {
        if (IsChangeDire)
        {
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.back * Steer);
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.back * Steer);

        }

    }



    void FixedUpdate()
    {
        if (!canMove) // If movement not allowed yet, return
            return;

        if (IsFrozen)
        {
            StartCoroutine(AllowMovement2());
            return;
        }




        if (Input.GetKey(KeyCode.W))
        {
            if (AccelBwd) StopAccel(backwards, Breaks);
            else Accel(forward);
        }
        //Accelerate in backward direction
        else if (Input.GetKey(KeyCode.S))
        {
            if (AccelFwd) StopAccel(forward, Breaks);
            else Accel(backwards);
        }
        else
        {
            if (AccelFwd)
                StopAccel(forward, slowDown); //Applies breaks slowly if no key is pressed while in forward direction
            else if (AccelBwd)
                StopAccel(backwards, slowDown); //Applies breaks slowly if no key is pressed while in backward direction
        }
    }

    public void Accel(int Direction)
    {
        if (Direction == forward)
        {
            AccelFwd = true;
            if (Acceleration <= MaxSpeed)
            {
                Acceleration += speedChange;
            }
            direCar();
            /*  if (Input.GetKey(KeyCode.A))
                  transform.Rotate(Vector3.forward * Steer);
              if (Input.GetKey(KeyCode.D))
                  transform.Rotate(Vector3.back * Steer);*/
        }
        else if (Direction == backwards)
        {
            AccelBwd = true;
            if ((backwards * MaxSpeed) <= Acceleration)
            {
                Acceleration -= speedChange;
            }
            /* if (Input.GetKey(KeyCode.A))
                 transform.Rotate(Vector3.back * Steer);
             if (Input.GetKey(KeyCode.D))
                 transform.Rotate(Vector3.forward * Steer);*/
            direCarRev();
        }

        if (Steer <= MaxSteer)
            Steer += steerChange;

        transform.Translate(Vector2.up * Acceleration * Time.deltaTime);
    }

    public void StopAccel(int Direction, float BreakingFactor)
    {
        if (Direction == forward)
        {
            if (Acceleration >= 0.0f)
            {
                Acceleration -= BreakingFactor;

                direCar();
            }
            else
                AccelFwd = false;
        }
        else if (Direction == backwards)
        {
            if (Acceleration <= minSteer)
            {
                Acceleration += BreakingFactor;

                direCarRev();
            }
            else
                AccelBwd = false;
        }

        if (Steer >= minSteer)
            Steer -= steerChange;

        transform.Translate(Vector2.up * Acceleration * Time.deltaTime);
    }
    public float CurrentAcceleration
    {
        get { return Acceleration; }
    }

}
