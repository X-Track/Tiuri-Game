using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogScript : MonoBehaviour
{
    private bool isInRange;
    private int textDialog;


    [Header("Checks")]
    public Text textMesh;


    void Start()
    {
        // set is in range by the start on false
        isInRange = false;

        //default text is none
        textMesh.text = "";

        //dialog start is 0
        textDialog = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            textMesh.text = "";
            textDialog = 0;
        }
    }



    private void Update()
    {

        if (Input.GetKeyDown("f") && isInRange)
        {
            textDialog = textDialog + 1;
        }
        if (textDialog == 0 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: HEY YOU! You must help me!" + "\n" + "F to talk";
        }
        if (textDialog == 1 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: The black magician, Prince Viridian, stole a letter from my king... I was there myself on this terrible day. This letter must be delivered to Dagonaut!";
        }
        if (textDialog == 2 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: The black magician tore the letter into 3 parts and hid it in his castle not far from here! Your job is to find these 3 parts and bring them back to me.";
        }
        if (textDialog == 3 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: But be carefull! Some people say there are ghostly knights under command of Prins Viridian… They wander the castle… looking for lost souls!";
        }
        if (textDialog == 4 && isInRange)
        {
            textMesh.text = "\n" + "Tiuri: I… I don’t know… I failed to become a knight… So I don’t think you can rely on me fulfilling this mission…";
        }
        if (textDialog == 5 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: Kid... You are the only chance to save our kingdoms. A great war is at stake!";
        }
        if (textDialog == 6 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: See this mission as your final test to become a knight… If you pass this I will personally speak to your king and I will demand that you become a knight!";
        }
        if (textDialog == 7 && isInRange)
        {
            textMesh.text = "\n" + "Tiuri: This might be my only chance to become a knight… If you promise this, I will accept your mission.";
        }
        if (textDialog == 8 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: I like to hear that ... Next to me is a book with instructions ... Read it carefully, because this might save your life. ";
        }
        if (textDialog == 9 && isInRange)
        {
            textMesh.text = "\n" + "Ridder: After you are done with the instructions go to the right, there is a secret entrance leading straight to the castle. Good luck kid!";
        }
        if (textDialog == 10 && isInRange)
        {
            textMesh.text = "\n" + "";
        }
    }
}