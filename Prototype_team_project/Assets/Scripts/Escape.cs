using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Escape : MonoBehaviour
{
    private bool inRange;
    private Vector3 telePosition;
    public GameObject Player;
    public GameObject telepoint;

    private void Start()
    {
        telePosition = telepoint.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inRange)
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        Player.transform.position = telePosition;
    }
}
