using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] Sprite[] categorySprites;
    [SerializeField] GameObject sideMenu;
    [SerializeField] GameObject backButton;

    [SerializeField] GameObject sel_furn_UI_panel;
    [SerializeField] TextMeshProUGUI sel_furn_UI_name;
    [SerializeField] Image sel_furn_UI_image;
    [SerializeField] TextMeshProUGUI title;
    [Header("Prefabs and parents")]
    [SerializeField] Transform categoryContentParent;
    [SerializeField] CategoryUI categoryUIPrefab;
    [SerializeField] Transform furnitureContentParent;
    [SerializeField] FurnitureUI furnitureUIPrefab;
    [Header("Single Furn edit Control")]
    [SerializeField] GameObject furn_Edit_Option_Menu;
    [SerializeField] GameObject furn_Edit_Control_Menu;
    [SerializeField] GameObject furn_Edit_Confirm_menu;
    Dictionary<Category, List<FurnitureUI>> categorizedFurnitures;
    Category? selectedCatergory = null;
    /// <summary>
    /// Function that spawnscategory and furnitures
    /// </summary>
    public void Initialize()
    {
        CategoryUI chairCat = Instantiate(categoryUIPrefab, categoryContentParent);
        chairCat.InitCategory(Category.Chair, categorySprites[0], this);

        CategoryUI shelfCat = Instantiate(categoryUIPrefab, categoryContentParent);
        shelfCat.InitCategory(Category.Shelf, categorySprites[1], this);

        CategoryUI tableCat = Instantiate(categoryUIPrefab, categoryContentParent);
        tableCat.InitCategory(Category.Table, categorySprites[2], this);

        CategoryUI bedCat = Instantiate(categoryUIPrefab, categoryContentParent);
        bedCat.InitCategory(Category.Bed, categorySprites[3], this);

        CategoryUI sofaCat = Instantiate(categoryUIPrefab, categoryContentParent);
        sofaCat.InitCategory(Category.Sofa, categorySprites[4], this);

        CategoryUI otherCat = Instantiate(categoryUIPrefab, categoryContentParent);
        otherCat.InitCategory(Category.Other, categorySprites[5], this);
        SpawnfurnituresUI();
    }
    /// <summary>
    /// Filter category based furnitures
    /// </summary>
    /// <param name="category"></param>
    public void FilterCategory(Category category)
    {
        furnitureContentParent.gameObject.SetActive(true);
        backButton.SetActive(true);
        categoryContentParent.gameObject.SetActive(false);
        title.text = category.ToString();
        if (selectedCatergory.HasValue)
        {
            categorizedFurnitures[selectedCatergory.Value].ForEach(x => x.gameObject.SetActive(false));
            selectedCatergory = category;
            categorizedFurnitures[selectedCatergory.Value].ForEach(x => x.gameObject.SetActive(true));
        }
        else
        {
            selectedCatergory = category;
            categorizedFurnitures[selectedCatergory.Value].ForEach(x => x.gameObject.SetActive(true));
        }

    }

    /// <summary>
    /// Selected furniture
    /// </summary>
    /// <param name="furnID">used for identifying the furniture</param>
    /// <param name="furnName"> the furniture name to display</param>
    /// <param name="furnSprite">the furniture sprite to display</param>
    public void SelectFurnitureID(string furnID, string furnName, Sprite furnSprite)
    {
        GameManager.Instance.Player.furnitureID = furnID;
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.place);
        sel_furn_UI_name.text = furnName;
        sel_furn_UI_image.sprite = furnSprite;
        sel_furn_UI_image.preserveAspect = true;
        sel_furn_UI_panel.gameObject.SetActive(true);
    }
    /// <summary>
    /// Deselct furniture
    /// </summary>
    public void DeselectFurnitureID()
    {
        sel_furn_UI_panel.gameObject.SetActive(false);
        GameManager.Instance.Player.furnitureID = null;
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.none);
    }
    /// <summary>
    /// Back button call back in side panel
    /// </summary>
    public void Back()
    {
        furnitureContentParent.gameObject.SetActive(false);
        backButton.SetActive(false);
        title.text = "Select a Category";
        categoryContentParent.gameObject.SetActive(true);
        if (selectedCatergory.HasValue)
        {
            categorizedFurnitures[selectedCatergory.Value].ForEach(x => x.gameObject.SetActive(false));
            selectedCatergory = null;
        }
    }


    #region single furniture edit control
    /// <summary>
    /// Enable the control panel
    /// </summary>
    /// <param name="resetMenu">should reset</param>
    public void EnableControl(bool resetMenu = false)
    {
        if (resetMenu == true)
        {
            ResetMenu();
        }
        sideMenu?.SetActive(false);
        furn_Edit_Option_Menu.SetActive(true);
        furn_Edit_Control_Menu.SetActive(true);
    }
    /// <summary>
    /// Called when move button is clicked, will enter edit mode
    /// </summary>
    public void OnMoveClick()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.move);
        GameManager.Instance.Player.SavePreviousState();
        furn_Edit_Control_Menu.SetActive(false);
        furn_Edit_Confirm_menu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when rotate button is clicked, will enter Edit mode
    /// </summary>
    public void OnRotateClick()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.rotate);
        GameManager.Instance.Player.SavePreviousState();
        furn_Edit_Control_Menu.SetActive(false);
        furn_Edit_Confirm_menu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when transform edit is done
    /// </summary>
    public void OnTransformUpdateDone()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.none);
        furn_Edit_Confirm_menu.gameObject.SetActive(false);
        furn_Edit_Control_Menu.SetActive(true);
    }

    /// <summary>
    /// Called whhen transform edit is cancelled
    /// </summary>
    public void OnTransformUpdateCancel()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.none);
        furn_Edit_Confirm_menu.gameObject.SetActive(false);
        furn_Edit_Control_Menu.SetActive(true);
        GameManager.Instance.Player.EditCancel();
    }
    /// <summary>
    /// Closes the control menu
    /// </summary>
    public void CloseControl()
    {
        GameManager.Instance.Player.ResetSelectedFuriture();
        ResetMenu();
    }
    /// <summary>
    /// Resets the menu
    /// </summary>
    private void ResetMenu()
    {
        furn_Edit_Confirm_menu.gameObject.SetActive(false);
        sideMenu.SetActive(true);
        sel_furn_UI_panel.SetActive(false);
        furn_Edit_Option_Menu.SetActive(false);
        furn_Edit_Control_Menu.SetActive(false);
    }

    /// <summary>
    /// Delet the seelected furniture
    /// </summary>
    public void DeleteFurniture()
    {
        ResetMenu();
        GameManager.Instance.Player.DeleteFurniture();
    }
    #endregion

    /// <summary>
    /// Spawns all the furniture UI
    /// </summary>
    private void SpawnfurnituresUI()
    {
        categorizedFurnitures = new Dictionary<Category, List<FurnitureUI>>();
        foreach (var furnData in GameManager.Instance.data.Furnitures)
        {
            FurnitureUI furnUIInstance = Instantiate(furnitureUIPrefab, furnitureContentParent);
            furnUIInstance.Initialize(furnData.Furniture_Name, furnData.Furniture_ID, this);
            if (categorizedFurnitures.ContainsKey(furnData.Category))
            {

                categorizedFurnitures[furnData.Category].Add(furnUIInstance);
            }
            else
            {
                categorizedFurnitures.Add(furnData.Category, new List<FurnitureUI> { furnUIInstance });
            }
            furnUIInstance.gameObject.SetActive(false);

        }
    }

}
