using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public LineRenderer line;
    public float maxForce, forceModifier;

    private float force;
    private Rigidbody rb;

    private Vector3 startPos, endPos;
    private bool canshoot = false;
    private Vector3 direction;

    private void Awake()
    {
        line.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        if (canshoot)
        {
            canshoot = false;
            direction = startPos - endPos;
            rb.AddForce(direction * force, ForceMode.Impulse);
            force = 0.0f;
            startPos = endPos = Vector3.zero;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !canshoot)
        {
            startPos = Chickpoint();
            line.gameObject.SetActive(true);
            line.SetPosition(0, line.transform.localPosition);
        }
        if (Input.GetMouseButton(0))
        {
            endPos = Chickpoint();
            force = Mathf.Clamp(Vector3.Distance(endPos, startPos) * forceModifier, 0, maxForce);
            line.SetPosition(1, transform.InverseTransformPoint(endPos));
        }
        if (Input.GetMouseButtonUp(0))
        {
            canshoot = true;
            line.gameObject.SetActive(false);
        }
    }
    Vector3 Chickpoint()
    {
        Vector3 pos = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit, 20f))
        {
            pos = hit.point;
        }
        return pos;
    }
}
