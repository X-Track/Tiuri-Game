using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dialogScriptend : MonoBehaviour
{
    private bool isInRange;
    private int textDialog;


    [Header("Checks")]
    public Text textMesh;
    public Text textTiuri;
    public Text textKnight;
    public Text textTalk;

    void Start()
    {
        // set is in range by the start on false
        isInRange = false;

        //default text is none
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
            textTalk.text = "F to Give Papers";
        }
        if (textDialog == 1 && isInRange)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
} 