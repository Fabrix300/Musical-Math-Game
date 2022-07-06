using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RequestTimer : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Image circularUiFill;
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
        if (slider)
        {
            slider.maxValue = totalTime;
            slider.value = totalTime;
            slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
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
                    if (circularUiFill) circularUiFill.color = Color.green;
                    OnTimerEnd?.Invoke();
                }
                if (stopTimer == false)
                {
                    if(slider) slider.value = time;
                    if(circularUiFill) circularUiFill.fillAmount = time/20; FillAmountChanged(time);
                }
            }
        }
    }

    public void RunTimer()
    {
        if (slider)
        {
            slider.maxValue = totalTime;
            slider.value = totalTime;
        }
        /* NUEVO */ if (circularUiFill) circularUiFill.fillAmount = totalTime / totalTime;
        graceTimeCopy = graceTime;
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
        if (slider)
        {
            if (stopped)
            {
                return maxValueOfMultiplicator - (minValueOfMultiplicator - ((slider.value / totalTime) * minValueOfMultiplicator));
            }
            return 1f;
        }
        return maxValueOfMultiplicator;
    }

    public void ValueChangeCheck()
    {
        if(fill) fill.color = Color.Lerp(Color.red, Color.green, slider.value / 20);
    }

    public void FillAmountChanged(float time)
    {
        if(circularUiFill) circularUiFill.color = Color.Lerp(Color.red, Color.green, time / totalTime);
    }
}
