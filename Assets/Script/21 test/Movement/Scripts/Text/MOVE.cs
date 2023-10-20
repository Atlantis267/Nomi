using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE : MonoBehaviour
{
    public CharacterController characterController { get; private set; }
    float speed = 5f;
    float Jump = 20f;
    Vector3 move;
    Vector3 rotate;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var vInput = Input.GetAxis("Vertical");
        var hInput = Input.GetAxis("Horizontal");

        if (characterController.isGrounded)
        {
            move = new Vector3 (vInput * speed, move.y, hInput * speed);
            rotate = transform.up * speed * hInput;
            if (Input.GetKey(KeyCode.Space))
            {
                move.y = Jump;
            }
        }
        move.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(move * Time.deltaTime);
        transform.Rotate(rotate * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sphere")
        {
            Vector3 difference = other.transform.position - transform.position;
            float dist = difference.magnitude;
            Vector3 gravityDirection = difference.normalized;
            float gravity = 6.7f * (transform.localScale.x * other.transform.localScale.x * 60) / (dist * dist);
            Vector3 gravityVector = (other.transform.position * gravity);
            characterController.Move(gravityVector * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        characterController.enabled = true;
    }
}
