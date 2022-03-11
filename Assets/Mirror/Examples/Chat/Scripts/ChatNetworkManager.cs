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

        //����������֮�󣬻�����ԭ���Ĵ���(base.OnStartServer())֮��ע��һ��������ڴ����Զ������
        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        //��дԴ���룬����Դ�ͻ��������ύ��NetworkConnection������Ϣ�������Ԥ����
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            // tell the server to create a player with this name
            conn.Send(new CreatePlayerMessage { name = PlayerName });
        }

        //����һ�����Ԥ���壬���һ�����Ϣ�޸����֣���Ϊ�丳����ҵ�����
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
