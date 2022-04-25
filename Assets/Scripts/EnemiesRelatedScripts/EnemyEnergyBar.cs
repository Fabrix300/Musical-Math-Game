using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEnergyBar : MonoBehaviour
{
    public Slider slider;
    public Image energyFill;
    public Text nameText;
    public Text levelText;
    public Image enemyImage;

    private Enemy enemyComp;
    private CombatAssets combatAssets;

    public void SetValues()
    {
        if (enemyComp)
        {
            nameText.text = enemyComp.enemyName;
            levelText.text = enemyComp.level.ToString();
            enemyImage.sprite = combatAssets.GetEnemyImage(enemyComp.enemyType);
            slider.maxValue = enemyComp.healthPoints;
            slider.value = enemyComp.maxHealthPoints;
            SetSliderColor(slider.value, slider.maxValue);
            enemyComp.OnPlayerHealthPointsChange += UpdateEnergy;
        } 
    }

    public void UpdateEnergy()
    {
        slider.maxValue = enemyComp.healthPoints;
        slider.value = enemyComp.maxHealthPoints;
        SetSliderColor(slider.value, slider.maxValue);
    }

    public void SetSliderColor(float energyPoints, float maxEnergyPoints)
    {
        float result = energyPoints / maxEnergyPoints;
        if (result < 0.75f && result >= 0.5f)
        {
            energyFill.color = new Color32(250, 255, 0, 255);
        }
        else if (result < 0.5f && result >= 0.25f)
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

    public void SetEnemyComponent(Enemy enemyComponent)
    {
        enemyComp = enemyComponent;
    }

    public Enemy GetEnemyComponent()
    {
        return enemyComp;
    }

    public void SetCombatAssetsInstance(CombatAssets instance)
    {
        combatAssets = instance;
    }
}
