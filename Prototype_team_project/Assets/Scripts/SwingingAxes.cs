using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxes : MonoBehaviour
{
    public float speed = 2f;
    public float maxRotation = 45f;
    private bool isOn;

    private void Start()
    {
        isOn = true;
    }

    void Update()
    {
        if (isOn)
        {
            transform.rotation = Quaternion.Euler(0, 0f, maxRotation * Mathf.Sin(Time.time * speed));
        }
        if (!isOn)
        {
            transform.rotation = Quaternion.Euler(0, 0f, maxRotation * Mathf.Sin(Time.time * 0));
        }
    }

    public void BoolChanger(bool isAxeOn)
    {
        if (isAxeOn)
        {
            isOn = true;
        }

        if (!isAxeOn)
        {
            isOn = false;
        }
    }
}