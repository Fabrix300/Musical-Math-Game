using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CombatState { NONE, PLAYERTURN, ENEMYTURN, WON, LOST }

public class CombatSystem : MonoBehaviour
{
    public EnemyEnergyBar enemyEnergyBar;
    public Image enemyRequestImage;
    public PlayerAnswerHUDTransitions[] HUDElements;
    public Button[] noteButtonsAndDeleteButton;
    public Text formulationText;
    public Text resultText;
    public Text enemyRequestText;

    private CombatState state;
    private GameObject enemyPreFab;
    private Enemy enemyEnemyComp;
    private GameObject playerPreFab;

    private List<float> notesInput = new();
    private float enemyRequestDecimal;

    private CombatData combatData; private PlayerStats playerStats; private CombatAssets combatAssets;

    void Start()
    {
        state = CombatState.NONE;
        combatData = CombatData.instance; playerStats = PlayerStats.instance; combatAssets = CombatAssets.instance;
        playerPreFab = combatAssets.playerPreFab;
        enemyPreFab = combatAssets.GetEnemyPreFab(combatData.GetEnemyToCombat().enemyType);
        enemyRequestImage.sprite = combatAssets.GetEnemyImage(combatData.GetEnemyToCombat().enemyType);

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        InstantiatePlayerAndEnemy();
        SetCombatHUDs();
        resultText.text = "0";

        yield return new WaitForSeconds(2f);
        state = CombatState.PLAYERTURN;
        PlayerTurn();
        // set a timer
    }

    IEnumerator EnemyTurn()
    {
        //Attack the player or heal or whatever
        //Update HUDS
        yield return new WaitForSeconds(2f);
        //check if player is dead, actualizar estados y pasar a endcombat o playerTurn;
    }

    void PlayerTurn()
    {
        ActivateAnimatorsOfPlayerAnswerHUD();
        // register input of buttons DONE;
        GenerateRandomOperationForEnemyRequest();
        // check if correct
    }

    public void GenerateRandomOperationForEnemyRequest()
    {
        float upperLimit = 40;
        float lowerLimit = 6;
        int multiplicatorResult = (int) Random.Range(lowerLimit, upperLimit);
        enemyRequestDecimal = 0.125f * multiplicatorResult;
        enemyRequestText.text = ConvertToFractionString(enemyRequestDecimal);
    }

    public void OnPressNoteButton(Button clickedButton)
    {
        if (state != CombatState.PLAYERTURN)
        {
            return;
        }
        switch (clickedButton.name)
        {
            case "Redonda":{
                    notesInput.Add(1f);
                    if (!formulationText || formulationText.text == "") formulationText.text += "1";
                    else formulationText.text += " + 1";
                    break;
                }
            case "Blanca": {
                    notesInput.Add(0.5f);
                    if (!formulationText || formulationText.text == "") formulationText.text += "1/2";
                    else formulationText.text += " + 1/2";
                    break;
                }
            case "Negra": {
                    notesInput.Add(0.25f);
                    if (!formulationText || formulationText.text == "") formulationText.text += "1/4";
                    else formulationText.text += " + 1/4";
                    break;
                }
            case "Corchea": {
                    notesInput.Add(0.125f);
                    if (!formulationText || formulationText.text == "") formulationText.text += "1/8";
                    else formulationText.text += " + 1/8";
                    break;
                }
            case "BackSpace": {
                    if (notesInput.Count > 1)
                    {
                        notesInput.RemoveAt(notesInput.Count - 1);
                        int indexStartOfSubString = formulationText.text.LastIndexOf(" ") - 2;
                        //Debug.Log('"' + formulationText.text + '"');
                        formulationText.text = formulationText.text.Remove(indexStartOfSubString, formulationText.text.Length - indexStartOfSubString);
                    } else if (notesInput.Count == 1)
                    {
                        notesInput.RemoveAt(notesInput.Count - 1);
                        formulationText.text = "";
                    }
                    break;
                }
        }
        //CalculateResult();
        resultText.text = ConvertToFractionString(SumNotesInputValues());
        if(CheckResult())
        {
            StartCoroutine(PlayerAction());
        }
    }

