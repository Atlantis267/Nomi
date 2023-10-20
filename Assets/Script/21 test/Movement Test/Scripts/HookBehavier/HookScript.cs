using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookScript : MonoBehaviour
{
    public string[] tagsToCheck;
    //Force applied to nova bomb upon spawn
    public float speed, returnSpeed;
    public float range, stopRange;



    //Private variables
    [HideInInspector]
    public Transform caster, collidedWith;
    private bool hasCollided;
    private SpringJoint joint;
    private ControlleHook pm;

    private void Start()
    {
        pm = GetComponent<ControlleHook>();
        
    }
    private void Update()
    {
        if (caster)
        {
            StartGrapple();
            //Check if we have impacted
            ReturnGrapple();
            if (collidedWith) 
            {
                collidedWith.transform.position = transform.position; 
            }
        }else 
        { 
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Here add as many checks as you want for your nova bomb's collision
        if (!hasCollided && tagsToCheck.Contains(other.gameObject.tag))
        {
            Collision(other.transform);
        }
        else if(!hasCollided && other.gameObject.tag == "Envirment")
        {
            Collision(null);
        }
    }

    void Collision(Transform col)
    {
        speed = returnSpeed;
        //Stop movement
        hasCollided = true;
        if (col)
        {
            transform.position = col.position;
            collidedWith = col;
        }
    }
    
    void StartGrapple()
    {
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = caster.position;
        float distanceFromPoint = Vector3.Distance(caster.position, transform.position);

        joint.maxDistance = distanceFromPoint * 0.0f;
        joint.minDistance = distanceFromPoint * 0.0f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void ReturnGrapple()
    {
        if (hasCollided)
        {
            transform.LookAt(caster);
            var dist = Vector3.Distance(transform.position, caster.position);
            if (dist < stopRange)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            var dist = Vector3.Distance(transform.position, caster.position);
            if (dist > range)
            {
                Collision(null);
            }
        }
    }
    public bool IsGrappling()
    {
        return joint != null;
    }
}
