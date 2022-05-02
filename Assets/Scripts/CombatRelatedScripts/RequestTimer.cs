using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RequestTimer : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public float totalTime;
    public float graceTime;
    public bool stopped = true;

    public event Action OnTimerEnd;

    private bool stopTimer;
    private float actualTime;

    // Start is called before the first frame update
    void Start()
    { 
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            if (Mathf.Abs(actualTime - Time.time) >= graceTime)
            {
                float time = totalTime - Mathf.Abs(actualTime + graceTime - Time.time);
                if (time <= 0f)
                {
                    stopTimer = true;
                    OnTimerEnd?.Invoke();
                }
                if (stopTimer == false)
                {
                    slider.value = time;
                }
            }
        }
    }

    public void RunTimer()
    {
        stopped = false;
        slider.maxValue = totalTime;
        slider.value = totalTime;
        stopTimer = false;
        actualTime = Time.time;
    }

    public void StopTimer()
    {
        stopped = true;
    }

    public float GetTimerBonusMultiplicator()
    {
        if (stopped)
        {
            return 2.1f - (1.1f - ((slider.value / totalTime) * 1.1f));
        }
        return 1f;
    }

    public void ValueChangeCheck()
    {
        fill.color = Color.Lerp(Color.red, Color.green, slider.value / 20);
    }
}
