using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RequestTimer : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Image uiFill;
    public float totalTime;
    public float graceTime;
    public float maxValueOfMultiplicator;
    public float minValueOfMultiplicator;
    public bool stopped = true;

    public event Action OnTimerEnd;

    private bool stopTimer;
    private float actualTime;
    private float graceTimeCopy;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = totalTime;
        slider.value = totalTime;
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            graceTimeCopy -= Time.deltaTime;
            if (graceTimeCopy < -graceTime)
            {
                float time = totalTime - Mathf.Abs(actualTime + 2*graceTime - Time.time);
                if (time <= 0f)
                {
                    stopTimer = true;
                    Debug.Log("Time ended");
                    OnTimerEnd?.Invoke();
                }
                if (stopTimer == false)
                {
                    slider.value = time;
                    if(uiFill) uiFill.fillAmount = time/20; FillAmountChanged(time);
                }
            }
        }
    }

    public void RunTimer()
    {
        slider.maxValue = totalTime;
        slider.value = totalTime;
        graceTimeCopy = graceTime;
        /* NUEVO */ uiFill.fillAmount = totalTime / totalTime;
        actualTime = Time.time;
        stopTimer = false;
        stopped = false;
    }

    public void StopTimer()
    {
        stopped = true;
    }

    public float GetTimerBonusMultiplicator()
    {
        if (stopped)
        {
            return maxValueOfMultiplicator - (minValueOfMultiplicator - ((slider.value / totalTime) * minValueOfMultiplicator));
        }
        return 1f;
    }

    public void ValueChangeCheck()
    {
        fill.color = Color.Lerp(Color.red, Color.green, slider.value / 20);
    }

    public void FillAmountChanged(float time)
    {
        uiFill.color = Color.Lerp(Color.red, Color.green, time / totalTime);
    }
}
