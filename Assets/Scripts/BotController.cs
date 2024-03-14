using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class BotController : MonoBehaviour
{
    public List<CommandLog> Loaded_CommandsLog;
    int commandNumber = 0;
    bool canMove = false;
    public bool IsFrozen = false;

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
    IEnumerator AllowMovement2()
    {
        yield return new WaitForSeconds(3f);

        IsFrozen = false;
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
        if (commandNumber < Loaded_CommandsLog.Count)
        {
            CommandLog log = Loaded_CommandsLog[commandNumber];
            transform.SetPositionAndRotation(log.pos, log.rot);
            commandNumber++;

        }


    }

}
