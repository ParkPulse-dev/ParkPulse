using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{

    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int Duration;

    public int remainingDuration;

    // Start is called before the first frame update
    void Start()
    {
        Begin(Duration);

    }

    private void Begin(int Second){

        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer(){

        while(remainingDuration >= 0){

            uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd(){
        print("End Time");
    }



 
}
