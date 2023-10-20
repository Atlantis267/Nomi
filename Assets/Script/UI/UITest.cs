using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UITest : MonoBehaviour
{
    public Button button;
    [SerializeField] private bool isMouesEnter;
    private CanvasGroup buttonTransition;
    void Start()
    {
        isMouesEnter = false;
        buttonTransition = button.GetComponent<CanvasGroup>();
    }
    void Update()
    {
        isMouesEnter = EventSystem.current.IsPointerOverGameObject();
        if (isMouesEnter) 
        {
            Debug.Log("12345");
        }
    }
}
