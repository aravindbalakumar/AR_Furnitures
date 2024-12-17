
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] public OnScreenDebug onscreenDebug;
    [SerializeField] public Camera ARCamera;
    [SerializeField] private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
    }
    [SerializeField] private UIManager uiManager;

    public UIManager UIManager
    {
        get
        {
            return uiManager;
        }
    }

    [SerializeField] TotalFurnitureData totalData;
    public TotalFurnitureData data
    {
        get
        {
            return totalData;
        }
    }
    [SerializeField] string dataPath;
    [HideInInspector]public bool isInitialized=false;
    

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static GameManager _instance;


    /// <summary>
    /// Awake function called when initialized
    /// </summary>
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            _instance = this; // singleton for global access
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // destroy any other duplicates
        }
        LoadData();
        InitializeComponents();
    }

    private void LoadData()
    {
        totalData = JsonConvert.DeserializeObject<TotalFurnitureData>(Resources.Load<TextAsset>("Data").text);
        
    }
    
    private void InitializeComponents()
    {
        if (!isInitialized) //check if it is not Initialized for creating objects first time and safety subscribtion
        {
            
            player.Init();
            uiManager.Initialize();
            isInitialized = true;
        }

    }

    private void OnDestroy()
    {
        if (isInitialized) // if it is initialized then safely unsubscribe the funtions
        {
            player.Deinit();
        }
    }
}
