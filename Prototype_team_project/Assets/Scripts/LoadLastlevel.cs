using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLastlevel : MonoBehaviour
{

    static private bool gotallpapers;
    private Text uitext;

    private void Start()
    {
        uitext = GameObject.Find("TextLabel").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gotallpapers)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (other.gameObject.tag == "Player" && !gotallpapers)
        {
            uitext.text = "I have to find all 3 pieces first.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uitext.text = "";
        }
    }


    public void ChangeBool()
    {
        gotallpapers = true;
    }
}
