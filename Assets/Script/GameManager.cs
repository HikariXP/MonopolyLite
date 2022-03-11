using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    /// <summary>
    /// 如果激活，就会开始后直接加载到TitleScene，测试模式下不需要用到这个
    /// </summary>
    public bool loadTitle;

    public DIYSceneManager sceneManager;

    /// <summary>
    /// 用于保存本地数据的二进制转换器
    /// </summary>
    private BinaryFormatter bf = new BinaryFormatter();

    /// <summary>
    /// 当前加载的账户信息
    /// </summary>
    public Account currentAccount { get; set; }

    void Awake()
    {
        Init();
        LoadData();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadAccountData();
        LoadData();
        if (loadTitle)
        {
            ToTitle();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// [测试]
    /// 用于初始化到标题界面
    /// </summary>
    private void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// 自我初始化
    /// </summary>
    private void Init()
    {
        //让GM本体加载不销毁
        DontDestroyOnLoad(this.gameObject);

        //为全局静态引用赋值(无线程保护)
        Instance = this;

        //为场景转换器设置全局静态引用
        sceneManager = gameObject.GetComponent<DIYSceneManager>();
        if (sceneManager == null)
        {
            Debug.LogError("DIYSceneManager Can't Find.");
        }
    }


    /// <summary>
    /// 统一保存数据：Account、SettingInfo
    /// </summary>
    public void SaveData()
    { 
    
    }

    /// <summary>
    /// 统一加载数据：Account、SettingInfo
    /// </summary>
    public void LoadData()
    { 
    
    }

    public void CreateAccount(string name)
    {
        currentAccount = new Account();
        currentAccount.AccountName = name;
    }

    /// <summary>
    /// 保存预设数据：Account
    /// </summary>
    public void SaveAccountData()
    {
        

        FileStream fileStream = File.Create(Application.persistentDataPath + "Account.save");

        bf.Serialize(fileStream,currentAccount);

        fileStream.Close();
        
    }

    /// <summary>
    /// 加载预设数据：Account
    /// </summary>
    public void LoadAccountData()
    {
        if (File.Exists(Application.persistentDataPath + "Account.save"))
        {
            FileStream fileStream = File.OpenRead(Application.persistentDataPath+"Account.save");

            if (fileStream.Length == 0)
            {
                currentAccount = null;

            }
            else
            {
                currentAccount = (Account)bf.Deserialize(fileStream);

                
            }
            fileStream.Close();

        }
        else
        {
            OnDataNotFound();
        }
    }

    /// <summary>
    /// 保存预设数据：SettingInfo
    /// </summary>
    public void SaveSettingInfo()
    {
        FileStream fileStream = File.Create(Application.persistentDataPath + "SettingInfo.save");
    }


    /// <summary>
    /// 当无法从本地存档中读取预设数据的时候启动
    /// </summary>
    public void OnDataNotFound()
    { 
        
    }
    
}
