using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public Image energyFill;
    public Text nameText;
    public Image characterImage;
    public Slider levelSlider;
    public Text levelText;

    private PlayerStats playerStats;
    private CombatAssets combatAssets;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.instance;
        combatAssets = CombatAssets.instance;
        nameText.text = playerStats.playerName;
        levelText.text = playerStats.level.ToString();
        levelSlider.maxValue = playerStats.GetPlayerMaxExpPoints();
        levelSlider.value = playerStats.GetPlayerExpPoints();
        characterImage.sprite = combatAssets.playerImage;
        slider.maxValue = playerStats.GetPlayerMaxEnergyPoints();
        slider.value = playerStats.GetPlayerEnergyPoints();
        //SetSliderColor(slider.value, slider.maxValue);
        playerStats.OnPlayerHealthPointsChange += UpdateEnergy;
        playerStats.OnPlayerExpPointsChange += UpdateExp;
    }

    public void UpdateEnergy()
    {
        //healthPointsText.text = Mathf.CeilToInt(playerStats.GetPlayerHealthPoints()) + "/" + Mathf.CeilToInt(playerStats.GetPlayerMaxHealthPoints());
        slider.maxValue = playerStats.GetPlayerMaxEnergyPoints();
        StartCoroutine(FillEnergySliderPlayer());
        //slider.value = playerStats.GetPlayerEnergyPoints();
        //SetSliderColor(slider.value, slider.maxValue); //<>
    }

    public IEnumerator FillEnergySliderPlayer()
    {
        float target = playerStats.GetPlayerEnergyPoints();
        float value = slider.value;
        float currentTime = 0f;
        while (currentTime < 1f)
        {
            slider.value = Mathf.Lerp(value, target, currentTime / 1f);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void UpdateExp()
    {
        levelText.text = playerStats.level.ToString();
        levelSlider.maxValue = playerStats.GetPlayerMaxExpPoints();
        levelSlider.value = playerStats.GetPlayerExpPoints();
    }

    /*public void SetSliderColor(float energyPoints, float maxEnergyPoints)
    {
        float result = energyPoints / maxEnergyPoints;
        if (result < 0.75f && result >= 0.5f)
        {
            energyFill.color = new Color32(250, 255, 0, 255);
        } 
        else if(result < 0.5f && result >= 0.25f)
        {
            energyFill.color = new Color32(255, 150, 0, 255);
        } 
        else if (result < 0.25)
        {
            energyFill.color = new Color32(255, 0, 0, 255);
        } 
        else
        {
            energyFill.color = new Color32(8, 255, 0, 255);
        }
    }*/
}