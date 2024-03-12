using System.Collections;
using UnityEngine;
using TMPro;

public class ThreeSecTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;

    [SerializeField] private int Duration = 3; // Set the duration to 3 seconds

    private static ThreeSecTimer instance;

    private int remainingDuration;

    void Start()
    {
        Begin(Duration);
        if (instance == null)
            instance = this;
    }

    public void Begin(int seconds)
    {
        remainingDuration = seconds;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration > 0)
        {
            uiText.text = $"{remainingDuration % 60}";
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }

        // Display "GO!" when the countdown reaches 0
        uiText.text = "GO!";

        // Wait for a moment before ending
        yield return new WaitForSeconds(1f);

        // Call the OnEnd method
        OnEnd();
    }

    private void OnEnd()
    {
        // Clear the text
        uiText.text = "";
    }

    public static ThreeSecTimer GetInstance()
    {
        return instance;
    }
}
