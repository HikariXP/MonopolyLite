using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Area_Event : NetworkBehaviour, I_AreaBase
{
    public TextMesh textMesh;

    public void Start()
    {
        textMesh.text = "Event";
    }


    public void OnPlayerPass(Player player)
    {

    }

    public void OnPlayerStay(Player player)
    {
        //PlayManager.Instance.PlayerReceiveEvent(player);
        //Debug.Log(player.PlayerName+":GetEvent");
    }
}
