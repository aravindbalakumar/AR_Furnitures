using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    Furniture selectedFurnitureInstance;
    Furn_Instance selectedFurnData = null;

    CurrentMode currentMode=CurrentMode.none;
    public UnityEvent<CurrentMode> OnCurrentModeUpdated;

    private void OnEnable() { GameManager.Instance.OnHit.AddListener(OnSelected); }
    private void OnDisable() { GameManager.Instance.OnHit.RemoveListener(OnSelected); }
    public void OnSelected(RaycastHit hitObject)
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
                    }
                }
                else
                {
                    selectedFurnitureInstance = hitFurnitureObject;
                    selectedFurnitureInstance.Select();
                }
            }
        }
        else if (hitObject.transform.gameObject.layer == LayerMask.NameToLayer("ARPlane"))
        {
            if(selectedFurnitureInstance!=null)
            {
                if (currentMode == CurrentMode.move)
                {
                    selectedFurnitureInstance.transform.position = hitObject.transform.position;
                }
                else if(currentMode == CurrentMode.place)
                {
                    selectedFurnitureInstance= GameObject.Instantiate<Furniture>( Resources.Load<Furniture>("/Furnitures/"+selectedFurnData.Furn_ID));
                    selectedFurnitureInstance.Select();
                    selectedFurnitureInstance.transform.position=hitObject.transform.position;
                    UpdateCurrentMode(CurrentMode.move);
                }
            }
        }
    }
    public void UpdateCurrentMode(CurrentMode currentMode)
    {
        this.currentMode = currentMode;
    }

}
