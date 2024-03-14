using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rematch : MonoBehaviour
{
    public void OnClick()
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1.0f;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
