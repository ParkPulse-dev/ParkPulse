using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPopUpManager : MonoBehaviour
{

    public string popUpStartExplain;

    void Start()
    {
        PopupSystem pop = gameObject.GetComponent<PopupSystem>();
        pop.PopUp(popUpStartExplain);
    }

}
