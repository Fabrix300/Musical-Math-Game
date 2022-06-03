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
    private AudioSource[] noteButtonsAndDeleteButtonAudioSources = new AudioSource[5];
    public Text[] noteButtonsLimiterTexts;
    public Animator congratsMessageAnimator;

    //Playing Notes HUD
    public Animator playingNotesHUDAnimator;
    public Image playingNotesHUDImage;
    public Sprite[] musicalNotesImages;

    // WinnerMessageHUD
    public Animator winnerMessage;
    public Animator backOverlay;
    public GameObject winnerMessageContTitle;
    public GameObject wMCLevelBar;
    public Slider wMCLevelBarSlider;
    public Text wMCExpText;
    public GameObject wMCLevelUpText;

    public RequestTimer timer;
    public Text formulationText;
    public Text resultText;
    public Text enemyRequestText;

    private CombatState state;
    private Enemy enemyEnemyComp;
    private GameObject playerGO;
    //TurnIndicators
    private GameObject turnIndicatorPlayer;
    private GameObject turnIndicatorEnemy;
    // AttackEffects Animators
    private GameObject attackEffectOfPlayer;
    private GameObject attackEffectOfEnemy;
    private Animator attackEffectOfPlayerAnim;
    private Animator attackEffectOfEnemyAnim;
    //Hit Audio Sources
    private AudioSource hitOnPlayer;
    private AudioSource hitOnEnemy;

    private List<float> notesInput = new();
    private int[] musicalFiguresLimits = { 0, 0, 0, 0 };
    private List<float> enemyNotes = new();
    private List<Sound[]> musicalNotes;
    private List<Sound[]> enemyMusicalNotes;
    private float enemyRequestDecimal;

    private CombatData combatData; private PlayerStats playerStats; private CombatAssets combatAssets;
    private GameManager gameManager; private AudioManager audioManager;

    void Start()
    {
        state = CombatState.NONE;
        combatData = CombatData.instance; playerStats = PlayerStats.instance; combatAssets = CombatAssets.instance;
        gameManager = GameManager.instance; audioManager = AudioManager.instance;
        musicalNotes = new List<Sound[]>(audioManager.musicalNotes);
        enemyMusicalNotes = new List<Sound[]>(audioManager.enemyMusicalNotes);
        enemyRequestImage.sprite = combatAssets.GetEnemyImage(combatData.GetEnemyToCombat().enemyType);

        timer.OnTimerEnd += RequestTimeEnd;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        InstantiatePlayerAndEnemy();
        SetCombatHUDs();
        resultText.text = "0";

        yield return new WaitForSeconds(1f);
        turnIndicatorPlayer.SetActive(true);
        yield return new WaitForSeconds(1f);
        state = CombatState.PLAYERTURN;
        PlayerTurn();
        yield return null;
    }

    void PlayerTurn()
    {
        turnIndicatorPlayer.SetActive(true);
        if (GenerateRandomOperationForEnemyRequest())
        {
            TriggerStartAnimationOfHUDElements();
        }
        timer.RunTimer();
    }

    public bool GenerateRandomOperationForEnemyRequest()
    {
        float upperLimit = 41; float lowerLimit = 5;
        int multiplicatorResult = (int) Random.Range(lowerLimit, upperLimit);
        enemyRequestDecimal = 0.125f * multiplicatorResult;
        enemyRequestText.text = ConvertDecimalToFractionString(enemyRequestDecimal);
        GenerateRandomLimitsForMusicalFigures(multiplicatorResult);
        return true;
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
                    //noteButtonsAndDeleteButtonAudioSources[0].Play();
                    notesInput.Add(1f);
                    if (!formulationText || formulationText.text == "")
                    {
                        formulationText.text += "1";
                        musicalFiguresLimits[0] -= 1;
                        if (musicalFiguresLimits[0] == 0)
                        {
                            noteButtonsAndDeleteButton[0].interactable = false;
                        }
                        noteButtonsLimiterTexts[0].text = musicalFiguresLimits[0].ToString();
                    }
                    else
                    {
                        formulationText.text += " + 1";
                        musicalFiguresLimits[0] -= 1;
                        if (musicalFiguresLimits[0] == 0) noteButtonsAndDeleteButton[0].interactable = false;
                        noteButtonsLimiterTexts[0].text = musicalFiguresLimits[0].ToString();
                    }
                    break;
                }
            case "Blanca":
                {
                    //noteButtonsAndDeleteButtonAudioSources[1].Play();
                    notesInput.Add(0.5f);
                    if (!formulationText || formulationText.text == "")
                    {
                        formulationText.text += "1/2";
                        musicalFiguresLimits[1] -= 1;
                        if (musicalFiguresLimits[1] == 0) noteButtonsAndDeleteButton[1].interactable = false;
                        noteButtonsLimiterTexts[1].text = musicalFiguresLimits[1].ToString();
                    }
                    else
                    {
                        formulationText.text += " + 1/2";
                        musicalFiguresLimits[1] -= 1;
                        if (musicalFiguresLimits[1] == 0) noteButtonsAndDeleteButton[1].interactable = false;
                        noteButtonsLimiterTexts[1].text = musicalFiguresLimits[1].ToString();
                    }
                    break;
                }
            case "Negra": 
                {
                    //noteButtonsAndDeleteButtonAudioSources[2].Play();
                    notesInput.Add(0.25f);
                    if (!formulationText || formulationText.text == "")
                    {
                        formulationText.text += "1/4";
                        musicalFiguresLimits[2] -= 1;
                        if (musicalFiguresLimits[2] == 0) noteButtonsAndDeleteButton[2].interactable = false;
                        noteButtonsLimiterTexts[2].text = musicalFiguresLimits[2].ToString();
                    }
                    else 
                    { 
                        formulationText.text += " + 1/4";
                        musicalFiguresLimits[2] -= 1;
                        if (musicalFiguresLimits[2] == 0) noteButtonsAndDeleteButton[2].interactable = false;
                        noteButtonsLimiterTexts[2].text = musicalFiguresLimits[2].ToString();
                    }
                    break;
                }
            case "Corchea": 
                {
                    //noteButtonsAndDeleteButtonAudioSources[3].Play();
                    notesInput.Add(0.125f);
                    if (!formulationText || formulationText.text == "")
                    {
                        formulationText.text += "1/8";
                        musicalFiguresLimits[3] -= 1;
                        if (musicalFiguresLimits[3] == 0) noteButtonsAndDeleteButton[3].interactable = false;
                        noteButtonsLimiterTexts[3].text = musicalFiguresLimits[3].ToString();
                    }
                    else 
                    { 
                        formulationText.text += " + 1/8";
                        musicalFiguresLimits[3] -= 1;
                        if (musicalFiguresLimits[3] == 0) noteButtonsAndDeleteButton[3].interactable = false;
                        noteButtonsLimiterTexts[3].text = musicalFiguresLimits[3].ToString();
                    }
                    break;
                }
            case "BackSpace": 
                {
                    //noteButtonsAndDeleteButtonAudioSources[4].Play();
                    if (notesInput.Count > 1)
                    {
                        switch (notesInput[^1]) 
                        {
                            case 1f: 
                                {
                                    if (musicalFiguresLimits[0] == 0) noteButtonsAndDeleteButton[0].interactable = true;
                                    musicalFiguresLimits[0] += 1;
                                    noteButtonsLimiterTexts[0].text = musicalFiguresLimits[0].ToString();
                                    break;
                                }
                            case 0.5f:
                                {
                                    if (musicalFiguresLimits[1] == 0) noteButtonsAndDeleteButton[1].interactable = true;
                                    musicalFiguresLimits[1] += 1;
                                    noteButtonsLimiterTexts[1].text = musicalFiguresLimits[1].ToString();
                                    break;
                                }
                            case 0.25f:
                                {
                                    if (musicalFiguresLimits[2] == 0) noteButtonsAndDeleteButton[2].interactable = true;
                                    musicalFiguresLimits[2] += 1;
                                    noteButtonsLimiterTexts[2].text = musicalFiguresLimits[2].ToString();
                                    break;
                                }
                            case 0.125f:
                                {
                                    if (musicalFiguresLimits[3] == 0) noteButtonsAndDeleteButton[3].interactable = true;
                                    musicalFiguresLimits[3] += 1;
                                    noteButtonsLimiterTexts[3].text = musicalFiguresLimits[3].ToString();
                                    break;
                                }
                        }
                        notesInput.RemoveAt(notesInput.Count - 1);
                        int indexStartOfSubString = formulationText.text.LastIndexOf(" ") - 2;
                        formulationText.text = formulationText.text.Remove(indexStartOfSubString, formulationText.text.Length - indexStartOfSubString);
                    } 
                    else if (notesInput.Count == 1)
                    {
                        switch (notesInput[^1])
                        {
                            case 1f:
                                {
                                    musicalFiguresLimits[0] += 1;
                                    noteButtonsLimiterTexts[0].text = musicalFiguresLimits[0].ToString();
                                    break;
                                }
                            case 0.5f:
                                {
                                    musicalFiguresLimits[1] += 1;
                                    noteButtonsLimiterTexts[1].text = musicalFiguresLimits[1].ToString();
                                    break;
                                }
                            case 0.25f:
                                {
                                    musicalFiguresLimits[2] += 1;
                                    noteButtonsLimiterTexts[2].text = musicalFiguresLimits[2].ToString();
                                    break;
                                }
                            case 0.125f:
                                {
                                    musicalFiguresLimits[3] += 1;
                                    noteButtonsLimiterTexts[3].text = musicalFiguresLimits[3].ToString();
                                    break;
                                }
                        }
                        notesInput.RemoveAt(notesInput.Count - 1);
                        formulationText.text = "";
                    }
                    break;
                }
        }
        resultText.text = ConvertDecimalToFractionString(SumNotesInputValues());
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
        StartCoroutine(PlayerActionButTimeEnded());
    }

    IEnumerator PlayerActionButTimeEnded()
    {
        DisableNoteButtonsAndDeleteButton();
        yield return new WaitForSeconds(0.8f);
        TriggerEndAnimationOfHUDElements();
        yield return new WaitForSeconds(2f);
        EnableNoteButtonsAndDeleteButton();
        resultText.text = "0";
        formulationText.text = "";
        notesInput.Clear();
        musicalFiguresLimits = new int[] {0,0,0,0};
        state = CombatState.ENEMYTURN;
        turnIndicatorPlayer.SetActive(false);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerAction()
    {
        yield return new WaitForSeconds(0.3f);
        TriggerStartAnimationOfCongratsMessage();
        audioManager.Play("CorrectAnswer");
        yield return new WaitForSeconds(1f);
        TriggerEndAnimationOfHUDElements();
        yield return new WaitForSeconds(0.8f);
        TriggerEndAnimationOfCongratsMessage();
        yield return new WaitForSeconds(.5f);
        TriggerStartAnimationOfPlayingNotesHUD();
        yield return new WaitForSeconds(1f);
        EnableNoteButtonsAndDeleteButton();
        resultText.text = "0";
        formulationText.text = "";

        //make fox singggg
        playerGO.GetComponent<Animator>().SetInteger("state", 5);
        //Add an image animated with the singing effect
        StartCoroutine(audioManager.FadeVolumeDown("Combat" + gameManager.savedSceneName));
        for (int i = 0; i < notesInput.Count; i++)
        {
            Sound[] noteArray = musicalNotes[Random.Range(0, 8)];
            switch (notesInput[i]) 
            {
                case 1f: 
                    {
                        //Changing image of PlayingNotesHUD
                        playingNotesHUDImage.sprite = musicalNotesImages[0];
                        AudioSource aS = noteArray[0].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;                    
                    }
                case 0.5f: 
                    {
                        //Changing image of PlayingNotesHUD
                        playingNotesHUDImage.sprite = musicalNotesImages[1];
                        AudioSource aS = noteArray[1].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
                case 0.25f: 
                    {
                        //Changing image of PlayingNotesHUD
                        playingNotesHUDImage.sprite = musicalNotesImages[2];
                        AudioSource aS = noteArray[2].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
                case 0.125f: 
                    {
                        //Changing image of PlayingNotesHUD
                        playingNotesHUDImage.sprite = musicalNotesImages[3];
                        AudioSource aS = noteArray[3].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
            }
        }
        playerGO.GetComponent<Animator>().SetInteger("state", 0);
        StartCoroutine(audioManager.FadeVolumeUp("Combat" + gameManager.savedSceneName));

        yield return new WaitForSeconds(0.5f);
        TriggerEndAnimationOfPlayingNotesHUD();
        playingNotesHUDImage.sprite = musicalNotesImages[4];

        //////////////////
        attackEffectOfEnemyAnim.SetTrigger("Start");
        hitOnEnemy.Play();
        bool isEnemyDead = enemyEnemyComp.Numb(playerStats.damage * timer.GetTimerBonusMultiplicator());
        //playerGO.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 560);
        notesInput.Clear();
        musicalFiguresLimits = new int[] { 0, 0, 0, 0 };

        yield return new WaitForSeconds(1.5f);

        if (isEnemyDead)
        {
            /*Creating GameObject with audioSource to play death sound of enemy*/
            GameObject audioHelper = Instantiate(combatAssets.audioHelper, enemyEnemyComp.gameObject.transform.position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(audioHelper, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
            AudioSource aS = audioHelper.GetComponent<AudioSource>();
            aS.clip = enemyEnemyComp.gameObject.GetComponent<AudioSource>().clip; aS.Play();
            /*******************************/
            enemyEnemyComp.gameObject.GetComponent<Animator>().SetInteger("state", 2);
            yield return new WaitForSeconds(1f);
            turnIndicatorPlayer.SetActive(false);
            state = CombatState.WON;
            int expObtained = ((int)enemyEnemyComp.maxHealthPoints) / 2;
            wMCExpText.text = "+" + expObtained;
            winnerMessage.SetInteger("state", 1);
            backOverlay.SetInteger("state", 1);
            StartCoroutine(audioManager.FadeVolumeDown("Combat" + gameManager.savedSceneName));
            audioManager.Play("CombatWon");
            yield return new WaitForSeconds(0.5f);
            winnerMessageContTitle.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            wMCLevelBarSlider.maxValue = playerStats.GetPlayerMaxExpPoints();
            wMCLevelBarSlider.value = playerStats.GetPlayerExpPoints();
            int valueTemp = playerStats.GetPlayerExpPoints();
            int target = playerStats.GetPlayerExpPoints() + expObtained;
            wMCLevelBar.SetActive(true);
            if (target >= playerStats.GetPlayerMaxExpPoints())
            {
                wMCLevelUpText.SetActive(true);
                float currentTime = 0;
                while (currentTime < 0.8f)
                {
                    wMCLevelBarSlider.value = Mathf.Lerp(valueTemp, playerStats.GetPlayerMaxExpPoints(), currentTime / 0.8f);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                float currentTime = 0;
                while (currentTime < 0.8f)
                {
                    wMCLevelBarSlider.value = Mathf.Lerp(valueTemp, target, currentTime / 0.8f);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
            }
            StartCoroutine(audioManager.FadeVolumeUp("Combat" + gameManager.savedSceneName));
            _ = playerStats.AddExpPointsAndCheck(expObtained);
            yield return new WaitForSeconds(2.6f);
            EndCombat();
        }
        else
        {
            turnIndicatorPlayer.SetActive(false);
            state = CombatState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        turnIndicatorEnemy.SetActive(true);
        float[] temp = { 1f, 0.5f, 0.25f, 0.125f };
        int enemySinginglength = Random.Range(1, 7);
        for (int i = 0; i < enemySinginglength; i++)
        {
            enemyNotes.Add(temp[Random.Range(0,4)]);
        }
        yield return new WaitForSeconds(1f);

        // Make Enemy Sing
        StartCoroutine(audioManager.FadeVolumeDown("Combat" + gameManager.savedSceneName));
        for (int i = 0; i < enemyNotes.Count; i++)
        {
            Sound[] noteArray = enemyMusicalNotes[Random.Range(0, 8)];
            switch (enemyNotes[i])
            {
                case 1f:
                    {
                        //Changing image of PlayingNotesHUD
                        //playingNotesHUDImage.sprite = musicalNotesImages[0];
                        AudioSource aS = noteArray[0].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
                case 0.5f:
                    {
                        //Changing image of PlayingNotesHUD
                        //playingNotesHUDImage.sprite = musicalNotesImages[1];
                        AudioSource aS = noteArray[1].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
                case 0.25f:
                    {
                        //Changing image of PlayingNotesHUD
                        //playingNotesHUDImage.sprite = musicalNotesImages[2];
                        AudioSource aS = noteArray[2].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
                case 0.125f:
                    {
                        //Changing image of PlayingNotesHUD
                        //playingNotesHUDImage.sprite = musicalNotesImages[3];
                        AudioSource aS = noteArray[3].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
            }
        }
        StartCoroutine(audioManager.FadeVolumeUp("Combat" + gameManager.savedSceneName));

        //Damage the player
        attackEffectOfPlayerAnim.SetTrigger("Start");
        hitOnPlayer.Play();
        bool isPlayerDead = playerStats.NumbPlayer(enemyEnemyComp.GetTotalDamage());
        enemyNotes.Clear();
        yield return new WaitForSeconds(1f);

        //check if player is dead, actualizar estados y pasar a endcombat o playerTurn;
        if (isPlayerDead)
        {
            turnIndicatorEnemy.SetActive(false);
            state = CombatState.LOST;
            EndCombat();
        }
        else
        {
            turnIndicatorEnemy.SetActive(false);
            state = CombatState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndCombat()
    {
        if (state == CombatState.WON)
        {
            Debug.Log("You won!");
            gameManager.ComeBackFromCombatScene();
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

    private string ConvertDecimalToFractionString(float numerator)
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

    private void GenerateRandomLimitsForMusicalFigures(int multiplicatorResult)
    {
        if (multiplicatorResult % 8f == 0f)
        {
            float multiplicatorResultCheck1 = multiplicatorResult / 8f;
            musicalFiguresLimits[0] += (int)multiplicatorResultCheck1;
            // Adding random int to the limits to give more liberty to the player, adding also that number to respective text
            for (int i = 0; i < musicalFiguresLimits.Length; i++)
            {
                musicalFiguresLimits[i] += Random.Range(2, 7);
                noteButtonsLimiterTexts[i].text = musicalFiguresLimits[i].ToString();
            }
            return;
        }
        else
        {
            float multiplicatorResultCheck2 = multiplicatorResult / 8f;
            int integer = Mathf.FloorToInt(multiplicatorResultCheck2);
            musicalFiguresLimits[0] += integer;
            float decimalPart = multiplicatorResultCheck2 - integer;
            while (decimalPart > 0)
            {
                if (decimalPart - 0.5f >= 0)
                {
                    decimalPart -= 0.5f;
                    musicalFiguresLimits[1] += 1;
                }
                else if (decimalPart - 0.25f >= 0)
                {
                    decimalPart -= 0.25f;
                    musicalFiguresLimits[2] += 1;
                }
                else if (decimalPart - 0.125f >= 0)
                {
                    decimalPart -= 0.125f;
                    musicalFiguresLimits[3] += 1;
                }
            }
        }
        // Adding random int to the limits to give more liberty to the player, adding also that number to respective text
        for (int i = 0; i < musicalFiguresLimits.Length; i++)
        {
            musicalFiguresLimits[i] += Random.Range(2,5);
            noteButtonsLimiterTexts[i].text = musicalFiguresLimits[i].ToString();
        }

    }

    private float SumNotesInputValues()
    {
        float result = 0f;
        for (int i = 0; i < notesInput.Count; i++) result += notesInput[i];
        return result;
    }

    public void TriggerStartAnimationOfPlayingNotesHUD()
    {
        playingNotesHUDAnimator.SetInteger("state", 1);
    }

    public void TriggerEndAnimationOfPlayingNotesHUD()
    {
        playingNotesHUDAnimator.SetInteger("state", 2);
    }

    public void TriggerEndAnimationOfCongratsMessage()
    {
        congratsMessageAnimator.SetInteger("state", 2);
    }

    public void TriggerStartAnimationOfCongratsMessage()
    {
        congratsMessageAnimator.SetInteger("state", 1);
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

    public void InstantiatePlayerAndEnemy() {
        playerGO = Instantiate(combatAssets.playerPreFab, new Vector3(-5.5f, 10f, 0f), Quaternion.identity);
        turnIndicatorPlayer = Instantiate(combatAssets.turnIndicatorPlayer, new Vector3(-5.5f, 0.5f, 0f), Quaternion.identity);
        attackEffectOfPlayer = Instantiate(combatAssets.attackEffect, new Vector3(-5.5f, -1.002905f, 0f), Quaternion.identity);
        attackEffectOfPlayerAnim = attackEffectOfPlayer.GetComponent<Animator>();
        turnIndicatorPlayer.SetActive(false);
        SceneManager.MoveGameObjectToScene(playerGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(turnIndicatorPlayer, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(attackEffectOfPlayer, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        GameObject enemyGO = Instantiate(combatAssets.GetEnemyPreFab(combatData.GetEnemyToCombat().enemyType), new Vector3(5.5f, 10f, 0f), Quaternion.identity);
        turnIndicatorEnemy = Instantiate(combatAssets.turnIndicatorEnemy, new Vector3(5.5f, 0.5f, 0f), Quaternion.identity);
        attackEffectOfEnemy = Instantiate(combatAssets.attackEffect, new Vector3(5.5f, -1.002905f, 0f), Quaternion.identity);
        attackEffectOfEnemyAnim = attackEffectOfEnemy.GetComponent<Animator>();
        turnIndicatorEnemy.SetActive(false);
        SceneManager.MoveGameObjectToScene(enemyGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(turnIndicatorEnemy, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(attackEffectOfEnemy, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        playerGO.GetComponent<PlayerMovement>().SetInCombat(true);
        /**/

        foreach (Sound[] sArray in musicalNotes)
        {
            foreach (Sound s in sArray)
            {
                s.source = playerGO.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                //s.source.volume = s.volume;
                s.source.volume = 1f;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = 0.5f;
                s.source.playOnAwake = s.playOnAwake;
            }
        }
        foreach (Sound s in audioManager.sounds)
        {
            if (s.name == "1HitOnPlayer")
            {
                s.source = playerGO.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                //s.source.volume = s.volume;
                s.source.volume = 0.85f;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = 0.5f;
                hitOnPlayer = s.source;
            }
        }

        foreach (Sound[] sArray in enemyMusicalNotes)
        {
            foreach (Sound s in sArray)
            {
                s.source = enemyGO.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                //s.source.volume = s.volume;
                s.source.volume = 1f;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = 0.4f;
                s.source.playOnAwake = s.playOnAwake;
            }
        }
        foreach (Sound s in audioManager.sounds)
        {
            if (s.name == "1HitOnEnemy")
            {
                s.source = enemyGO.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                //s.source.volume = s.volume;
                s.source.volume = 0.85f;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = 0.4f;
                hitOnEnemy = s.source;
            }
        }

        /**/

        /* Create AudioSources For Buttons */

        AudioClip mFAC = audioManager.GetAudioClip("MusicalFigureButtonClick");
        AudioClip bSAC = audioManager.GetAudioClip("BackSpaceButtonClick");
        for (int i = 0; i < noteButtonsAndDeleteButton.Length; i++)
        {
            noteButtonsAndDeleteButtonAudioSources[i] = noteButtonsAndDeleteButton[i].gameObject.AddComponent<AudioSource>();
            if (i == 4)
            {
                noteButtonsAndDeleteButtonAudioSources[i].clip = bSAC;
            }
            else
            {
                noteButtonsAndDeleteButtonAudioSources[i].clip = mFAC;
            }
            noteButtonsAndDeleteButtonAudioSources[i].volume = 0.6f;
            noteButtonsAndDeleteButtonAudioSources[i].playOnAwake = false;
        }

        /**/

        enemyGO.transform.Find("CanvasHolder").gameObject.SetActive(false);
        enemyGO.GetComponent<EnemyMovement>().inCombat = true;
        enemyGO.GetComponent<Animator>().SetInteger("state", 0);
        enemyEnemyComp = enemyGO.GetComponent<Enemy>();

        Enemy enemyTemp = combatData.GetEnemyToCombat();
        enemyEnemyComp.SetAll(
            enemyTemp.enemyType,
            enemyTemp.enemyName,
            enemyTemp.level,
            enemyTemp.healthPoints,
            enemyTemp.maxHealthPoints);
    }

    public void SetCombatHUDs()
    {
        enemyEnergyBar.SetEnemyComponent(enemyEnemyComp);
        enemyEnergyBar.SetCombatAssetsInstance(combatAssets);
        enemyEnergyBar.SetValues();
    }

    public void ReproduceButtonSound(Button b)
    {
        switch (b.name)
        {
            case "Redonda": { noteButtonsAndDeleteButtonAudioSources[0].Play(); break; }
            case "Blanca": { noteButtonsAndDeleteButtonAudioSources[1].Play(); break; }
            case "Negra": { noteButtonsAndDeleteButtonAudioSources[2].Play(); break; }
            case "Corchea": { noteButtonsAndDeleteButtonAudioSources[3].Play(); break; }
            case "BackSpace": { noteButtonsAndDeleteButtonAudioSources[4].Play(); break; }
        }
    }
}