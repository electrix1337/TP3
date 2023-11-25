using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FearComponent : MonoBehaviour
{
    [SerializeField] float fearMaxTime = 5;
    [SerializeField] float lightRange = 8;
    [SerializeField] float regenSpeed = 1f;
    [SerializeField] float depletionTime = 0.7f;
    [SerializeField] GameObject FearBar;
    [SerializeField] GameObject textObject;
    [SerializeField] GameObject lightFolder;
    [SerializeField] GameObject jumpscare;
    TextMeshProUGUI text;
    float timeLeft;
    List<(Vector3, lightScript)> lightsInfo = new List<(Vector3, lightScript)>();
    public bool inLight
    {
        get;
        set;
    } = false;

    private void Start()
    {
        timeLeft = fearMaxTime;
        text = textObject.GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < lightFolder.transform.childCount; i++)
        {
            Transform lightChild = lightFolder.transform.GetChild(i);
            lightsInfo.Add((lightChild.position, lightChild.GetComponent<lightScript>()));
        }
    }
    void ShowFear()
    {
        float stamina = timeLeft / fearMaxTime * 100;
        text.text = ((int)stamina).ToString() + "/100";
        FearBar.transform.localPosition = new Vector3((stamina - 100) / 2, 0, 0);
        FearBar.transform.localScale = new Vector3(timeLeft / fearMaxTime, 1, 1);
    }
    void LightInRange()
    {
        inLight = false;
        for (int i = 0; i < lightsInfo.Count; ++i)
        {
            /*
             check if the light we are checking is active and if the player is in range of the light
             */
            if (lightsInfo[i].Item2.active && Vector3.Magnitude(lightsInfo[i].Item1 - gameObject.transform.position) < lightRange)
            {
                inLight = true;
            }
        }
    }
    private void Update()
    {
        float time = Time.deltaTime;
        LightInRange();
        if (inLight)//check if the player is in light
        {
            timeLeft += regenSpeed * time;
            if (timeLeft > fearMaxTime)
            {
                timeLeft = fearMaxTime;
            }
        }
        else if (timeLeft != 0)//deplete fear bar 
        {
            if (timeLeft - time * depletionTime >= 0)//check if the time left for the fear bar is over 0
                timeLeft -= time * depletionTime;
            else
            {
                timeLeft = 0;
                StartCoroutine(ShowJumpscare(jumpscare));
                Game.GameOver();
            }
        }
        ShowFear();
    }
    public static IEnumerator ShowJumpscare(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(2);
        obj.SetActive(false);
    }
}
