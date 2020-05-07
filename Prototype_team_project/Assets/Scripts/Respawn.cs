using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    static private bool firstLoading = true;
    private GameObject player;
    public GameObject Respawnpoint;
    private Vector3 RespawnPointlocation;


    private void Start()
    {
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
}
