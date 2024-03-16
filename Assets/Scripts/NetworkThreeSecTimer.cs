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

    private bool timerStarted;

    void Start()
    {
        StartCoroutine(StartMovementCoroutine());
    }
    IEnumerator StartMovementCoroutine()
    {
        while (!timerStarted)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                yield return null; // Wait until at least two players are in the room
                continue;
            }
            GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

            // Check if both players are owned by the local player
            if (player1 != null && player2 != null)
            {

                CarController carController1 = player1.GetComponent<CarController>();
                CarController carController2 = player2.GetComponent<CarController>();

                // Check if both car controllers are obtained successfully
                if (carController1 != null && carController2 != null)
                {
                    carController1.StartMovement();
                    carController2.StartMovement();
                    timerStarted = true;

                    Begin(Duration);
                }
            }

            yield return null; // Wait for the next frame
        }
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
