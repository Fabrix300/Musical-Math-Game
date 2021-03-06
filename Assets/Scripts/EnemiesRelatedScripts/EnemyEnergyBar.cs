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
            slider.maxValue = enemyComp.maxHealthPoints;
            slider.value = enemyComp.healthPoints;
            //SetSliderColor(slider.value, slider.maxValue);
            enemyComp.OnEnemyHealthPointsChange += UpdateEnergy;
        } 
    }

    public void UpdateEnergy()
    {
        slider.maxValue = enemyComp.maxHealthPoints;
        StartCoroutine(FillSliderEnemy());
        //slider.value = enemyComp.healthPoints;
        //SetSliderColor(slider.value, slider.maxValue);
    }

    public IEnumerator FillSliderEnemy()
    {
        float target = enemyComp.healthPoints;
        float value = slider.value;
        float currentTime = 0;
        while (currentTime < 1f)
        {
            slider.value = Mathf.Lerp(value, target, currentTime / 1f);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    /*public void SetSliderColor(float energyPoints, float maxEnergyPoints)
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
    }*/

    private void OnDestroy()
    {
        enemyComp.OnEnemyHealthPointsChange -= UpdateEnergy;
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
