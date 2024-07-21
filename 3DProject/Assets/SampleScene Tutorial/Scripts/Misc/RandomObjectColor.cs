using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectColor : MonoBehaviour
{
    private void Awake()
    {
        // this will give the object a random color at startup and it only gets called once
        this.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0.0f, 1.0f, 0.75f, 1.0f, 0.5f, 1.0f);
    }
}
