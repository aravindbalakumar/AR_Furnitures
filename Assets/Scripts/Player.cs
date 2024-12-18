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
    /// <summary>
    /// saves previous position and rotation
    /// </summary>
    public void SavePreviousState()
    {
        selecterFurniturePosition = selectedFurnitureInstance.transform.position;
        selecterFurnitureRotation = selectedFurnitureInstance.transform.rotation;
    }

    /// <summary>
    /// Called when raycast hits on furniture or plane subscribed to raycast hit
    /// </summary>
    /// <param name="hitObject">the  raycast hit</param>
    private void OnSelectOrMoveOrPlace(RaycastHit hitObject)
    {
        if(hitObject.transform.gameObject == null) // checks if it is a gameObject or not
        {
            return;
        }

        if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("Furniture")) // checking wether the raycst hhit object is furniture or not
        {
            Furniture hitFurnitureObject = hitObject.transform.GetComponent<Furniture>(); // check if it has furniture object
            if (hitFurnitureObject == null)
            {
                return;
            }
            if(selectedFurnitureInstance==null) //if selectedfurniture is already empty assign the hitted one
            {
                selectedFurnitureInstance = hitFurnitureObject;
                selectedFurnitureInstance.Select();
                GameManager.Instance.UIManager.EnableControl(true);

            }
            else if(selectedFurnitureInstance.gameObject!= hitFurnitureObject.gameObject) // if selected is already there then replace it
            {
                selectedFurnitureInstance.Deselect();
                selectedFurnitureInstance = hitFurnitureObject;
                selectedFurnitureInstance.Select();
                GameManager.Instance.UIManager.EnableControl(true);
            }
        }
        else if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("ARPlane"))// if the raycast hits the ARPlane
        {
            switch(currentMode)
            {
                case CurrentMode.place:
                    if(furnitureID.isValidString()) // if i have selected a furniture with ID
                    {
                        selectedFurnitureInstance = Instantiate<Furniture>(Resources.Load<Furniture>("Furn/Prefabs/" + furnitureID)); //spawn the Furniture 
                        selectedFurnitureInstance.Select();
                        selectedFurnitureInstance.transform.position = hitObject.point;
                        GameManager.Instance.UIManager.EnableControl(true);
                        furnitureID = null;
                    }
                    break;
                case CurrentMode.move:
                    
                    if(selectedFurnitureInstance!=null)
                    {
                       
                        selectedFurnitureInstance.transform.position = hitObject.point; //update the selected furniture to this position
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
    /// <summary>
    /// Rotates the object
    /// </summary>
    /// <param name="dirVal">the directionalvalue</param>
    private void RotateObject( int dirVal)
    {
        if(selectedFurnitureInstance!=null)
        {
            selectedFurnitureInstance.transform.Rotate(Vector3.up, (dirVal * rotatespeed * Time.deltaTime));
        }
    }
    /// <summary>
    /// Updates the current Input mode
    /// </summary>
    /// <param name="currentMode">the current input mode</param>
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
    /// <summary>
    /// Called when edit mode is exited by cancelling
    /// </summary>
    public void EditCancel()
    {
        selectedFurnitureInstance.transform.position = selecterFurniturePosition;
        selectedFurnitureInstance.transform.rotation = selecterFurnitureRotation;
    }

    /// <summary>
    /// called when the furniture is deleted
    /// </summary>
    public void DeleteFurniture()
    {
        Destroy(selectedFurnitureInstance.gameObject);
        ResetSelectedFuriture();
        UpdateCurrentMode(CurrentMode.none);
    }
    /// <summary>
    /// reset the selected furniture
    /// </summary>
    public void ResetSelectedFuriture()
    {
        selectedFurnitureInstance = null;
    }
}
