using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textInterrupteur : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform); // Le text affichera toujours vers la caméra
    }
}
