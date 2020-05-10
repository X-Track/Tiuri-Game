using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sethidecursor();
    }

    public void Sethidecursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
