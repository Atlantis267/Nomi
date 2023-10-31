using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject flag;
    Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < -20f)  //if player die 
        {
            gameObject.transform.position = spawnPoint; //respawn to spawnPoint
        }
    }

    private void OnTriggerEnter(Collider other)     //set spawnPoint
    {
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            spawnPoint = flag.transform.position;
            Destroy(flag);
        }
    }
}
