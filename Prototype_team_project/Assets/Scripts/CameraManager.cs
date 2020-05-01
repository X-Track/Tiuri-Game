using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CanvasGroup canvasFade;
    private float duration = 0.5f;
    public int camera;
    public Camera[] cameras;

    private void Start()
    {
        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = false;
    }


    void Update()
    {
        StartCoroutine(ActivateSwaps(5));

        if (camera == 0)
        {
            cameras[0].enabled = true;
            cameras[1].enabled = false;
            cameras[2].enabled = false;
        }

        if (camera == 1)
        {
            cameras[0].enabled = false;
            cameras[1].enabled = true;
            cameras[2].enabled = false;
        }

        if (camera == 2)
        {
            cameras[0].enabled = false;
            cameras[1].enabled = false;
            cameras[2].enabled = true;
        }

        if (camera == 3)
        {
            camera = 0;
        }
    }

   
    private void SwitchCamera()
    {
        camera =+ camera;
    }


    IEnumerator ActivateSwaps(float time)
    {
        //StartCoroutine("CamSwap");
        SwitchCamera();
        yield return new WaitForSeconds(5);
    }


    IEnumerator CamSwap()
    {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            canvasFade.alpha = 0 + progress;
            yield return null;
        }
        
        SwitchCamera();
        
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            canvasFade.alpha = 1 - progress;
            yield return null;
        }
    }
}