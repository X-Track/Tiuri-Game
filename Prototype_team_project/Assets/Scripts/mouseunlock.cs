using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseunlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShowCursor();
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
