using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] Button category_button;
    [SerializeField] Image category_Image;
    UIManager uiManagerRef;
    Category category;
    [SerializeField] TextMeshProUGUI category_Name;
    public void OnEnable()
    {
        category_button.onClick.AddListener(OnClick);
    }
    public void OnDisable()
    {
        category_button.onClick.RemoveListener(OnClick);
    }
    public void InitCategory(Category category, Sprite sprite, UIManager uiManagerRef)
    {
        category_Image.sprite = sprite;
        category_Image.preserveAspect = true;
        this.category = category;
        this.uiManagerRef = uiManagerRef;
        category_Name.text = category.ToString();

    }
    public void OnClick()
    {
        if (uiManagerRef == null)
        {
            return;
        }
        uiManagerRef.FilterCategory(this.category);
    }
}
