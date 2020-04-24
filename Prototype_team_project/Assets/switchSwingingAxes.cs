using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchSwingingAxes : MonoBehaviour
{

    private bool isInRange;
    private bool isAxeOn;
    public GameObject axe1;
    public GameObject axe2;
    public GameObject barrier;
    public GameObject electric;
    private SwingingAxes axeScript1;
    private SwingingAxes axeScript2;

    void Start()
    {
        axeScript1 = axe1.GetComponent<SwingingAxes>();
        axeScript2 = axe2.GetComponent<SwingingAxes>();

        isInRange = false;
        isAxeOn = false;
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F) && isAxeOn)
        {
            Debug.Log("HAZA Aan");
            SwitchAxes(isAxeOn);
            StartCoroutine (SwitchAxesOff(2));
        }

        if (isInRange && Input.GetKeyDown(KeyCode.F) && !isAxeOn)
        {
            Debug.Log("HAZA uit");
            SwitchAxes(isAxeOn);
            StartCoroutine(SwitchAxesOff(2));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = true;
            Debug.Log("HAZA IN");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HAZA INUIT");
            isInRange = false;
        }
    }

    public void SwitchAxes(bool isOn)
    {
        if (isOn)
        {
            axeScript1.BoolChanger(isOn);
            axeScript2.BoolChanger(isOn);
            barrier.SetActive(true);
            electric.SetActive(true);
        }

        if (!isOn)
        {
            axeScript1.BoolChanger(isOn);
            axeScript2.BoolChanger(isOn);
            barrier.SetActive(false);
            electric.SetActive(false);

        }
    }

    IEnumerator SwitchAxesOff(float time)
    {
        yield return new WaitForSeconds(time);
        isAxeOn = !isAxeOn;
    }
}
