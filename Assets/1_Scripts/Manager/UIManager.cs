using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image KnightCD, ArcherCD, AssassinCD, HealthFill, ShieldFill;
    public TMP_Text KnightText, ArcherText, AssassinText, MaxHealthText, CurrentHealthText, MaxShieldText, CurrentShieldText;

    PlayerCore playerCore;
    public PlayerCore PlayerCore { get { return playerCore; } set { playerCore = value; } }

    private void Awake()
    {
        playerCore = FindObjectOfType<PlayerCore>();

        KnightText.gameObject.SetActive(false);
        ArcherText.gameObject.SetActive(false);
        AssassinText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerCore == null)
            return;

        if (playerCore.knightSkill)
        {
            KnightCD.fillAmount += 1.0f / playerCore.knightCDTime * Time.deltaTime;
            float countdown = playerCore.knightCDTime - playerCore.knightSkillTimer;
            KnightText.text = Mathf.RoundToInt(countdown).ToString();
        }

        if (playerCore.archerSkill)
        {
            ArcherCD.fillAmount += 1.0f / playerCore.archerCDTime * Time.deltaTime;
            float countdown = playerCore.archerCDTime - playerCore.archerSkillTimer;
            ArcherText.text = Mathf.RoundToInt(countdown).ToString();
        }

        if (playerCore.assassinSkill)
        {
            AssassinCD.fillAmount += 1.0f / playerCore.assassinCDTime * Time.deltaTime;
            float countdown = playerCore.assassinCDTime - playerCore.assassinSkillTimer;
            AssassinText.text = Mathf.RoundToInt(countdown).ToString();
        }

        if (playerCore.statusChanged)
        {
            playerCore.statusChanged = false;
            HealthFill.fillAmount = (float)playerCore.currentHealth / (float)playerCore.maxHealth;
            ShieldFill.fillAmount = (float)playerCore.currentShield / (float)playerCore.maxShield;
        }
    }
}
