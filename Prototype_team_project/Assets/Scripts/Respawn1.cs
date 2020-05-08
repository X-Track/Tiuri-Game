using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Respawn1 : MonoBehaviour
{
    static private bool firstLoading = true;
    private GameObject player;
    public GameObject Respawnpoint;
    private Vector3 RespawnPointlocation;
    private Animator fadeanimator;
    private Image blackImg;
    private AudioSource AudioSourcePlayer;
    public AudioClip GotCoughtYeet;
    private bool isTriggerd;

    private Animator musicanimator;

    private void Start()
    {
        musicanimator = GameObject.Find("BackgroundMusic").GetComponent<Animator>();
        AudioSourcePlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();
        blackImg = GameObject.Find("BlackImage").GetComponent<Image>();
        fadeanimator = GameObject.Find("BlackImage").GetComponent<Animator>();
        player = GameObject.Find("Player");
        RespawnPointlocation = Respawnpoint.transform.position;
        PlayerstartPos();
    }

    private void PlayerstartPos()
    {
        if (firstLoading)
        {
            SwitchValue();
        }

        else if (!firstLoading)
        {
            ResetPos();
        }
    }

    private void ResetPos()
    {
        player.transform.position = RespawnPointlocation;
    }

    private void SwitchValue()
    {
        firstLoading = false;
    }

    public void GotCought()
    {
        if (!isTriggerd)
        {
            StartCoroutine(Fading());
            AudioSourcePlayer.PlayOneShot(GotCoughtYeet);
            isTriggerd = true;
        }
        else
        {
            return;
        }

    }

    IEnumerator Fading()
    {
        musicanimator.SetBool("Music", true);
        fadeanimator.SetBool("Fade", true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
