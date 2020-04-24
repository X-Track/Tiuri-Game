using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPaperScript : MonoBehaviour
{
    public int papercounter = 0;
    private GameObject papercounterui;
    public GameObject pickupParticals;
    private PaperCounterUI PaperCounterUIscript;
    private bool isInRange;

    void Start()
    {
        papercounterui = GameObject.Find("FPEInteractionManager");
        PaperCounterUIscript = papercounterui.GetComponent<PaperCounterUI>();
        isInRange = false;
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            GivePapersToMe();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }

    public void GivePapersToMe()
    {
        PaperCounterUIscript.PaperCounterPlusPlus();
        Instantiate(pickupParticals, this.gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
