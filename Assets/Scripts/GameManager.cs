
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Player player;

    [Header("Raycast requirements")]
    [SerializeField] private Camera raycastCamera;
    [SerializeField] private LayerMask raycastFilter;

    
    private RaycastHandler raycastHandler = null;
    [SerializeField] TotalFurnitureData totalData;
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
    public UnityEvent<RaycastHit> OnHit
    {
        get { return raycastHandler.OnHit; }
    }
    public UnityEvent<CurrentMode> OnCurrentMode
    {
        get { return player.OnCurrentModeUpdated; }
    }


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
        dataPath = Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar + "Data.json";
        if (System.IO.File.Exists(dataPath))
        {
            totalData = JsonConvert.DeserializeObject<TotalFurnitureData>(System.IO.File.ReadAllText(dataPath));
            
        }
    }
    private void OnEnable() => planeManager.planesChanged += OnARPlanesChanged;
    
    private void OnARPlanesChanged(ARPlanesChangedEventArgs changes)
    {
        if (!isInitialized) //check if it is not Initialized for creating objects first time and safety subscribtion
        {
            if (raycastHandler == null)
            {
                raycastHandler = new RaycastHandler(raycastCamera, raycastFilter); // first time creating a object for raycast handler
            }
            
            inputManager.OnTouch.AddListener(raycastHandler.RaycastFromScreen);
            isInitialized = true;
        }

    }
    

    private void OnDisable() => planeManager.planesChanged -= OnARPlanesChanged;

    private void OnDestroy()
    {
        if (isInitialized) // if it is initialized then safely unsubscribe the funtions
        {
            inputManager.OnTouch.RemoveListener(raycastHandler.RaycastFromScreen);
        }
    }
}
