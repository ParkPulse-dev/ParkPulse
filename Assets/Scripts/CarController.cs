using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;

[Serializable]
public struct CommandLog
{
    public int FrameExecuted;
    public float rot_z;
    public Vector3 pos;
}

[Serializable]
public class GameInputData
{
    public List<CommandLog> CommandLog;
}

public class CarController : MonoBehaviour
{
    public float MaxSpeed = 5.0f;
    [SerializeField] float MaxSteer = 1.3f;
    [SerializeField] float Breaks = 0.15f;
    [SerializeField] float slowDown = 0.1f;
    public float speedChange = 0.05f;
    [SerializeField] float steerChange = 0.01f;
    [SerializeField] float minSteer = 0.4f;
    [SerializeField] int numFrame = 8000;
    float Acceleration = 0.0f;
    float Steer = 0.0f;
    bool AccelFwd, AccelBwd;
    bool not_written_yet;
    bool canMove = false;
    int forward = 1;
    int backwards = -1;
    List<CommandLog> CommandsLog;

    private static CarController instance;

    PhotonView view;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) // Checking if the build index of the active scene is 1
        {
            view = GetComponent<PhotonView>();
        }
        if (instance == null)
            instance = this;
        else
            Debug.LogWarning("Another instance of CarController already exists.");
    }

    public static CarController GetInstance()
    {
        return instance;
    }

    void Start()
    {
        CommandsLog = new List<CommandLog>();
        not_written_yet = false;
        StartCoroutine(AllowMovement());
    }
    IEnumerator AllowMovement()
    {
        yield return new WaitForSeconds(3f);
        canMove = true; // Allow movement after 3 seconds
    }

    void FixedUpdate()
    {
        if (view != null && !view.IsMine) return;
        if (!canMove) // If movement not allowed yet, return
            return;


        // CommandLog commandLog = new()
        // {
        // 	pos = transform.position,
        // 	rot_z = transform.rotation.z, // euleranglse
        // 	FrameExecuted = Time.frameCount
        // };

        // CommandsLog.Add(commandLog);

        if ((Time.frameCount > numFrame) && not_written_yet)
        {
            string _jsonPath = "Assets/Replay_System/computer_car_path.json";
            var gameInputData = new GameInputData
            {
                CommandLog = CommandsLog
            };

            Debug.Log("*****************************************");
            Debug.Log("Writing list len=" + CommandsLog.Count);
            Debug.Log("*****************************************");
            var actionInfo = JsonUtility.ToJson(gameInputData);
            File.WriteAllText(_jsonPath, actionInfo);

            not_written_yet = false;
        }
        //Accelerate in forward direction
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (AccelBwd) StopAccel(backwards, Breaks);
            else Accel(forward);
        }
        //Accelerate in backward direction
        else if (Input.GetKey(KeyCode.DownArrow))
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
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.back * Steer);
        }
        else if (Direction == backwards)
        {
            AccelBwd = true;
            if ((backwards * MaxSpeed) <= Acceleration)
            {
                Acceleration -= speedChange;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.back * Steer);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward * Steer);
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

                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Rotate(Vector3.forward * Steer);
                if (Input.GetKey(KeyCode.RightArrow))
                    transform.Rotate(Vector3.back * Steer);
            }
            else
                AccelFwd = false;
        }
        else if (Direction == backwards)
        {
            if (Acceleration <= minSteer)
            {
                Acceleration += BreakingFactor;

                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Rotate(Vector3.back * Steer);
                if (Input.GetKey(KeyCode.RightArrow))
                    transform.Rotate(Vector3.forward * Steer);
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
