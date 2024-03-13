
using System.Collections;
using TMPro;
using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;

    private string check = "Just like a real driving experience, accel is needed for a turn.";

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        if (text.Equals(check))
        {
            animator.SetTrigger("popAccel");
        }
        else
        {
            Debug.Log("POP");
            animator.SetTrigger("pop");
        }

        StartCoroutine(FreezeScene());
    }

    IEnumerator FreezeScene()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0.0f;
    }

    public void UnFreezeScene()
    {
        Time.timeScale = 1.0f;
    }

}
