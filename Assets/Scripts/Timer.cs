using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI endText;

    public int Duration;
    public int remainingDuration;

    private Image sprite;
    private Color spriteOriginalColor;

    private float delayInSeconds = 4f;

    private float delayForEndInSeconds = 1f;

    private int drawFontSize = 4;

    private float startAlpha = 0f; // Start with zero alpha

    void Start()
    {
        sprite = GetComponent<Image>();
        if (sprite != null)
        {
            spriteOriginalColor = sprite.color;
            spriteOriginalColor.a = startAlpha;
            sprite.color = spriteOriginalColor;
        }

        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        // Hide the image and text
        uiFill.gameObject.SetActive(false);
        uiText.gameObject.SetActive(false);

        if (sprite != null)
        {
            // Hide the sprite
            sprite.enabled = false;
        }

        // Wait for 4 seconds
        yield return new WaitForSeconds(delayInSeconds);

        // Display the image and text with fade-in effect
        StartCoroutine(FadeInUI());

        // Start the timer
        Begin(Duration);
    }

    private void Begin(int second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(delayForEndInSeconds);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // Display 'Draw' on screnn
        endText.alignment = TextAlignmentOptions.Center;
        endText.fontSize = drawFontSize;
        endText.text = "DRAW";
        Time.timeScale = 0f; // Freeze the scene
    }

    private IEnumerator FadeInUI()
    {
        // Display the image and text
        uiFill.gameObject.SetActive(true);
        uiText.gameObject.SetActive(true);

        if (sprite != null)
        {
            // Display the sprite
            sprite.enabled = true;
        }

        // Fade in effect for UI elements
        float duration = 1f;
        float elapsedTime = 0f;
        float staticZero = 0f;
        float staticOne = 1f;
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(staticZero, staticOne, elapsedTime / duration);

            // Fade in the sprite
            if (sprite != null)
            {
                Color currentColor = sprite.color;
                currentColor.a = alpha;
                sprite.color = currentColor;
            }

            // Fade in the UI elements
            Color uiTextColor = uiText.color;
            uiTextColor.a = alpha;
            uiText.color = uiTextColor;

            Color uiFillColor = uiFill.color;
            uiFillColor.a = alpha;
            uiFill.color = uiFillColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure alpha is set to 1f at the end
        if (sprite != null)
        {
            Color currentColor = sprite.color;
            currentColor.a = staticOne;
            sprite.color = currentColor;
        }

        Color finalTextColor = uiText.color;
        finalTextColor.a = staticOne;
        uiText.color = finalTextColor;

        Color finalFillColor = uiFill.color;
        finalFillColor.a = staticOne;
        uiFill.color = finalFillColor;
    }
}
