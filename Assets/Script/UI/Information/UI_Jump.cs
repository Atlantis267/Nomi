using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Jump : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    [SerializeField] private float movement = 30.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private GameObject jumpImg;
    void Start()
    {
        isPointerIn = false;
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(movement, 0, 0);
        jumpImg.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Jump"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Jump"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
        if (isPointerIn)
        {
            transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * speed);
            jumpImg.SetActive(true);
        }
        else
        {
            transform.position = startPosition;
            jumpImg.SetActive(false);
        }
    }
}
