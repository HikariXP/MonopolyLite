using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
/// <summary>
/// 区域蓝图
/// </summary>
public interface I_AreaBase
{
    /// <summary>
    /// 玩家停留
    /// </summary>
    /// <param name="player"></param>
    void OnPlayerStay(Player player);

    /// <summary>
    /// 玩家路过
    /// </summary>
    /// <param name="player"></param>
    void OnPlayerPass(Player player);
}
