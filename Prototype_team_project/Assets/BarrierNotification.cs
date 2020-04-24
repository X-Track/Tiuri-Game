using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarrierNotification : MonoBehaviour
{
    public Text text;

    private void OnTriggerEnter(Collider other)
    {
        text.text = "\n" + "This looks too dangerous" + "\n" + "I should turn the axes off first.";
    }

    private void OnTriggerExit(Collider other)
    {
        text.text = "";
    }
}
