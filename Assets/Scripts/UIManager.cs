using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject sideMenu;
    [SerializeField] Sprite[] categorySprites;
    [SerializeField] GameObject backButton;
    [SerializeField] Transform categoryContentParent;
    [SerializeField] CategoryUI categoryUIPrefab;
    [SerializeField] Transform furnitureContentParent;
    [SerializeField] FurnitureUI furnitureUIPrefab;
    Dictionary<Category, List<FurnitureUI>> categorizedFurnitures;
    Category? selectedCatergory = null;
    [SerializeField] GameObject controlPanel;
    [SerializeField] GameObject controlMenu;
    [SerializeField] GameObject OkayButton;

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
        Spawnfurnitures();
    }

    public void FilterCategory(Category category)
    {
        furnitureContentParent.gameObject.SetActive(true);
        backButton.SetActive(true);
        categoryContentParent.gameObject.SetActive(false);

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

    public void OnSelectedFurniture(string furnID)
    {
        GameManager.Instance.Player.furnitureID = furnID;
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.place);
    }
    public void Back()
    {
        furnitureContentParent.gameObject.SetActive(false);
        backButton.SetActive(false);
        categoryContentParent.gameObject.SetActive(true);
        if (selectedCatergory.HasValue)
        {
            categorizedFurnitures[selectedCatergory.Value].ForEach(x => x.gameObject.SetActive(false));
            selectedCatergory = null;
        }
    }

    public void EnableControl(bool resetMenu=false)
    {
        if (resetMenu == true)
        {
            OnControClose();
        }
        sideMenu?.SetActive(false);
        controlPanel.SetActive(true);
        controlMenu.SetActive(true);
    }

    public void OnMoveClick()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.move);
        controlMenu.SetActive(false);
        OkayButton.gameObject.SetActive(true);
    }
    public void OnRotateClick()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.rotate);
        controlMenu.SetActive(false);
        OkayButton.gameObject.SetActive(true);
    }
    public void OnEditDone()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.none);
        OkayButton.gameObject.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void OnControClose()
    {
        GameManager.Instance.Player.UpdateCurrentMode(CurrentMode.none);
        OkayButton.gameObject.SetActive(false);
        sideMenu.SetActive(true);
        controlPanel.SetActive(false);
        controlMenu.SetActive(false);
    }
    private void Spawnfurnitures()
    {
        categorizedFurnitures = new Dictionary<Category, List<FurnitureUI>>();
        foreach (var furnData in GameManager.Instance.data.Furnitures)
        {
            FurnitureUI furnUIInstance = Instantiate(furnitureUIPrefab, furnitureContentParent);
            furnUIInstance.Initialize(furnData.Furniture_Name, furnData.Furniture_ID,this);
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
