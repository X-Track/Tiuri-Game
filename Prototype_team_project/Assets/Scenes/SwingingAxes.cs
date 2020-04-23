using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxes : MonoBehaviour
{
    public float speed = 2f;
    public float maxRotation = 45f;

    private void Start()
    {

    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0f, maxRotation * Mathf.Sin(Time.time * speed));
    }
}