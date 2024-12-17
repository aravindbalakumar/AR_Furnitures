using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    CurrentMode currentMode=CurrentMode.none;
    [Header("Input")]
    [SerializeField] private InputManager inputManager;
    float rotatespeed = 250;
    public UnityEvent<CurrentMode> OnCurrentInputMode;
    [Header("Raycast requirements")]
    [SerializeField] private LayerMask raycastFilter;
    private RaycastHandler raycastHandler = null;


    Furniture selectedFurnitureInstance;
    [HideInInspector] public string furnitureID = null;
    private Vector3 selecterFurniturePosition;
    private Quaternion selecterFurnitureRotation;

    public void Init()
    {

        if (raycastHandler == null)
        {
            raycastHandler = new RaycastHandler(GameManager.Instance.ARCamera, raycastFilter); // first time creating a object for raycast handler
        }
        raycastHandler.OnHit+=OnSelectOrMoveOrPlace;
    }
    public void Deinit()
    {
        raycastHandler.OnHit-= OnSelectOrMoveOrPlace;
    }
    public void SavePreviousState()
    {
        selecterFurniturePosition = selectedFurnitureInstance.transform.position;
        selecterFurnitureRotation = selectedFurnitureInstance.transform.rotation;
    }
    private void OnSelectOrMoveOrPlace(RaycastHit hitObject)
    {
        if(hitObject.transform.gameObject == null)
        {
            return;
        }

        if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("Furniture"))
        {
            Furniture hitFurnitureObject = hitObject.transform.GetComponent<Furniture>();
            if (hitFurnitureObject == null)
            {
                return;
            }
            if(selectedFurnitureInstance==null)
            {
                selectedFurnitureInstance = hitFurnitureObject;
                selectedFurnitureInstance.Select();
                GameManager.Instance.UIManager.EnableControl(true);

            }
            else if(selectedFurnitureInstance.gameObject!= hitFurnitureObject.gameObject)
            {
                selectedFurnitureInstance.Deselect();
                selectedFurnitureInstance = hitFurnitureObject;
                selectedFurnitureInstance.Select();
                GameManager.Instance.UIManager.EnableControl(true);
            }
        }
        else if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("ARPlane"))
        {
            switch(currentMode)
            {
                case CurrentMode.place:
                    if(furnitureID.isValidString())
                    {
                        selectedFurnitureInstance = Instantiate<Furniture>(Resources.Load<Furniture>("Furn/Prefabs/" + furnitureID));
                        selectedFurnitureInstance.Select();
                        selectedFurnitureInstance.transform.position = hitObject.point;
                        GameManager.Instance.UIManager.EnableControl(true);
                        furnitureID = null;
                    }
                    break;
                case CurrentMode.move:
                    
                    if(selectedFurnitureInstance!=null)
                    {
                       
                        selectedFurnitureInstance.transform.position = hitObject.point;
                    }
                    break;
                case CurrentMode.rotate:
                    if (selectedFurnitureInstance != null)
                    {
                    }
                    break;
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
        if (this.currentMode== CurrentMode.move || this.currentMode==CurrentMode.place)
        {
            
            inputManager.OnTouch.AddListener(raycastHandler.RaycastFromScreen);
            inputManager.OnTouch_X_Direction.RemoveListener(RotateObject);
        }
        else if(this.currentMode== CurrentMode.rotate)
        {
            inputManager.OnTouch.RemoveListener(raycastHandler.RaycastFromScreen);
            inputManager.OnTouch_X_Direction.AddListener(RotateObject);
        }
        else if(this.currentMode==CurrentMode.none)
        {
            inputManager.OnTouch.AddListener(raycastHandler.RaycastFromScreen);
            inputManager.OnTouch_X_Direction.RemoveListener(RotateObject);
        }
    }

    public void OnEndLevel(int stageendID)
    {
        if(stageendID==0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void EditCancel()
    {
        selectedFurnitureInstance.transform.position = selecterFurniturePosition;
        selectedFurnitureInstance.transform.rotation = selecterFurnitureRotation;
    }
    public void DeleteFurniture()
    {
        Destroy(selectedFurnitureInstance.gameObject);
        ResetSelectedFuriture();
        UpdateCurrentMode(CurrentMode.none);
    }
    public void ResetSelectedFuriture()
    {
        selectedFurnitureInstance = null;
    }
}
