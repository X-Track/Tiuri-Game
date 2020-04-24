using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperCounterUI : MonoBehaviour
{
    private GameObject paperCounterUIElement;
    public int realPaperCounter;
    private Text counterText;


    void Start()
    {
        paperCounterUIElement = GameObject.Find("PaperCounter");
        counterText = paperCounterUIElement.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (realPaperCounter == 0)
        {
            counterText.text = "Letter Collected 0 / 1";
        }
        if (realPaperCounter == 1)
        {
            counterText.text = "Letter Collected 1 / 1";
        }
    }

    public void PaperCounterPlusPlus()
    {
        realPaperCounter = realPaperCounter +1;
    }
}
