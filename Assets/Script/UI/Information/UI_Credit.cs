using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Credit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    private Image creditImg;
    private Color clearCol;
    private Color transCol = new Color32(255, 255, 255, 81);

    void Start()
    {
        isPointerIn = false;
        creditImg = gameObject.GetComponent<Image>();
        creditImg.color = clearCol;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Credit"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Credit"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
        if (isPointerIn)
        {
            creditImg.color = transCol;
        }
        else
        {
            creditImg.color = clearCol;
        }
    }
}
