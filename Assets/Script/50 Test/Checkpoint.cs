using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public List<GameObject> CheckPoint;

    public Vector3 vectorPoint;

    // Start is called before the first frame update
    void Start()
    {
        vectorPoint = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 4.2f)
        {
            //transform.position = Vector3.zero; //respawn to spawnPoint

            //Debug.Log("die3");
        }

    }

    private void OnTriggerEnter(Collider other)     //set spawnPoint
    {
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            vectorPoint = transform.position;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Water"))
        {
            transform.position = vectorPoint; //respawn to spawnPoint
            Debug.Log("die");
        }
        if (other.gameObject.CompareTag("EndPoint"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (other.gameObject.CompareTag("EndPoint2"))
        {
            SceneManager.LoadScene("Test_UI");
        }

    }
}
