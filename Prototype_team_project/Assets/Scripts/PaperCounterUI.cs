using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperCounterUI : MonoBehaviour
{
    private GameObject paperCounterUIElement;
    public int realPaperCounter;
    private Image counterText;
    public Sprite[] sprites;


    void Start()
    {
        paperCounterUIElement = GameObject.Find("PaperCounter");
        counterText = paperCounterUIElement.GetComponent<Image>();
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
            counterText.sprite = sprites[3];
        }
    }

    public void PaperCounterPlusPlus()
    {
        realPaperCounter = realPaperCounter +1;
    }
}
