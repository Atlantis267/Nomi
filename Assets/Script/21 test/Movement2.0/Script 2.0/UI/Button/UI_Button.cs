using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] private float movement = 15f;
    [SerializeField] private float speed = 1000f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(movement, 0, 0);
    }

    public void Move()
    {        
        transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * speed);
    }

    public void MoveBack()
    {
        transform.position = startPosition;
    }
}
