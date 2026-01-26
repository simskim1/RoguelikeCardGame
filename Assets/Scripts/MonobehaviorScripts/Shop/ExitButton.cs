using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class ExitButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject ShopUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopUI.SetActive(false);
    }
}
