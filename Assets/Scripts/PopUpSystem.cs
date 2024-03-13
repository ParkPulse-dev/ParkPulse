
using System.Collections;
using TMPro;
using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
        StartCoroutine(FreezeScene());
    }

    IEnumerator FreezeScene()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.0f;
    }

    public void UnFreezeScene()
    {
        Time.timeScale = 1.0f;
    }

}
