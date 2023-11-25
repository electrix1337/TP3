using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameTime : MonoBehaviour
{
   TextMeshProUGUI text;
   int minutes = 0;
   int seconds = 0;
   float millis = 0f;

    void Start()
    {
        text = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        millis += Time.deltaTime;

        if(millis>=1)
        {
            int overflowMillis = (int)Mathf.Floor(millis);

            seconds += overflowMillis;
            millis -= overflowMillis;

            if(seconds>=60)
            {
                seconds -= 60;
                minutes++;
            }
        }
        text.text = Format(minutes) + ":" + Format(seconds) + ":" + Format(Mathf.Floor(millis * 100));

        string Format(float value)
        {
            string result = value.ToString();
            if(result.Length==1)
            {
                return "0" + result;
            }
            return result;
        }

    }
    public void EndGame()
    {
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text += minutes + "m, " + seconds + "s, " + Mathf.Floor(millis * 100) + "ms";
    }
}
