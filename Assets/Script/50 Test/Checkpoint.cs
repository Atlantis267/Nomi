using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] List<GameObject> CheckPoint;

    [SerializeField] Vector3 vectorPoint;

    // Start is called before the first frame update
    void Start()
    {
        vectorPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)     //set spawnPoint
    {
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            vectorPoint = player.transform.position;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Water"))
        {
            player.transform.position = vectorPoint; //respawn to spawnPoint
        }
    }
}
