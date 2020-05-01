using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLightMessage : MonoBehaviour
{

    public GameObject lightMessage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(lightMessage);
        }
    }
}
