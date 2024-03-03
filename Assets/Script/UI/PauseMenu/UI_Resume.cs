using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Resume : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private bool isPointerIn;
    private Image resumeImg;
    private Color clearCol;
    private Color transCol = new Color32(190, 40, 88, 150);

    void Start()
    {
        isPointerIn = false;
        resumeImg = gameObject.GetComponent<Image>();
        resumeImg.color = clearCol;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Resume"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Resume"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
        if (isPointerIn)
        {
            resumeImg.color = transCol;
        }
        else
        {
            resumeImg.color = clearCol;
        }
    }
}
