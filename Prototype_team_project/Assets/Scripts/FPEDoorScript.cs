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

    void Start()
    {
       animator = GetComponent<Animator>();
       InteractionTextLabel = GameObject.Find("InteractionTextLabel");
       UIText = InteractionTextLabel.GetComponent<Text>();
       isOpen = false;
       canInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
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
        if (Input.GetKeyDown(KeyCode.F)&& canInteract)
        {
            isOpen = !isOpen;
            animator.SetBool("Open", isOpen);
        }
    }
}
