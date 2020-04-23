using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLightArea : MonoBehaviour
{

    public GameObject Player;
    public Text text;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        text.text = "\n" + "This place is to dark" + "\n" + "I should grab some kind of light first.";
    }

    private void OnTriggerExit(Collider other)
    {
        text.text = "";
        Destroy(text, 5);
        Destroy(gameObject, 5);
    }
}
