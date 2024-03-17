using UnityEngine;

public class startTutorial : MonoBehaviour
{
    public GameObject popupObject; // קישור לאובייקט של ה-popup

    void Start()
    {
        // עצור את המשחק במצב pause
        

        // הפעל את ה-popup
      //  popupObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ClosePopup()
    {
        // הפעל את המשחק מחדש
        Time.timeScale = 1;

        // סגור את ה-popup
        popupObject.SetActive(false);
    }
}
