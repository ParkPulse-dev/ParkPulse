using UnityEngine;

public class startTutorial : MonoBehaviour
{
    public GameObject popupObject; // ����� �������� �� �-popup

    void Start()
    {
        // ���� �� ����� ���� pause
        

        // ���� �� �-popup
      //  popupObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ClosePopup()
    {
        // ���� �� ����� ����
        Time.timeScale = 1;

        // ���� �� �-popup
        popupObject.SetActive(false);
    }
}
