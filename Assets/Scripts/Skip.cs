using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}