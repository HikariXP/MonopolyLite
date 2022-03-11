using UnityEngine;

namespace Mirror.Examples.Chat
{
    [AddComponentMenu("")]
    public class ChatNetworkManager : NetworkManager
    {
        [Header("Chat GUI")]
        public ChatWindow chatWindow;

        // Set by UI element UsernameInput OnValueChanged
        public string PlayerName { get; set; }

        // Called by UI element NetworkAddressInput.OnValueChanged
        public void SetHostname(string hostname)
        {
            networkAddress = hostname;
        }

        public struct CreatePlayerMessage : NetworkMessage
        {
            public string name;
        }

        //启动服务器之后，基于其原本的代码(base.OnStartServer())之后，注册一个句柄关于创建自定义玩家
        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        //覆写源代码，基于源客户端链接提交的NetworkConnection传递信息创建玩家预设体
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            // tell the server to create a player with this name
            conn.Send(new CreatePlayerMessage { name = PlayerName });
        }

        //创建一个玩家预设体，并且基于信息修改名字，再为其赋予玩家的属性
        void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
        {
            // create a gameobject using the name supplied by client
            GameObject playergo = Instantiate(playerPrefab);
            playergo.GetComponent<Player>().playerName = createPlayerMessage.name;

            // set it as the player
            NetworkServer.AddPlayerForConnection(connection, playergo);

            chatWindow.gameObject.SetActive(true);
        }
    }
}
