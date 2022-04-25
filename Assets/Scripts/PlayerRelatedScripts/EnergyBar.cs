using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public Image energyFill;
    public Text nameText;
    public Text levelText;
    public Image characterImage;

    private PlayerStats playerStats;
    private CombatAssets combatAssets;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.instance;
        combatAssets = CombatAssets.instance;
        nameText.text = playerStats.playerName;
        levelText.text = playerStats.level.ToString();
        characterImage.sprite = combatAssets.playerImage;
        slider.maxValue = playerStats.GetPlayerMaxEnergyPoints();
        slider.value = playerStats.GetPlayerEnergyPoints();
        SetSliderColor(slider.value, slider.maxValue);
        playerStats.OnPlayerHealthPointsChange += UpdateEnergy;
    }

    public void UpdateEnergy()
    {
        //healthPointsText.text = Mathf.CeilToInt(playerStats.GetPlayerHealthPoints()) + "/" + Mathf.CeilToInt(playerStats.GetPlayerMaxHealthPoints());
        slider.maxValue = playerStats.GetPlayerMaxEnergyPoints();
        slider.value = playerStats.GetPlayerEnergyPoints();
        SetSliderColor(slider.value, slider.maxValue); //<>
    }

    public void SetSliderColor(float energyPoints, float maxEnergyPoints)
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
    }
}