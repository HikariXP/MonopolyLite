using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AcrylicNetworkManager : NetworkManager
{
    public Dictionary<NetworkConnection, string> PlayerDictionary = new Dictionary<NetworkConnection, string>();

    public override void Start()
    {
        base.Start();
       
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        CustomCreatePlayerMessage message = new CustomCreatePlayerMessage()
        {
            NewPlayerName = GameManager.Instance.currentAccount.AccountName,
            NewPlayerColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f))
        };

        NetworkClient.Send(message);

        Debug.Log("ClientConnectToServer,Message has sent.");
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        PlayManager.Instance.RemovePlayer(conn);

        PlayerDictionary.Remove(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        RegisterCustomMessage();

    }




    /// <summary>
    /// 注册相关信息
    /// </summary>
    private void RegisterCustomMessage()
    {
        NetworkServer.RegisterHandler<CustomCreatePlayerMessage>(OnCreatePlayer);
    }
    
    /// <summary>
    /// 当服务器接受到CustomCreatePlayerMessage时会创建角色
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="message"></param>
    public void OnCreatePlayer(NetworkConnection connection,CustomCreatePlayerMessage message)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Can't Create PlayerPerfab, because it is null.");
            return;
        }
        GameObject tempPlayer = Instantiate(playerPrefab);

        tempPlayer.GetComponent<Player>().PlayerName = message.NewPlayerName;
        tempPlayer.GetComponent<Player>().playerColor = message.NewPlayerColor;
        NetworkServer.AddPlayerForConnection(connection, tempPlayer);

        PlayerDictionary.Add(connection, tempPlayer.GetComponent<Player>().PlayerName);

        PlayManager.Instance.AddPlayer(connection);
        PlayManager.Instance.SetPlayerColor(tempPlayer.GetComponent<Player>().netId, tempPlayer.GetComponent<Player>().playerColor);

        Debug.Log("CreatePlayer:"+tempPlayer.GetComponent<Player>().PlayerName);
    }
}

public struct CustomCreatePlayerMessage : NetworkMessage
{
    public string NewPlayerName;
    public Color NewPlayerColor;
}

