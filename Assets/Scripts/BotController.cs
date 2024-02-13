using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class BotController : MonoBehaviour
{
    public List<CommandLog> Loaded_CommandsLog;
    int commandNumber = 0;
    Quaternion rotation2;
    bool canMove = false;

    [SerializeField] float secondBeforeMove = 3f;

    void Start()
    {
        string _jsonPath = "Assets/Replay_System/computer_car_path.json";
        var file = File.ReadAllText(_jsonPath);
        Loaded_CommandsLog = JsonUtility.FromJson<GameInputData>(file).CommandLog;
        Debug.Log(Loaded_CommandsLog.Count);
        StartCoroutine(AllowMovement());
    }
    IEnumerator AllowMovement()
    {
        yield return new WaitForSeconds(secondBeforeMove);
        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove) // If movement not allowed yet, return
            return;
        if (commandNumber < Loaded_CommandsLog.Count)
        {
            CommandLog log = Loaded_CommandsLog[commandNumber];
            float radToDeg = ((log.rot_z * 180) / 3.14f) % 360;
            rotation2 = Quaternion.Euler(0f, 0f, radToDeg);
            transform.rotation = rotation2; 
            transform.position = log.pos;
            commandNumber++;

        }


    }

}
