using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PauseQuit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    private Image quitImg;
    private Color clearCol;
    private Color transCol = new Color32(190, 40, 88, 150);

    void Start()
    {
        isPointerIn = false;
        quitImg = gameObject.GetComponent<Image>();
        quitImg.color = clearCol;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Pause_Quit"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Pause_Quit"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
        if (isPointerIn)
        {
            quitImg.color = transCol;
        }
        else
        {
            quitImg.color = clearCol;
        }
    }
}
