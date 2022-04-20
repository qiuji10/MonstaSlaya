using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingCharacter : MonoBehaviour
{
    GameSceneManager gsm;
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameStats cs;
    [SerializeField] GameObject knight, archer, assassin;

    private void Awake()
    {
        gsm = FindObjectOfType<GameSceneManager>();
    }

    private void Start()
    {
        timerText.text = "Time used\n" + cs.time;

        knight.SetActive(false);
        archer.SetActive(false);
        assassin.SetActive(false);

        if (cs.state == PlayerCore.Character.KNIGHT)
        {
            knight.SetActive(true);
        }
        else if (cs.state == PlayerCore.Character.ARCHER)
        {
            archer.SetActive(true);
        }
        else if (cs.state == PlayerCore.Character.ASSASSIN)
        {
            assassin.SetActive(true);
        }

        if (gsm.GetSceneIndex() == 3)
        {
            if (PlayerPrefs.GetInt("HaveHighscore") == 0)
            {
                PlayerPrefs.SetInt("HaveHighscore", 1);
                SetHighScore();
            }
            else if (PlayerPrefs.GetInt("HaveHighscore") == 1)
            {
                if (cs.minutes < PlayerPrefs.GetFloat("minutes"))
                {
                    SetHighScore();
                }
                else if (cs.minutes == PlayerPrefs.GetFloat("minutes"))
                {
                    if (cs.seconds < PlayerPrefs.GetFloat("seconds"))
                    {
                        SetHighScore();
                    }
                    else if (cs.seconds == PlayerPrefs.GetFloat("seconds"))
                    {
                        if (cs.milliseconds < PlayerPrefs.GetFloat("milliseconds"))
                        {
                            SetHighScore();
                        }
                    }
                }
            } 
        }
    }

    public void SetHighScore()
    {
        PlayerPrefs.SetFloat("milliseconds", cs.milliseconds);
        PlayerPrefs.SetFloat("seconds", cs.seconds);
        PlayerPrefs.SetFloat("minutes", cs.minutes);
    }
}
