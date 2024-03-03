using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    private Image menuImg;
    private Color clearCol;
    private Color transCol = new Color32(190, 40, 88, 150);

    void Start()
    {
        isPointerIn = false;
        menuImg = gameObject.GetComponent<Image>();
        menuImg.color = clearCol;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Menu"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Menu"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
        if (isPointerIn)
        {
            menuImg.color = transCol;
        }
        else
        {
            menuImg.color = clearCol;
        }
    }
}
