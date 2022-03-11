using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string PlayerName;

    [SyncVar]
    public float Money;

    [SyncVar]
    public int CurrentPosIndex;

    public NavMeshAgent agent;

    public CinemachineVirtualCamera cvc;

    [SyncVar]
    public Color playerColor;

    [SyncVar]
    public Area_Building SelectedBuilding;

    public void Start()
    {
        if (isLocalPlayer)
        {
            PlayUIManager.Instance.LocalPlayer = this;
            cvc = GameObject.Find("VC_FollowActivedPlayer").GetComponent<CinemachineVirtualCamera>();
        }
        agent = gameObject.GetComponent<NavMeshAgent>();
        
        
    }

    [Command]
    public void OnCallStartGame()
    {
        PlayManager.Instance.StartGame();
    }

    [ClientRpc]
    public void OnGameStart()
    {
        PlayUIManager.Instance.OnGameStart();
    }

    [TargetRpc]
    public void OnLocalPlayerTurn()
    {
        PlayUIManager.Instance.OnStartTurn();
    }

    [Command]
    public void OnEndLocalPlayerTurn()
    {
        PlayManager.Instance.PlayerEndTurn(this);
    }

    [TargetRpc]
    public void OnPlayerEndTurn()
    {
        PlayUIManager.Instance.OnEndTurn();
    }

    [ClientRpc]
    public void Move(Vector3 target)
    {
        agent.destination = target;
    }

    [ClientRpc]
    public void LookAtPlayer(Transform targetPlayer)
    {
        if (isLocalPlayer)
        {
            cvc.Follow = targetPlayer;
        }
    }

    [TargetRpc]
    public void ReceiveAreaPurchaseRequest(Area_Building area_Building)
    {
        PlayUIManager.Instance.DisplayAreaPurchaseInfo(area_Building);
    }

    [Command]
    public void AgreeAreaPurchase()
    {
        PlayManager.Instance.PlayerAgreePurchase(SelectedBuilding,this);
    }

    [TargetRpc]
    public void ReceiveAreaUpgradeRequest(Area_Building area_Building)
    {
        PlayUIManager.Instance.DisplayAreaUpgradeInfo(area_Building);
    }

    [Command]
    public void AgreeAreaUpgrade()
    {
        PlayManager.Instance.PlayerAgreeUpdate(SelectedBuilding, this);
    }
}
