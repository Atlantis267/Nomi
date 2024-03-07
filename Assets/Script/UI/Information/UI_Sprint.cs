using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Sprint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isPointerIn;
    [SerializeField] private float movement = 30.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private GameObject sprintImg;
    void Start()
    {
        isPointerIn = false;
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(movement, 0, 0);
        sprintImg.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Sprint"))
        {
            isPointerIn = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.CompareTag("Sprint"))
        {
            isPointerIn = false;
        }
    }

    void Update()
    {
        if (isPointerIn)
        {
            transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * speed);
            sprintImg.SetActive(true);
        }
        else
        {
            transform.position = startPosition;
            sprintImg.SetActive(false);
        }
    }
}
