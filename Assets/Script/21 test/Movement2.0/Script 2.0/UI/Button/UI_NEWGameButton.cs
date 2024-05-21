using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NEWGameButton : MonoBehaviour
{
    [SerializeField] private float movement = 100.0f;
    [SerializeField] private float speed = 3.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(movement, 0, 0);
    }

    public void Move()
    {
        Debug.Log("point");
        transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * speed);
    }

    public void MoveBack()
    {
        transform.position = startPosition;
    }
}
