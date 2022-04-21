using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    TMP_Text highscoreTimer;

    private void Awake()
    {
        highscoreTimer = GetComponent<TextMeshProUGUI>();    
    }

    void Start()
    {
        highscoreTimer.text = "Best Record:\n" + ((int)PlayerPrefs.GetFloat("minutes")).ToString() + "m " + ((int)PlayerPrefs.GetFloat("seconds")).ToString() + "s " + ((int)PlayerPrefs.GetFloat("milliseconds")).ToString() + "ms";
    }
}