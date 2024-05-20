using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text textName;
    [SerializeField] Text textCountPlayers;
    public void SetInfo(RoomInfo info) // From LobbyManage (OnRoomListUpdate)
    {
        textName.text = info.Name;
        textCountPlayers.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinTolistRoom() // On buttom click
    {
        Debug.Log(textName.text);
        PhotonNetwork.JoinRoom(textName.text);
    }
    
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(textName.text);
    }
}
