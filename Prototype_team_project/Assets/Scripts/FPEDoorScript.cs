using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FPEDoorScript : MonoBehaviour
{
    private Animator animator;
    private Text UIText;
    private GameObject InteractionTextLabel;
    public bool isOpen;
    private bool canInteract;


    private AudioSource AudioSourcePlayer;
    public AudioClip playDoorOpenSound;
    public AudioClip playDoorCloseSound;


    void Start()
    {
        AudioSourcePlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        InteractionTextLabel = GameObject.Find("InteractionTextLabel");
        UIText = InteractionTextLabel.GetComponent<Text>();
        isOpen = false;
        canInteract = false;
        AudioSourcePlayer.volume = 0;
        StartCoroutine(Turnonvolume());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HAZZAAAA");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
        UIText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            isOpen = !isOpen;
            animator.SetBool("Open", isOpen);
        }
    }


    public void PlayDoorOpenSound()
    {
        AudioSourcePlayer.PlayOneShot(playDoorOpenSound);
    }

    public void PlayDoorCloseSound()
    {
        AudioSourcePlayer.PlayOneShot(playDoorCloseSound);
    }

    IEnumerator Turnonvolume()
    {
        yield return new WaitForSeconds(5);
        AudioSourcePlayer.volume = 0.7f;
    }
}
