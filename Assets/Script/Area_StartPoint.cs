using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单独的起点脚本：衍生于I_AreaBase
/// </summary>
public class Area_StartPoint : MonoBehaviour, I_AreaBase
{
    public float PassMoney = 1000;

    [Server]
    public void OnPlayerPass(Player player)
    {
        PlayManager.Instance.ChangePlayerMoney(player, PassMoney);
    }

    [Server]
    public void OnPlayerStay(Player player)
    {
        Debug.Log("玩家停留在了起点");
    }


}
