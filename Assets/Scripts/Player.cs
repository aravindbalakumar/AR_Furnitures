using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    Furniture selectedFurnitureInstance;
    [HideInInspector] public string furnitureID = null;
    CurrentMode currentMode=CurrentMode.none;
    [Header("Input")]
    [SerializeField] private InputManager inputManager;
    float rotatespeed = 50;
    public UnityEvent<CurrentMode> OnCurrentInputMode;
    [Header("Raycast requirements")]
    [SerializeField] private LayerMask raycastFilter;
    private RaycastHandler raycastHandler = null;
    
    public void Init()
    {

        if (raycastHandler == null)
        {
            raycastHandler = new RaycastHandler(GameManager.Instance.ARCamera, raycastFilter); // first time creating a object for raycast handler
        }
        raycastHandler.OnHit+=OnMoveOrPlace;
    }
    public void Deinit()
    {
        raycastHandler.OnHit-=OnMoveOrPlace;
    }

    private void OnMoveOrPlace(RaycastHit hitObject)
    {
        if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("Furniture"))
        {
            Furniture hitFurnitureObject = hitObject.transform.GetComponent<Furniture>();
            if (hitFurnitureObject != null)
            {
                if (selectedFurnitureInstance != null)
                {

                    if (selectedFurnitureInstance.gameObject == hitFurnitureObject.gameObject)
                    {
                        selectedFurnitureInstance.Deselect();
                        GameManager.Instance.UIManager.OnControClose();
                    }
                }
                else
                {
                    selectedFurnitureInstance = hitFurnitureObject;
                    selectedFurnitureInstance.Select();
                    GameManager.Instance.UIManager.EnableControl(true);
                }
            }
        }
        else if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("ARPlane"))
        {
            if (selectedFurnitureInstance != null)
            {
                if (currentMode == CurrentMode.move)
                {
                    selectedFurnitureInstance.transform.position = hitObject.transform.position;
                }
            }

            if (selectedFurnitureInstance == null)
            {
                if (currentMode == CurrentMode.place)
                {
                    selectedFurnitureInstance = Instantiate<Furniture>(Resources.Load<Furniture>("/Furn/Prefabs/" + furnitureID));
                    selectedFurnitureInstance.Select();
                    selectedFurnitureInstance.transform.position = hitObject.transform.position;
                    GameManager.Instance.UIManager.EnableControl(true);
                    furnitureID = null;
                }
            }
            
        }
    }

    private void RotateObject( int dirVal)
    {
        if(selectedFurnitureInstance!=null)
        {
            selectedFurnitureInstance.transform.Rotate(Vector3.up, (dirVal * rotatespeed * Time.deltaTime));
        }
    }

    public void UpdateCurrentMode(CurrentMode currentMode)
    {
        this.currentMode = currentMode;
        if(this.currentMode== CurrentMode.move || this.currentMode==CurrentMode.place)
        {

            inputManager.OnTouch.AddListener(raycastHandler.RaycastFromScreen);
            inputManager.OnTouch_X_Direction.RemoveListener(RotateObject);
        }
        else if(this.currentMode== CurrentMode.rotate)
        {
            inputManager.OnTouch.RemoveListener(raycastHandler.RaycastFromScreen);
            inputManager.OnTouch_X_Direction.AddListener(RotateObject);
        }
        else
        {
            inputManager.OnTouch.RemoveListener(raycastHandler.RaycastFromScreen);
            inputManager.OnTouch_X_Direction.RemoveListener(RotateObject);
        }
    }

}
