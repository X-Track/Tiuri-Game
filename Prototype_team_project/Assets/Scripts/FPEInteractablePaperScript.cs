using UnityEngine;
using System.Collections;

public class FPEInteractablePaperScript : FPEInteractableBaseScript
{
    public int papercounter = 0;
    private GameObject papercounterui;
    public GameObject pickupParticals;
    private PaperCounterUI PaperCounterUIscript;



public override void Awake()
        {
            base.Awake();
            interactionType = eInteractionType.PAPERS;

        papercounterui = GameObject.Find("FPEInteractionManager");
        PaperCounterUIscript = papercounterui.GetComponent<PaperCounterUI>();
    }

    public void GivePapersToMe()
    {
        PaperCounterUIscript.PaperCounterPlusPlus();
        Instantiate(pickupParticals, this.gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
