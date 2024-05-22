using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeTextLeft;
    public Text timeTextRight;
    public float time;
    public float second;
    public int minute;
    public bool timeStop;

    void Start()
    {
        timeStop = false;
        minute = 0;
        second = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            time += 10;
        if (Input.GetKeyDown(KeyCode.V))
            time += 294;
        if (!timeStop)
            time += Time.deltaTime;

        minute = (int)time / 60;
        second = (time - (minute * 60)) % 60;

        timeTextLeft.text = minute.ToString("00");
        timeTextRight.text = second.ToString("00");
    }
}
