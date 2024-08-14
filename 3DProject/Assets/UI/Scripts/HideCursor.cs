using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverUI.isActiveAndEnabled)
        {
            if (Cursor.lockState.Equals(CursorLockMode.Locked))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState.Equals(CursorLockMode.None))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
