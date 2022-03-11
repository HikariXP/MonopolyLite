using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 整个TitleScene的整体UI管控
/// </summary>
public class TitleUIManager : MonoBehaviour
{
    //创建全局引用
    public static TitleUIManager Instance;
    /// <summary>
    /// 初次进入游戏输入名字的面板
    /// </summary>
    public GameObject FirstNamePanelGO;

    public InputField FirstNameInputField;

    public GameObject TitlePanelGO;

    public Text VersionText;

    public Text WelcomeText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckHaveAccount();
    }

    /// <summary>
    /// 检查GM中是否存在Account，否则激活初次输入面板
    /// </summary>
    private void CheckHaveAccount()
    {
        if (GameManager.Instance.currentAccount == null||GameManager.Instance.currentAccount.AccountName=="")
        {
            OnEnableFirstNamePanel();
        }
        else OnEnableTitlePanel();
    }

    #region 初次面板

    /// <summary>
    /// 激活初次改名字的面板
    /// </summary>
    public void OnEnableFirstNamePanel()
    {
        OnDisableTitlePanel();
        FirstNamePanelGO.SetActive(true);
    }

    /// <summary>
    /// 取消初次面板激活
    /// </summary>
    public void OnDisableFirstNamePanel()
    {
        FirstNamePanelGO.SetActive(false);
    }

    public void OnEnableTitlePanel()
    {
        OnDisableFirstNamePanel();
        TitlePanelGO.SetActive(true);
        RefreshTitleText();
    }

    public void RefreshTitleText()
    {
        VersionText.text = "Version:"+Application.version;
        WelcomeText.text = "欢迎回来，" + GameManager.Instance.currentAccount.AccountName;
    }

    public void OnDisableTitlePanel()
    {
        TitlePanelGO.SetActive(false);
    }

    /// <summary>
    /// 初次输入面板中确认名字
    /// </summary>
    public void OnConfirmFirstName()
    {
        if (CheckName())
        {
            GameManager.Instance.CreateAccount(FirstNameInputField.text);
            GameManager.Instance.SaveAccountData();
            OnEnableTitlePanel();
        }
        else
        { 
            //提示玩家名字输入有问题
        }
    }

    /// <summary>
    /// 检查名字
    /// </summary>
    /// <returns>合格与否</returns>
    public bool CheckName()
    {
        string temp = FirstNameInputField.text;
        if (temp.Length > 0) return true;
        else return false;
    }

    /// <summary>
    /// 点击开始游戏按钮
    /// </summary>
    public void OnToPlayScene()
    {
        GameManager.Instance.sceneManager.LoadScene("Play");
    }

    #endregion
}
