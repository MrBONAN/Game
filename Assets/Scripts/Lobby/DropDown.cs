using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    [NonSerialized] public static string[] Levels;
    [NonSerialized] private static TMP_Dropdown dropDown;
    void Start()
    {
        dropDown = transform.GetComponent<TMP_Dropdown>();
        Levels = LobbyManage.RoomNames;

        foreach (var level in Levels)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(level));
        }

        dropDown.onValueChanged.AddListener(delegate { DropdownItemSelected(); });
    }

    private void DropdownItemSelected()
    {
        var res = dropDown.options[dropDown.value].text;
        SetRoomName(res);
    }

    private void SetRoomName(string res) => LobbyManage._roomName = res;
    
}
