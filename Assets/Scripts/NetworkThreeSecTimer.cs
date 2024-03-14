using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NetworkThreeSecTimer : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private int Duration = 3; // Set the duration to 3 seconds

    private int remainingDuration;

    private float lastNetworkTime = 0f;

    void Start()
    {
        Begin(Duration);
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

            if (PhotonNetwork.Time - lastNetworkTime >= 1f)
            {
                lastNetworkTime = (float)PhotonNetwork.Time;
                photonView.RPC("SyncTimer", RpcTarget.OthersBuffered, remainingDuration);
            }

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

    [PunRPC]
    private void SyncTimer(int syncDuration)
    {
        remainingDuration = syncDuration;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(remainingDuration);
        }
        else
        {
            remainingDuration = (int)stream.ReceiveNext();
        }
    }
    
}
