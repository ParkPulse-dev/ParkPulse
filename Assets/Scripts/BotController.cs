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

    IEnumerator Start()
    {
        // Construct the path to the JSON file in the StreamingAssets folder
        string _jsonPath = Path.Combine(Application.streamingAssetsPath, "computer_car_path.json");

        // Check if the path is a URL (WebGL) or a local file path
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // For WebGL, use UnityWebRequest to download the JSON file
            using (var www = UnityEngine.Networking.UnityWebRequest.Get(_jsonPath))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    string json = www.downloadHandler.text;
                    ParseJson(json);
                }
                else
                {
                    Debug.LogError($"Failed to load JSON file: {_jsonPath}");
                }
            }
        }
        else
        {
            // For other platforms, read the JSON file directly from the file system
            string json = File.ReadAllText(_jsonPath);
            ParseJson(json);
        }

        // Start movement coroutine
        StartCoroutine(AllowMovement());
    }

    void ParseJson(string json)
    {
        // Parse JSON data into command log list
        GameInputData inputData = JsonUtility.FromJson<GameInputData>(json);
        Loaded_CommandsLog = inputData.CommandLog;
        Debug.Log($"Loaded {Loaded_CommandsLog.Count} commands from JSON.");
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
