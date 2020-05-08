using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyForDoor : MonoBehaviour
{
    public GameObject interactableDoor;
    private FPEDoorScriptkeyd DoorScript;
    private bool inRange;
    public Text text;
    public GameObject key;
    private AudioSource AudioSourcePlayer;
    public AudioClip pickupkeyaudio;

    static private bool pickedUp;

    private void Start()
    {
        AudioSourcePlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();
        DoorScript = interactableDoor.GetComponent<FPEDoorScriptkeyd>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (pickedUp)
            {
                inRange = true;
                text.text = "";
            }
            if (!pickedUp)
            {
                inRange = true;
                text.text = "F to Pickup Key";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && pickedUp)
        {
            inRange = false;
            text.text = "";
        }

    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKey();
        }
    }

    private void PickUpKey()
    {
        pickedUp = true;
        PlayPickupSound();
        key.SetActive(false);
        DoorScript.PickedupKey();
    }

    private void PlayPickupSound()
    {
        if (pickedUp)
        {
            AudioSourcePlayer.PlayOneShot(pickupkeyaudio);
        }
        else
        {
            return;
        }
    }


}
