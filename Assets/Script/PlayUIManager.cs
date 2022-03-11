using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror.Discovery;

/// <summary>
/// 战局UI总管理器,后期将分离部分职责，demo期间全权负责玩家UI
/// </summary>
public class PlayUIManager : MonoBehaviour
{
    public static PlayUIManager Instance;

    /// <summary>
    /// 直接放入引用
    /// </summary>
    public PlayManager playManager;
    //需要检测的玩家
    public Player LocalPlayer;

    [Header("UI BeforeGameStart")]


    //匹配界面父节点
    public GameObject MatchPanelGO;
        //模式选择
        public GameObject ModeSelectPanelGO;
        //搜索中的界面
        public GameObject SearchingPanelGO;

    public GameObject HostPanel;

    public GameObject ClientPanel;

        //开始游戏的按钮(主机端才有)
        public GameObject StartGameButton;
        //开始按钮上的玩家数量显示
        public Text PlayerAmountInRoomText;
        //玩家输入的IP
        public InputField ClientIPInput;

    [Header("NotificationPanel")]
    public GameObject NotificationPanelGO;
    //购买面板
    public GameObject PurchasePanelGO;
    //升级面板
    public GameObject UpgradePanelGO;
    //事件面板
    public GameObject EventPanelGO;

    //选择模式后等待的提示，主机模式：等待中、客户端模式：搜索中
    public Text MatchTipsText;

    [Header("Gaming UI")]
    //抬头显示UI节点
    public GameObject HUD_PanelGO;
    //回合内UI
    public GameObject OnTurnPanelGO;

    public Text PlayerMoneyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MatchPanelGO.SetActive(true);
        ModeSelectPanelGO.SetActive(true);
    }

    

    private void Update()
    {
        CheckCanStartGame();
        DisplayPlayerMoney();
    }

    private void OnEnable()
    {
        //playManager.PlayerMoneyChangeEvent += DisplayPlayerMoney;
    }

    private void OnDisable()
    {
        //playManager.PlayerMoneyChangeEvent -= DisplayPlayerMoney;
    }

    /// <summary>
    /// 回到主标题
    /// </summary>
    public void OnBackToTitle()
    {
        GameManager.Instance.sceneManager.LoadScene("Title");
    }

    /// <summary>
    /// 返回到模式选择界面
    /// </summary>
    public void OnCallBackToModePanel()
    {
        SearchingPanelGO.SetActive(false);
        ModeSelectPanelGO.SetActive(true);
    }

    public void OnCallGameStart()
    {
        LocalPlayer.OnCallStartGame();
    }

    /// <summary>
    /// 点击创建房间模式
    /// </summary>
    public void OnCallStartHost()
    {
        ModeSelectPanelGO.SetActive(false);
        SearchingPanelGO.SetActive(true);
        HostPanel.SetActive(true);
        ClientPanel.SetActive(false);
    }

    /// <summary>
    /// 点击加入房间模式
    /// </summary>
    public void OnCallStartClient()
    {
        ModeSelectPanelGO.SetActive(false);
        SearchingPanelGO.SetActive(true);
        StartGameButton.SetActive(false);
        ClientPanel.SetActive(true);
        HostPanel.SetActive(false);
    }
    //确认链接
    public void OnTryToConnectToIp()
    {

    }

    /// <summary>
    /// [即将弃置]当发现服务器的时候展示出来
    /// </summary>
    public void OnDiscoverServer()
    {
        //RefreshServerList();
    }

    /// <summary>
    /// [即将弃置]刷新目前侦测到的服务器展示
    /// </summary>
    public void RefreshServerList()
    {
    }

    /// <summary>
    /// 链接到服务器的时候刷新UI
    /// </summary>
    public void OnConnectToServer()
    {
        MatchTipsText.text = "等待开始...";
        Debug.Log("LinkToServer");
    }

    /// <summary>
    /// 检查玩家人数是否可以开始游戏，同时修改开始按钮
    /// </summary>
    public void CheckCanStartGame()
    {
        PlayerAmountInRoomText.text = "x" + PlayManager.Instance.PlayerConnectionList.Count;
        if (PlayManager.Instance.canStartGame)
        {
            StartGameButton.GetComponent<Button>().interactable = true;
        }
        else StartGameButton.GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// 游戏开始后，取消MatchUIPanel激活，激活响应战局UI
    /// </summary>
    public void OnGameStart()
    {
        Debug.Log("玩家对UI发起游戏开始请求");

        //激活对应的UI
        MatchPanelGO.SetActive(false);


        HUD_PanelGO.SetActive(true);
        NotificationPanelGO.SetActive(true);

        DisplayPlayerMoney();
        
    }

    /// <summary>
    /// 到达本机回合：开启相关UI
    /// </summary>
    
    public void OnStartTurn()
    {
        OnTurnPanelGO.SetActive(true);

        //[即将弃用]开始回合的时候跟随玩家
        CameraController.Instance.FollowPlayerCamera.Follow = LocalPlayer.transform;
    }

    /// <summary>
    /// 结束本机回合：关闭相关UI
    /// </summary>
    public void OnEndTurn()
    {
        

        OnTurnPanelGO.SetActive(false);
    }

    /// <summary>
    /// 玩家按钮发起结束回合行动
    /// </summary>
    public void PlayerCallEndTurn()
    {
        //LocalPlayer.OnCallEndTurn();
        LocalPlayer.OnEndLocalPlayerTurn();
    }

    public void DisplayPlayerMoney()
    {
        if (LocalPlayer != null)
        {
            PlayerMoneyText.text = LocalPlayer.Money.ToString("N");
        }
    }

    public void DisplayAreaPurchaseInfo(Area_Building area_Building)
    {
        PurchasePanelGO.SetActive(true);
        PurchasePanelGO.GetComponent<AreaPurchaseUI>().DisplayPurchaseInfo(area_Building.AreaName,area_Building.BasicPrice.ToString());
    }

    public void DisplayAreaUpgradeInfo(Area_Building area_Building)
    {
        UpgradePanelGO.SetActive(true);
        UpgradePanelGO.GetComponent<AreaUpgradeUI>().DisplayUpgradeInfo(area_Building.AreaName,("Lv"+area_Building.CurrentLevel), area_Building.BasicPrice.ToString());
    }

    public void PlayerAgreePurchase()
    {
        LocalPlayer.AgreeAreaPurchase();
        PurchasePanelGO.SetActive(false);
    }

    public void PlayerAgreeUpgrade()
    {
        LocalPlayer.AgreeAreaUpgrade();
        UpgradePanelGO.SetActive(false);
    }
}
