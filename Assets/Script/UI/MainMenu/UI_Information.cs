using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Information: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    [SerializeField] private float movement = 25.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float speed = 3.0f;
    void Start()
    {
        isPointerIn = false;
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(movement, 0, 0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Setting"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Setting"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
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
