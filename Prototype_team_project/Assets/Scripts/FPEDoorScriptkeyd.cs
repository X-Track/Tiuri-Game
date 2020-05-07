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
    private GameObject keyImage;
    static public bool keyPickedup;


    void Start()
    {
       animator = GetComponent<Animator>();
       keyImage = GameObject.Find("keyImage");
       isOpen = false;
       inRange = false;
    }

    public void PickedupKey()
    {
        keyPickedup = true;
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
            TextLabel.text = "Used The Key To Unlock The Door";
        }

        if (inRange && Input.GetKeyDown(KeyCode.F) && !keyPickedup)
        {
            TextLabel.text = "Need a Key To UnLock";
        } 

        if (keyPickedup)
        {
            keyImage.SetActive(true);
        }
        if (!keyPickedup)
        {
            keyImage.SetActive(false);
        }
    }


    private void UnlockDoor()
    {
        keyPickedup = false;
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
