using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightScript : MonoBehaviour, ITriggerAction
{
    [SerializeField] GameObject spotLight;
    public bool active { get; private set; }

    private void Start()
    {
        active = spotLight.activeInHierarchy;
    }
    public void Trigger()
    {
        if (spotLight.activeInHierarchy)
        {
            spotLight.SetActive(false);
            active = false;
        }
        else
        {
            spotLight.SetActive(true);
            active = true;
        }
    }
}
