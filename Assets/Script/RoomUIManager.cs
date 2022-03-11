using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUIManager : MonoBehaviour
{
    public GameObject PlayerList;

    public GameObject Room_PlayerPerfab;

    public void Update()
    {
        RefreshPlayer();
    }

    public void RefreshPlayer()
    {
        //for (int i = 0; i < PlayerList.transform.childCount; i++)
        //{
        //    Destroy(PlayerList.transform.GetChild(i).gameObject);
        //}

        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        //{
        //    GameObject temp =  Instantiate(Room_PlayerPerfab, PlayerList.transform);
        //    temp.GetComponent<Room_PlayerPerfab>().Refresh(go.GetComponent<Player>().PlayerName);
        //}
    }
    
}
