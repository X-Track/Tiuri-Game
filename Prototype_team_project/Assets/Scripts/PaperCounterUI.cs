using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperCounterUI : MonoBehaviour
{
    private GameObject paperCounterUIElement;
    static public int realPaperCounter;
    private Image counterText;
    public Sprite[] sprites;
    private LoadLastlevel lastlevelscript;
    private Text uitext;


    void Start()
    {
        uitext = GameObject.Find("TextLabel").GetComponent<Text>();
        paperCounterUIElement = GameObject.Find("PaperCounter");
        counterText = paperCounterUIElement.GetComponent<Image>();
        lastlevelscript = GameObject.Find("LastlevelTeleport").GetComponent<LoadLastlevel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (realPaperCounter == 0)
        {
            counterText.sprite = sprites[0];
        }
        if (realPaperCounter == 1)
        {
            counterText.sprite = sprites[1];
        }
        if (realPaperCounter == 2)
        {
            counterText.sprite = sprites[2];
        }
        if (realPaperCounter == 3)
        {
            uitext.text = "Now I can escape trought the tunnels";
            StartCoroutine(RemoveTheText(5));
            lastlevelscript.ChangeBool();
            counterText.sprite = sprites[3];
        }
        if (realPaperCounter == 4)
        {
            counterText.sprite = sprites[3];
        }
    }

    public void PaperCounterPlusPlus()
    {
        realPaperCounter = realPaperCounter +1;
    }

    IEnumerator RemoveTheText(float time)
    {
        yield return new WaitForSeconds(time);
        realPaperCounter = realPaperCounter + 1;
        uitext.text = "";
    }
}
