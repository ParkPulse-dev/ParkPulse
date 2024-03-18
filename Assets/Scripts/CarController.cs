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
    public Quaternion rot;
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
    [SerializeField] int numFrame = 12000;
    public float Acceleration = 0.0f;
    float Steer = 0.0f;
    bool AccelFwd, AccelBwd;
    bool not_written_yet;
    bool canMove = false;
    int forward = 1;
    int backwards = -1;
    List<CommandLog> CommandsLog;
    private static CarController instance;
    PhotonView view;
    public Sprite DefaultSprite;
    public Sprite SnowSprite;
    public Sprite ChangeDirectionSprite;
    private SpriteRenderer SpriteRenderer;
    public bool isFrozen = false;
    public bool isChangeDire = false;
    private bool isChangedDire = false;
    public SpriteRenderer MiniMapSpriteRenderer;

    public int parkQ = 0;


    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            view = GetComponent<PhotonView>();
        }
        if (instance == null)
            instance = this;
    }

    public static CarController GetInstance()
    {
        return instance;
    }

    void Start()
    {
        CommandsLog = new List<CommandLog>();
        not_written_yet = false; // turn on to record
        SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AllowMovement());
    }
    public IEnumerator AllowMovement()
    {
        yield return new WaitForSeconds(3f);
        canMove = true; // Allow movement after 3 seconds
    }

    public void StartMovement()
    {
        StartCoroutine(AllowMovement());
    }

    IEnumerator SelfAction(int action)
    {
        if (action == 0)
        {
            view.RPC("ChangeSprite", RpcTarget.All, 0); // Notify all clients to change sprite to SnowSprite
            canMove = false;
            yield return new WaitForSeconds(3f);
            view.RPC("ChangeSprite", RpcTarget.All, 2);
            isFrozen = false;
            canMove = true;
        }
        else if (action == 1)
        {
            view.RPC("ChangeSprite", RpcTarget.All, 1);
            isChangeDire = false;
            isChangedDire = true;
        }
    }

    [PunRPC]
    void ChangeSprite(int spriteIndex)
    {
        // Change sprite based on the index received
        switch (spriteIndex)
        {
            case 0:
                SpriteRenderer.sprite = SnowSprite;
                MiniMapSpriteRenderer.sprite = SnowSprite;
                break;
            case 1:
                SpriteRenderer.sprite = ChangeDirectionSprite;
                MiniMapSpriteRenderer.sprite = ChangeDirectionSprite;
                break;
            case 2:
                if (!isChangedDire)
                {
                    SpriteRenderer.sprite = DefaultSprite;
                    MiniMapSpriteRenderer.sprite = DefaultSprite;
                }
                else
                {
                    SpriteRenderer.sprite = ChangeDirectionSprite;
                    MiniMapSpriteRenderer.sprite = ChangeDirectionSprite;
                }
                break;
            default:
                // Handle other cases or defaults
                break;
        }
    }

    void FixedUpdate()
    {
        if (!canMove || (view != null && !view.IsMine))
        {
            return;
        }

        if (isFrozen)
        {
            StartCoroutine(SelfAction(0));
            return;
        }
        if (isChangeDire)
        {
            StartCoroutine(SelfAction(1));
            return;
        }


        CommandLog commandLog = new()
        {
            pos = transform.position,
            rot = transform.rotation,
            FrameExecuted = Time.frameCount
        };

        CommandsLog.Add(commandLog);

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
        // Only execute movement logic for the local player's car
        if ((view != null && view.IsMine) || SceneManager.GetActiveScene().buildIndex == 0)
        {
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
            DireCar();
        }
        else if (Direction == backwards)
        {
            AccelBwd = true;
            if ((backwards * MaxSpeed) <= Acceleration)
            {
                Acceleration -= speedChange;
            }
            DireCarRev();
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

                DireCar();
            }
            else
                AccelFwd = false;
        }
        else if (Direction == backwards)
        {
            if (Acceleration <= minSteer)
            {
                Acceleration += BreakingFactor;

                DireCarRev();
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

    public void DireCar()
    {
        if (!isChangedDire)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.back * Steer);
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.back * Steer);

        }

    }
    public void DireCarRev()
    {
        if (isChangedDire)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.back * Steer);
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward * Steer);
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.back * Steer);

        }

    }
}
