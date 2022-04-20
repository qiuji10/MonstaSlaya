using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float milliseconds, seconds, minutes;

    public TMP_Text timerText;
    [SerializeField] GameStats save;

    private void Update()
    {
        minutes = (int)(Time.timeSinceLevelLoad / 60f) % 60;
        seconds = (int)(Time.timeSinceLevelLoad % 60f);
        milliseconds = (int)(Time.timeSinceLevelLoad * 1000f) % 1000;

        timerText.text = string.Format("{0}:{1}:{2}", minutes, seconds, (int)milliseconds);
    }
}
