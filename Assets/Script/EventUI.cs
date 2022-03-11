using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    //事件名字
    public Text EventNameText;

    //事件描述
    public Text EventInfoText;

    //事件效果
    public Text EventEffectText;

    public void DisplayEvent(string name,string info,string effectinfo)
    {
        EventNameText.text = name;
        EventInfoText.text = info;
        EventEffectText.text = effectinfo;
    }
}
