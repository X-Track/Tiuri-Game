using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyobjectaftersecondsad : MonoBehaviour
{
    public GameObject Thisgameobject;
    public float thistime;


    private void Update()
    {
        Destroy(Thisgameobject, thistime);
    }
}
