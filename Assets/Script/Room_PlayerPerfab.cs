using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_PlayerPerfab : MonoBehaviour
{
    public Text PlayerNameText;

    public void Refresh(string playerName)
    {
        PlayerNameText.text = playerName;
    }
}
