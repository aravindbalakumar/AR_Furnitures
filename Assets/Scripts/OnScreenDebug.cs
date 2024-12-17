using UnityEngine;
using System.Collections;
using TMPro;

public class OnScreenDebug : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI debugText;
    public void Debug(string incomingText)
    {
        if(debugText.text.isValidString())
        {
            debugText.text = debugText.text + System.Environment.NewLine+incomingText;
        }
        else
        {
            debugText.text = incomingText;
        }
    }
}
