using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class PhotonManage : MonoBehaviourPunCallbacks
{
    [NonSerialized] public static string nickName;
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("Disconnected grom server");
    
    public void LeaveButton() => PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom() => PhotonNetwork.LoadLevel("main");
}
