using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

/// <summary>
/// 战局管理器
/// </summary>
public class PlayManager : NetworkBehaviour
{
    public static PlayManager Instance;



    
    public List<NetworkConnection> PlayerConnectionList = new List<NetworkConnection>();

    public GameObject MapStruct;
    public List<I_AreaBase> AreaBases = new List<I_AreaBase>();

    public List<Transform> AreaTrans = new List<Transform>();

    public readonly SyncDictionary<uint, Color> PlayerColors = new SyncDictionary<uint, Color>();

    public bool canStartGame = false;

    public int MinPlayerAmount = 1;

    public float InitMoney = 5000f;

    [SyncVar]
    public int ActivePlayerIndex = 0;


    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        LoadMapStruct();
    }

    [Server]
    public void ChangePlayerMoney(Player player, float changeAmount)
    {
        player.Money += changeAmount;
    }

    [Server]
    public void CheckPlayersMoney()
    { 
    
    }

    public void LoadMapStruct()
    {
        foreach (I_AreaBase i_Area in MapStruct.GetComponentsInChildren<I_AreaBase>())
        {
            AreaBases.Add(i_Area);
        }
        foreach (Transform transform in MapStruct.GetComponentsInChildren<Transform>())
        {
            if (transform.CompareTag("PathPoint"))
            {
                AreaTrans.Add(transform);
            }
        }
        Debug.Log("Load I_AreaBase:" + AreaBases.Count + " and AreaTrans:" + AreaTrans.Count);

    }


    /// <summary>
    /// 改变区域状态
    /// </summary>
    //[Server]
    //public void ChangeAreaStatus(Player)
    //{ 

    //}

    [Server]
    public void AddPlayer(NetworkConnection connection)
    {
        PlayerConnectionList.Add(connection);
        CheckPlayerAmount();
    }

    [Server]
    public void RemovePlayer(NetworkConnection connection)
    {
        PlayerConnectionList.Remove(connection);
        CheckPlayerAmount();
    }

    public void CheckPlayerAmount()
    {
        if (PlayerConnectionList.Count >= 1)
        {
            canStartGame = true;
        }
        else canStartGame = false;
    }

    public void SetPlayerColor(uint targetNetID,Color targetColor)
    {
        PlayerColors.Add(targetNetID, targetColor);
        Debug.Log("PlayerColors Add :" + targetColor);
    }

    public void RemovePlayerColor(uint targetNetID)
    {
        if (PlayerColors.ContainsKey(targetNetID))
        {
            PlayerColors.Remove(targetNetID);
        }
        else Debug.LogWarning("TargetNetID is Not in PlayerColors");
    }


    /// <summary>由Host房主发起开始游戏的请求，随后让每个玩家都激活开始游戏</summary>
    [Server]
    public void StartGame()
    {
        foreach(NetworkConnection playerConnection in PlayerConnectionList)
        {
            playerConnection.identity.GetComponent<Player>().OnGameStart();
            playerConnection.identity.GetComponent<Player>().Money = InitMoney;
        }
        PlayerConnectionList[0].identity.GetComponent<Player>().OnLocalPlayerTurn();
        LookAtActivePlayer(PlayerConnectionList[0].identity.GetComponent<Player>().transform);


    }


    [Server]
    public void LookAtActivePlayer(Transform target)
    {
        foreach (NetworkConnection playerConnection in PlayerConnectionList)
        {
            playerConnection.identity.GetComponent<Player>().LookAtPlayer(target);
        }
    }

    [Server]
    public void PlayerEndTurn(Player player)
    {
        player.OnPlayerEndTurn();
        StartCoroutine(E_PlayerEndTurn(player));
    }

    [Server]
    IEnumerator E_PlayerEndTurn(Player player)
    {
        player.SelectedBuilding = null;

        int dice = Random.Range(1,7);
        Debug.Log(dice);


        int startIndex = player.CurrentPosIndex;



        for (int i = 0; i < dice; i++)
        {
            startIndex += 1;
            if (startIndex >= AreaBases.Count)
            {
                startIndex -= AreaBases.Count;
            }
            AreaBases[startIndex].OnPlayerPass(player);
        }
        AreaBases[startIndex].OnPlayerStay(player);


        player.CurrentPosIndex += dice;

        if (player.CurrentPosIndex >= AreaBases.Count)
        {
            player.CurrentPosIndex -= AreaBases.Count;
        }

        player.Move(AreaTrans[player.CurrentPosIndex].position);

        yield return new WaitForSeconds(2f);
        ActivePlayerIndex += 1;
        ActivePlayerIndex = ActivePlayerIndex >= PlayerConnectionList.Count ? 0 : ActivePlayerIndex;

        PlayerConnectionList[ActivePlayerIndex].identity.GetComponent<Player>().OnLocalPlayerTurn();

        LookAtActivePlayer(PlayerConnectionList[ActivePlayerIndex].identity.GetComponent<Player>().transform);
    }


    public void TrunToNextPlayer()
    { 
        
    }

    public Player GetPlayer(uint netID)
    {
        foreach (NetworkConnection nc in PlayerConnectionList)
        {
            if (nc.identity.netId == netID)
            {
                return nc.identity.GetComponent<Player>();
            }
        }
        return null;
    }


    [Server]
    public void AreaPurchaseRequest(Area_Building area_Building, Player player)
    {
        player.ReceiveAreaPurchaseRequest(area_Building);
    }

   /// <summary>
   /// 不知道是不是因为使用了Command，所以这个方法在只在服务端上执行了，只要客户端上地块信息IsServer为否，就不会在客户端上执行
   /// 换而言之，这个方法依然只在服务端上执行了即使现在没有任何的特性标签
   /// </summary>
   /// <param name="area_Building"></param>
   /// <param name="player"></param>
    [Server]
    public void PlayerAgreePurchase(Area_Building area_Building,Player player)
    {
        ChangePlayerMoney(player, -area_Building.BasicPrice);

        area_Building.Owner_Netid = player.netId;

        area_Building.ResetPrice();

        Debug.Log("TargetNetID:"+area_Building.Owner_Netid);
        Debug.Log("Player Buy:"+area_Building.AreaName);
    }

    [Server]
    public void AreaUpgradeRequest(Area_Building area_Building, Player player)
    {
        player.ReceiveAreaUpgradeRequest(area_Building);
    }

    [Server]
    public void PlayerAgreeUpdate(Area_Building area_Building, Player player)
    {
        ChangePlayerMoney(player, area_Building.upgradePrice);
        area_Building.Upgrade();
    }

    [Client]
    public Color GetPlayerColor(uint PlayerNetID)
    {
        Debug.Log("Getting Color:" + PlayerNetID);
        foreach(KeyValuePair<uint,Color> kvp in PlayerColors)
        {
            Debug.Log("kvp.key:" + kvp.Key+"PlayerNetID:"+PlayerNetID+ kvp.Key.Equals(PlayerNetID));
            if (kvp.Key.Equals(PlayerNetID))
            {
                Debug.Log("GetColorReturn:" + kvp.Value);
                return kvp.Value;
            }
        }
        Debug.Log("GetColorReturn:" + Color.black);
        return Color.black;
    }
}


public static class EnumeratorList
{
    public static WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

    public static WaitForSeconds OneSecond = new WaitForSeconds(1f);

    public static WaitForSeconds HalfSecond = new WaitForSeconds(0.5f);

    public static WaitForSeconds DotOneSecond = new WaitForSeconds(0.1f);
}