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
    public Animator congratsMessageAnimator;
    public RequestTimer timer;
    public Text formulationText;
    public Text resultText;
    public Text enemyRequestText;

    private CombatState state;
    private GameObject enemyPreFab;
    private Enemy enemyEnemyComp;
    private GameObject playerPreFab;

    private List<float> notesInput = new();
    private float enemyRequestDecimal;
    private bool isPlayerFirstTurn = true;
    private bool isFirstTimeActivatingCongratsMessageAnimator = true;

    private CombatData combatData; private PlayerStats playerStats; private CombatAssets combatAssets;

    void Start()
    {
        state = CombatState.NONE;
        combatData = CombatData.instance; playerStats = PlayerStats.instance; combatAssets = CombatAssets.instance;
        playerPreFab = combatAssets.playerPreFab;
        enemyPreFab = combatAssets.GetEnemyPreFab(combatData.GetEnemyToCombat().enemyType);
        enemyRequestImage.sprite = combatAssets.GetEnemyImage(combatData.GetEnemyToCombat().enemyType);

        timer.OnTimerEnd += RequestTimeEnd;
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
    }

    IEnumerator EnemyTurn()
    {
        //Attack the player or heal or whatever
        bool isPlayerDead = playerStats.NumbPlayer(enemyEnemyComp.GetTotalDamage());
        yield return new WaitForSeconds(1f);

        //check if player is dead, actualizar estados y pasar a endcombat o playerTurn;
        if (isPlayerDead)
        {
            state = CombatState.LOST;
            EndCombat();
        } else
        {
            state = CombatState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        if(isPlayerFirstTurn)
        {
            ActivateAnimatorsOfPlayerAnswerHUD();
        } 
        else
        {
            TriggerStartAnimationOfHUDElements();
        }
        GenerateRandomOperationForEnemyRequest();

        timer.RunTimer();
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
        resultText.text = ConvertToFractionString(SumNotesInputValues());
        if(CheckResult())
        {
            timer.StopTimer();
            DisableNoteButtonsAndDeleteButton();
            StartCoroutine(PlayerAction());
        }
    }

    public void RequestTimeEnd()
    {
        timer.StopTimer();
        DisableNoteButtonsAndDeleteButton();
        StartCoroutine(PlayerActionButTimeEnded());
    }

    IEnumerator PlayerActionButTimeEnded()
    {
        yield return new WaitForSeconds(0.8f);
        TriggerEndAnimationOfHUDElements();
        yield return new WaitForSeconds(2f);
        EnableNoteButtonsAndDeleteButton();
        resultText.text = "0";
        formulationText.text = "";
        notesInput.Clear();
        //make fox sing
        /*bool isEnemyDead = enemyEnemyComp.Numb(playerStats.damage.GetValue() * timer.GetTimerBonusMultiplicator());

        yield return new WaitForSeconds(1.5f);*/
        state = CombatState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        isPlayerFirstTurn = false;
    }

    IEnumerator PlayerAction()
    {
        yield return new WaitForSeconds(0.3f);
        if(isFirstTimeActivatingCongratsMessageAnimator)
        {
            EnableCongratsMessageAnimator();
            isFirstTimeActivatingCongratsMessageAnimator = false;
        }
        else
        { 
            TriggerStartAnimationOfCongratsMessage();
        }
        yield return new WaitForSeconds(1f);
        TriggerEndAnimationOfHUDElements();
        yield return new WaitForSeconds(0.8f);
        TriggerEndAnimationOfCongratsMessage();
        yield return new WaitForSeconds(1.5f);
        EnableNoteButtonsAndDeleteButton();
        resultText.text = "0";
        formulationText.text = "";
        notesInput.Clear();
        //make fox sing
        bool isEnemyDead = enemyEnemyComp.Numb(playerStats.damage.GetValue() * timer.GetTimerBonusMultiplicator());

        yield return new WaitForSeconds(1.5f);

        if (isEnemyDead)
        {
            state = CombatState.WON;
            EndCombat();
            isPlayerFirstTurn = false;
        }
        else
        {
            state = CombatState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            isPlayerFirstTurn = false;
        }
    }

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

    public void TriggerEndAnimationOfCongratsMessage()
    {
        congratsMessageAnimator.SetInteger("state", 1);
    }

    public void TriggerStartAnimationOfCongratsMessage()
    {
        congratsMessageAnimator.SetInteger("state", 0);
    }

    public void EnableCongratsMessageAnimator()
    {
        congratsMessageAnimator.enabled = true;
    }

    public void EnableNoteButtonsAndDeleteButton()
    {
        for (int i = 0; i < noteButtonsAndDeleteButton.Length; i++)
        {
            noteButtonsAndDeleteButton[i].interactable = true;
        }
    }

    public void DisableNoteButtonsAndDeleteButton()
    {
        for (int i = 0; i < noteButtonsAndDeleteButton.Length; i++)
        {
            noteButtonsAndDeleteButton[i].interactable = false;
        }
    }

    public void TriggerStartAnimationOfHUDElements()
    {
        for (int i = 0; i < HUDElements.Length; i++)
        {
            HUDElements[i].TriggerStartAnimation();
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
