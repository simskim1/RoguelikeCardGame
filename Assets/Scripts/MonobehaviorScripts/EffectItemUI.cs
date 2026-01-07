using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EffectItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;

    public void Setup(string title, string description)
    {
        titleText.text = title;
        descText.text = description;
        //effectImage.sprite = image;
    }
}