using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class FurnitureUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI furnName;
    [SerializeField] Image furnImage;
    [SerializeField] Button button;
    UIManager uiManager;
    bool isInitialized;
    Sprite furnSprite;
    string furnID;

    public void Initialize(string furnName, string furnID,UIManager uiManager)
    {
        this.furnID = furnID;
        this.furnName.text = furnName;
        this.uiManager = uiManager;
        isInitialized = true;

    }

    private void OnClick()=>uiManager.OnSelectedFurniture(furnID);

    private void OnEnable()
    {
        if(isInitialized)
        {
           furnSprite= Resources.Load<Sprite>("Furn/Sprites/" + furnID);
            furnImage.sprite = furnSprite;
            furnImage.preserveAspect = true;
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnDisable()
    {
        if (isInitialized)
        {
            Resources.UnloadAsset(furnSprite);
            furnImage.sprite = null;
            furnImage.preserveAspect = false;
            button.onClick.RemoveListener(OnClick);
        }
    }

}