    IEnumerator PlayerAction()
    {
        DisableNoteButtonsAndDeleteButton();
        yield return new WaitForSeconds(1f);
        //Show ui element that indicates correct
        TriggerEndAnimationOfHUDElements();
        //make fox sing
        bool isEnemyDead = enemyEnemyComp.Numb(playerStats.damage.GetValue());

        // UPDATE ENEMY HUD

        yield return new WaitForSeconds(2f);

        if (isEnemyDead)
        {
            state = CombatState.WON;
            EndCombat();
        }
        else
        {
            state = CombatState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    /*private void CalculateResult()
    {
        float numerator = 0f;
        for (int i = 0; i < notesInput.Count; i++)
        {
            numerator += notesInput[i];
        }
        string test = numerator.ToString();
        int indexOfDot = test.LastIndexOf(".");
        if (indexOfDot > -1) //has decimals!
        {
            int numberOfDecimals = test.Length - 1 - indexOfDot;
            float multiplicator = Mathf.Pow(10f, numberOfDecimals);
            float denominator = 1f;
            numerator *= multiplicator;
            denominator *= multiplicator;
            int gcd = GCD((int)numerator, (int)denominator);
            numerator /= gcd;
            denominator /= gcd;
            resultText.text = numerator.ToString() + "/" + denominator.ToString();
        } 
        else
        {
            resultText.text = numerator.ToString();
        }
    }*/

    void EndCombat()
    {
        if (state == CombatState.WON)
        {
            Debug.Log("You won!");
        }
        else if (state == CombatState.LOST)
        {
            Debug.Log("You lost.");
        }
    }

    private bool CheckResult()
    {
        if (enemyRequestDecimal == SumNotesInputValues())
        {
            return true;
        }
        return false;
    }

    private string ConvertToFractionString(float numerator)
    {
        string test = numerator.ToString();
        int indexOfDot = test.LastIndexOf(".");
        if (indexOfDot > -1) //has decimals!
        {
            int numberOfDecimals = test.Length - 1 - indexOfDot;
            float multiplicator = Mathf.Pow(10f, numberOfDecimals);
            float denominator = 1f;
            numerator *= multiplicator;
            denominator *= multiplicator;
            int gcd = GCD((int)numerator, (int)denominator);
            numerator /= gcd;
            denominator /= gcd;
            return numerator.ToString() + "/" + denominator.ToString();
        }
        else
        {
            return numerator.ToString();
        }
    }

    private int GCD(int a, int b)
    {
        int Remainder;
        while (b != 0)
        {
            Remainder = a % b;
            a = b;
            b = Remainder;
        }
        return a;
    }

    private float SumNotesInputValues()
    {
        float result = 0f;
        for (int i = 0; i < notesInput.Count; i++) result += notesInput[i];
        return result;
    }

    public void DisableNoteButtonsAndDeleteButton()
    {
        for (int i = 0; i < noteButtonsAndDeleteButton.Length; i++)
        {
            noteButtonsAndDeleteButton[i].interactable = false;
        }
    }

    public void TriggerEndAnimationOfHUDElements()
    {
        for (int i = 0; i < HUDElements.Length; i++)
        {
            HUDElements[i].TriggerEndAnimation();
        }
    }

    public void ActivateAnimatorsOfPlayerAnswerHUD()
    {
        for(int i = 0; i < HUDElements.Length; i++)
        {
            HUDElements[i].ActivateAnimatorAndStartAnimation();
        }
    }

    public void InstantiatePlayerAndEnemy() {
        GameObject playerGO = Instantiate(playerPreFab, new Vector3(-5.5f, 10f, 0f), Quaternion.identity);
        SceneManager.MoveGameObjectToScene(playerGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        GameObject enemyGO = Instantiate(enemyPreFab, new Vector3(5.5f, 10f, 0f), Quaternion.identity);
        SceneManager.MoveGameObjectToScene(enemyGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        playerGO.GetComponent<PlayerMovement>().SetInCombat(true);
        enemyGO.GetComponent<EnemyMovement>().inCombat = true;
        enemyGO.GetComponent<Animator>().SetInteger("state", 0);
        enemyEnemyComp = enemyGO.GetComponent<Enemy>();

        enemyEnemyComp.SetAll(
            combatData.GetEnemyToCombat().enemyType,
            combatData.GetEnemyToCombat().enemyName,
            combatData.GetEnemyToCombat().level,
            combatData.GetEnemyToCombat().healthPoints,
            combatData.GetEnemyToCombat().maxHealthPoints);
    }

    public void SetCombatHUDs()
    {
        enemyEnergyBar.SetEnemyComponent(enemyEnemyComp);
        enemyEnergyBar.SetCombatAssetsInstance(combatAssets);
        enemyEnergyBar.SetValues();
    }
}
