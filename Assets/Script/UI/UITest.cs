using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UITest : MonoBehaviour
{
    [SerializeField] private bool isPointerIn;
    [SerializeField] private float movement = 25.0f;
    private CanvasGroup buttonTransition; //控制UI發光
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
    void Update()
    {
        //counter += Time.deltaTime;
        //float timesUp = counter / time;
        isPointerIn = EventSystem.current.IsPointerOverGameObject();
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
