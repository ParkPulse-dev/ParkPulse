using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ToTheLobby : MonoBehaviourPunCallbacks
{
    public void OnClick()
    {
        Time.timeScale = 1.0f;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Loading");
    }
}