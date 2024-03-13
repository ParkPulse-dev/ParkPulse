
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
    }

}
