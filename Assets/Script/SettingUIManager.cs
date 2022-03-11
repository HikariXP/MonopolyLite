using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    public InputField PlayerInputNewText;
    public Text InputPlaceHolder;

    public void OnConfirmChangeName()
    {
        if (PlayerInputNewText.text.Length > 0)
        {
            GameManager.Instance.currentAccount.AccountName = PlayerInputNewText.text;
            GameManager.Instance.SaveAccountData();
            RefreshInputNameDisplay();
            TitleUIManager.Instance.RefreshTitleText();
        }
    }

    public void RefreshInputNameDisplay()
    {
        InputPlaceHolder.text = "现用名字：" +GameManager.Instance.currentAccount.AccountName;
        PlayerInputNewText.text = "";
    }

    public void OnEnable()
    {
        RefreshInputNameDisplay();
    }

}
