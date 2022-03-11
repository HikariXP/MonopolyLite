using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUpgradeUI : MonoBehaviour
{
    public Text AreaNameText;

    public Text LevelText;

    public Text PriceText;

    public void DisplayUpgradeInfo(string areaName,string currentLevel,string price)
    {
        AreaNameText.text = areaName;
        LevelText.text = currentLevel;
        PriceText.text = price;
    }
}
