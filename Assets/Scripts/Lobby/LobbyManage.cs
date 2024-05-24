using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManage : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] public TextMeshProUGUI nickName;
    [NonSerialized] public static string _roomName;
    [NonSerialized] public static string[] RoomNames = new[] {"Choose level", "GameHandler"};
    private List<RoomInfo> allRoomsInfo = new List<RoomInfo>();
    [SerializeField] ListItem itemPrefab;
    [SerializeField] Transform content;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public void FixedUpdate()
    {
        if (!(nickName is null))
            SetNickName();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to: " + PhotonNetwork.CloudRegion);

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public void SetNickName() => PhotonNetwork.NickName = nickName.text == null ? "user" : nickName.text;

    public void CreateRoomButton() // On buttom click
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (RoomNames.Contains(_roomName))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }
        else
            Debug.Log("no such room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomlist)
    {
        foreach (RoomInfo room in roomlist)
        {
            if (allRoomsInfo.Where(x => x.masterClientId == room.masterClientId).Count() > 0 ||
                room.PlayerCount == 0) // Если 0 человек то сервак идет нахер
                return;
            ListItem listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
            {
                listItem.SetInfo(room);
                allRoomsInfo.Add(room);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        if (!(nickName == null))
            SetNickName();
        if (RoomNames.Contains(_roomName))
        {
            PhotonManage.nickName = nickName.text;
            PhotonNetwork.LoadLevel(_roomName);
        }
    }

    public override void OnCreatedRoom() => Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("Error creating room");
}