using UnityEngine;

public class startTutorial : MonoBehaviour
{
    public GameObject popupObject;

    void Start()
    {

        Time.timeScale = 0;
    }

    public void ClosePopup()
    {

        Time.timeScale = 1;


        popupObject.SetActive(false);
    }
}
