using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = new Vector3(0,0,0);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 4.2f)
        {
            transform.position = Vector3.zero; //respawn to spawnPoint

            Debug.Log("die3");
        }
    }
}
