using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueMovingScript : MonoBehaviour
{
    public Animator statueAnimator;
    private bool isInRange;


    void Start()
    {
        isInRange = false;
        statueAnimator.SetBool("isActivating", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            statueAnimator.SetBool("isActivating", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = true;
            Debug.Log("HAZA IN");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HAZA INUIT");
            isInRange = false;
        }
    }
}
