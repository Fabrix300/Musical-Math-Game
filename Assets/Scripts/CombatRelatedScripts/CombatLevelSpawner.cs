using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLevelSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] listOfCLs;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        gameManager.OnCombatLevelLoaded += ActivateCombatLevelHolder;
    }

    public void ActivateCombatLevelHolder()
    {
        int actualLevelNumber = int.Parse(gameManager.savedSceneName[5..]);
        listOfCLs[actualLevelNumber - 1].SetActive(true);
    }

    private void OnDestroy()
    {
        gameManager.OnCombatLevelLoaded -= ActivateCombatLevelHolder;
    }
}
