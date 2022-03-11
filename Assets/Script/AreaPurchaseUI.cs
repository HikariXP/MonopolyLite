using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaPurchaseUI : MonoBehaviour
{
    public Text AreaNameText;

    public Text PriceText;

    public void DisplayPurchaseInfo(string areaName, string price)
    {
        AreaNameText.text = areaName;
        PriceText.text = price;
    }
}
