using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// 常规建筑区块
/// </summary>
public class Area_Building :NetworkBehaviour , I_AreaBase
{
    public string AreaName;

    public TextMesh textMesh;


    /// <summary> 建筑区域基础价格。(默认100),也是初次买入的价格。 </summary> 
    [Tooltip("此建筑区块的最基础低价")]
    public float BasicPrice = 100;

    [SyncVar]
    public int CurrentLevel = 0;

    [SyncVar(hook = nameof(OnChangeOwner))]
    public uint Owner_Netid = 0;


    //当前过路费
    [SyncVar]
    public float passPrice = 0;
    //当前升级花费
    [SyncVar]
    public float upgradePrice = 0;

    public float Lv0CostRatio = 0.5f;
    public float Lv1CostRatio = 0.75f;
    public float Lv2CostRatio = 0.8f;
    public float Lv3CostRatio = 0.9f;

    public float Lv0UpGradeRatio = 2f;
    public float Lv1UpGradeRatio = 4f;
    public float Lv2UpGradeRatio = 8f;



    public GameObject Level1BuildingGO;
    public GameObject Level2BuildingGO;
    public GameObject Level3BuildingGO;

    /// <summary>
    /// 获取此组件用于换色，索引0号是玩家材质
    /// </summary>
    public MeshRenderer meshRenderer;

    public void Start()
    {
        //修改地方名字
        textMesh.text = AreaName;
        //获取自己的渲染器
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 根据现况刷新价格
    /// </summary>
    [Server]
    public void ResetPrice()
    {
        switch (CurrentLevel)
        {
            case 0:
                upgradePrice = BasicPrice * Lv0CostRatio;
                passPrice = BasicPrice * Lv0CostRatio;
                break;
            case 1:
                upgradePrice = BasicPrice * Lv1CostRatio;
                passPrice = BasicPrice * Lv1CostRatio;
                break;
            case 2:
                upgradePrice = BasicPrice * Lv2CostRatio;
                passPrice = BasicPrice * Lv2CostRatio;
                break;
        }
    }

    
    public void OnChangeOwner(uint oldID,uint newID)
    {
        RefreshOutLook(newID);
    }


    /// <summary>
    /// 改变外观，即将加入小房子
    /// 当前问题：客户端获取Owner_Netid永远为0
    /// </summary>
    public void RefreshOutLook(uint newOwnerNetID)
    {
        if (newOwnerNetID == 0)
        {
            ResetOutLook();
            Debug.Log("ResetOutLook:CurrentNetID:"+ newOwnerNetID);
        }  
        else
        {
            meshRenderer.material.color = PlayManager.Instance.GetPlayerColor(newOwnerNetID);
            Debug.Log("RefreshOutLook:" + PlayManager.Instance.GetPlayerColor(newOwnerNetID));
        }

        //switch (CurrentLevel)
        //{
        //    case 0:
        //        Level1BuildingGO.SetActive(false);
        //        Level2BuildingGO.SetActive(false);
        //        Level3BuildingGO.SetActive(false);
        //        break;
        //    case 1:
        //        Level1BuildingGO.SetActive(true);
        //        Level2BuildingGO.SetActive(false);
        //        Level3BuildingGO.SetActive(false);
        //        break;
        //    case 2:
        //        Level1BuildingGO.SetActive(true);
        //        Level2BuildingGO.SetActive(true);
        //        Level3BuildingGO.SetActive(false);
        //        break;
        //    case 3:
        //        Level1BuildingGO.SetActive(true);
        //        Level2BuildingGO.SetActive(true);
        //        Level3BuildingGO.SetActive(true);
        //        break;
        //}
    }

    /// <summary>
    /// 特殊需求重置外观为默认
    /// </summary>
    public void ResetOutLook()
    {
        ////恢复空材质
        //meshRenderer.materials[0].color = PlayManager.Instance.NullOwnerColor;
        ////全部建筑隐藏
        //Level1BuildingGO.SetActive(false);
        //Level2BuildingGO.SetActive(false);
        //Level3BuildingGO.SetActive(false);
    }

    [ClientRpc]
    public void Upgrade()
    {
        CurrentLevel += 1;
        ResetPrice();
    }


    /// <summary>
    /// 当玩家路过本区块
    /// </summary>
    /// <param name="player">路过的玩家</param>
    [Server]
    public void OnPlayerPass(Player player)
    {
        
    }


    /// <summary>
    /// 当玩家到达本区块
    /// </summary>
    /// <param name="player">路过的玩家</param>
    [Server]
    public void OnPlayerStay(Player player)
    {
        player.SelectedBuilding = this;
        if (player == null)
        {
            Debug.Log("Player 没有传进来OnPlayerStay");
        }
        else
        //如果地块没人
        if (Owner_Netid == 0)      //如果没有拥有者
        {
            PlayManager.Instance.AreaPurchaseRequest(this, player);
        }
        else if (Owner_Netid != player.netId)    //如果是其他玩家的地块
        {
            Debug.Log("收取过路费");
            PlayManager.Instance.ChangePlayerMoney(player, -passPrice);
            PlayManager.Instance.ChangePlayerMoney(PlayManager.Instance.GetPlayer(Owner_Netid), passPrice);
        }
        else if (Owner_Netid == player.netId)    //如果是玩家拥有的地块
        {
            if (CurrentLevel < 3)
            {
                PlayManager.Instance.AreaUpgradeRequest(this, player);
            }
        }
    }
}
