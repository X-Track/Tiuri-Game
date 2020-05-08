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
    public Text textTiuri;
    public Text textKnight;
    public Text textTalk;
    private AudioSource AudioSourcePlayer;


    public AudioClip riddertext1;
    public AudioClip riddertext2;
    public AudioClip riddertext3;
    public AudioClip riddertext4;
    public AudioClip riddertext5;
    public AudioClip riddertext6;
    public AudioClip riddertext7;
    public AudioClip riddertext8;

    public AudioClip tiuritext1;
    public AudioClip tiuritext2;

    void Start()
    {
        // set is in range by the start on false
        isInRange = false;

        AudioSourcePlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();
        textMesh.text = "";
        textTiuri.text = "";
        textKnight.text = "";

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
            textTiuri.text = "";
            textKnight.text = "";
            textTalk.text = "";
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
            AudioSourcePlayer.PlayOneShot(riddertext1);
            AudioSourcePlayer.volume = 0.8f;
            textDialog = textDialog + 1;
            
        }

        if (textDialog == 1 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "HEY YOU! You must help me!";

        }


        if (textDialog == 2 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.PlayOneShot(riddertext2);
            AudioSourcePlayer.volume = 0.7f;
            textDialog = textDialog + 1;
        }
        if (textDialog == 3 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "The black magician, Prince Viridian, stole a letter from my king... I was there myself on this terrible day. This letter must be delivered to Dagonaut!";
        }



        if (textDialog == 4 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.PlayOneShot(riddertext3);
            AudioSourcePlayer.volume = 0.7f;
            textDialog = textDialog + 1;
        }
        if (textDialog == 5 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "The black magician tore the letter into 3 parts and hid it in his castle not far from here! Your job is to find these 3 parts and bring them back to me.";

        }

        if (textDialog == 6 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.PlayOneShot(riddertext4);
            AudioSourcePlayer.volume = 0.7f;
            textDialog = textDialog + 1;
        }
        if (textDialog == 7 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "But be carefull! Some people say there are ghostly knights under command of Prince Viridian… They wander the castle… looking for lost souls!";

        }

        if (textDialog == 8 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.PlayOneShot(tiuritext1);
            AudioSourcePlayer.volume = 1f;
            textDialog = textDialog + 1;
        }
        if (textDialog == 9 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "";
            textTiuri.text = "Tiuri:";

            textMesh.text = "I…. I don’t know… I failed to become a knight… So I don’t think you can rely on me fulfilling this mission…";

        }

        if (textDialog == 10 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.volume = 0.7f;
            AudioSourcePlayer.PlayOneShot(riddertext5);
            textDialog = textDialog + 1;
        }
        if (textDialog == 11 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "Kid... You are the only chance to save our kingdoms. A great war is at stake!";

        }

        if (textDialog == 12 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.volume = 0.7f;
            AudioSourcePlayer.PlayOneShot(riddertext6);
            textDialog = textDialog + 1;
        }
        if (textDialog == 13 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "See this mission as your final test to become a knight… If you pass this I will personally speak to your king and I will demand that you become a knight!";

        }

        if (textDialog == 14 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.volume = 1f;
            AudioSourcePlayer.PlayOneShot(tiuritext2);
            textDialog = textDialog + 1;
        }
        if (textDialog == 15 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "";
            textTiuri.text = "Tiuri:";

            textMesh.text = "This might be my only chance to become a knight… If you promise this, I will accept your mission.";

        }

        if (textDialog == 16 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.volume = 0.7f;
            AudioSourcePlayer.PlayOneShot(riddertext7);
            textDialog = textDialog + 1;
        }
        if (textDialog == 17 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "I like to hear that ... Next to me is a book with instructions ... Read it carefully, because this might save your life. ";

        }

        if (textDialog == 18 && isInRange)
        {
            AudioSourcePlayer.Stop();
            AudioSourcePlayer.volume = 0.7f;
            AudioSourcePlayer.PlayOneShot(riddertext8);
            textDialog = textDialog + 1;
        }
        if (textDialog == 19 && isInRange)
        {

            textTalk.text = "F to Talk";

            textKnight.text = "Knight:";
            textTiuri.text = "";

            textMesh.text = "After you are done with the instructions go to the right, there is a secret entrance leading straight to the castle. Good luck kid!";

        }
        if (textDialog == 20 && isInRange)
        {

            textTalk.text = "";

            textKnight.text = "";
            textTiuri.text = "";

            textMesh.text = "";

        }
    }
}