using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FPEDoorScriptkeyd : MonoBehaviour
{
    private Animator animator;
    public bool isOpen;
    private bool inRange;


    public Text TextLabel;
    public GameObject keyImage;
    public bool keyPickedup;


    void Start()
    {
       animator = GetComponent<Animator>();
       isOpen = false;
       inRange = false;
       keyPickedup = false;
       keyImage.SetActive(false);
    }

    public void PickedupKey()
    {
        keyPickedup = true;
        keyImage.SetActive(true);
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
            TextLabel.text = "";
        }
    }


    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F) && keyPickedup)
        {
            UnlockDoor();
        }

        if (inRange && Input.GetKeyDown(KeyCode.F) && !keyPickedup)
        {
            TextLabel.text = "Need a Key To UnLock";
        }
    }


    private void UnlockDoor()
    {
        keyImage.SetActive(false);
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }




    //IEnumerator UsedKey (float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    StartCoroutine(TextDisable(1));
    //    TextLabel.text = "UNLOCKED";
    //    keyd = !keyd;
    //}

    //IEnumerator TextDisable(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    TextLabel.text = "";
    //}
}
