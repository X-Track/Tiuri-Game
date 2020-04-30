using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLightArea : MonoBehaviour
{
    public Text text;
    

    private void OnTriggerEnter(Collider other)
    {
        text.text = "\n" + "This place is too dark" + "\n" + "I should grab some kind of light first.";
    }

    private void OnTriggerExit(Collider other)
    {
        text.text = "";
        Destroy(text, 5);
        Destroy(gameObject, 5);
    }
}
