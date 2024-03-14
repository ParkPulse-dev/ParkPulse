using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI endText;

    public int Duration;
    public int remainingDuration;

    private Image sprite;
    private Color spriteOriginalColor;

    [SerializeField] float delayInSeconds = 4f;

    [SerializeField] float delayForEndInSeconds = 1f;

    [SerializeField] int drawFontSize = 4;

    [SerializeField] float startAlpha = 0f; // Start with zero alpha
    public string text;
    private PhotonView photonView;

    private float lastNetworkTime = 0f;
    private float lastDuration = 0f;



    void Start()
    {
        photonView = GetComponent<PhotonView>();

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
        if (!photonView.IsMine)
            yield break;
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



        // Start the timer
        photonView.RPC("BeginTimer", RpcTarget.AllBuffered, Duration);
    }

    [PunRPC]
    private void BeginTimer(int second)
    {

        remainingDuration = second;
        // Display the image and text with fade-in effect
        StartCoroutine(FadeInUI());
        StartCoroutine(UpdateTimerNetwork());
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

    private IEnumerator UpdateTimerNetwork()
    {
        while (remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
            float fillValue = Mathf.InverseLerp(0, Duration, remainingDuration);
            uiFill.fillAmount = fillValue;
            remainingDuration--;

            if (PhotonNetwork.Time - lastNetworkTime >= 1f)
            {
                lastNetworkTime = (float)PhotonNetwork.Time;
                lastDuration = remainingDuration;
                photonView.RPC("SyncTimer", RpcTarget.OthersBuffered, remainingDuration, fillValue);
            }

            yield return new WaitForSeconds(delayForEndInSeconds);
        }
        OnEnd();
    }

    [PunRPC]
    private void SyncTimer(int syncDuration, float syncFill)
    {
        remainingDuration = syncDuration;
        uiFill.fillAmount = syncFill;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b);


    }

    private void OnEnd()
    {
        // Display 'Draw' on screnn
        endText.alignment = TextAlignmentOptions.Center;
        endText.fontSize = drawFontSize;
        endText.text = "DRAW";
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PopupSystem pop = gameObject.GetComponent<PopupSystem>();
            pop.PopUp(text);
        }
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
