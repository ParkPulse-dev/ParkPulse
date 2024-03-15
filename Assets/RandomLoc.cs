using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RandomLoc : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject freeze;
    public GameObject ChangeDir;
    public GameObject Minimize;
    Vector3 posFreeze;
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int randomNumber = Random.Range(0, 2);

        if (currentSceneName == "Level1")
        {
            if (randomNumber == 0)
            {
                freeze.transform.position = new Vector3(8f, 11.7f, 0);
                ChangeDir.transform.position = new Vector3(-19.3f, 11f, 0);
                Minimize.transform.position = new Vector3(0f, -11f, 0);
            }
            else if (randomNumber == 1)
            {
                freeze.transform.position = new Vector3(23f, 0, 0);
                ChangeDir.transform.position = new Vector3(-25f, 0, 0);
                Minimize.transform.position = new Vector3(-1f, 12f, 0);
            }
        }
        if (currentSceneName == "Level2")
        {
            if (randomNumber == 0)
            {
                freeze.transform.position = new Vector3(8f, 11.7f, 0);
                ChangeDir.transform.position = new Vector3(-19.3f, 11f, 0);
                Minimize.transform.position = new Vector3(0f, -11f, 0);
            }
            else if (randomNumber == 1)
            {
                freeze.transform.position = new Vector3(20f, 1, 0);
                ChangeDir.transform.position = new Vector3(-22f, 1, 0);
                Minimize.transform.position = new Vector3(-1f, 12f, 0);
            }
        }
        if (currentSceneName == "Level3")
        {
            if (randomNumber == 0)
            {
                freeze.transform.position = new Vector3(20f, -5f, 0);
                ChangeDir.transform.position = new Vector3(-1f, 11f, 0);
                Minimize.transform.position = new Vector3(-1f, -11f, 0);
            }
            else if (randomNumber == 1)
            {
                freeze.transform.position = new Vector3(20f, 1, 0);
                ChangeDir.transform.position = new Vector3(-12f, 1, 0);
                Minimize.transform.position = new Vector3(-1f, 11f, 0);
            }
        }

    }
        
        
 }

    // Update is called once per frame
   