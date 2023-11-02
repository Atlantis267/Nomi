using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_NewGame : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    [SerializeField] private float movement = 25.0f;
    private CanvasGroup buttonTransition; //±±¨îUIµo¥ú
    private Vector3 startPosition;
    private Vector3 endPosition;
    //[SerializeField] private float time = 0.5f;
    [SerializeField] private float speed = 3.0f;
    //private float counter;
    void Start()
    {
        isPointerIn = false;
        //buttonTransition = button.GetComponent<CanvasGroup>();
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(movement, 0, 0);
    }
    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (eventData.pointerEnter.CompareTag("NewGame")) 
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("NewGame"))
        {
            isPointerIn = false;
        }
    }
    void Update()
    {
        //counter += Time.deltaTime;
        //float timesUp = counter / time;
        //isPointerIn = EventSystem.current.IsPointerOverGameObject();
        if (isPointerIn)
        {
            transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
