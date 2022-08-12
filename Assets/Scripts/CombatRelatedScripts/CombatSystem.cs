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
    // congratsMessage
    public Animator congratsMessageAnimator;
    public Text congratsMessage;
    public Image fractionMark;
    public Image noteMark;
    public Text multiplicatorText;
    public Image starImage;
    // Transforms of Notes Frames
    public List<RectTransform> notesRectTransforms;
    public RectTransform firstFrame;
    public RectTransform pentagramNotesParent;
    public List<GameObject> pentagramNotesPrefabs;
    // Note Selector
    public NoteSelectorSwipe noteSelectorSwipe;
    // Random Note Toggle
    public Toggle randomNoteToggle;
    // MoodIndicator
    public Text moodIndicatorText; public GameObject moodIndicatorCorrectMark;
    // Pentagram
    public Animator pentagramAnimator;
    public PentagramPlayingGuide pentagramGuide;
    //Playing Notes HUD
    //public Animator playingNotesHUDAnimator;
    //public Image playingNotesHUDImage;
    //public Sprite[] musicalNotesImages;
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
    //Damage Indicators
    private GameObject damageIndicatorOfPlayer; private GameObject damageIndicatorOfEnemy;
    private Animator damageIndicatorOfPlayerAnim; private Animator damageIndicatorOfEnemyAnim;
    //TurnIndicators
    private GameObject turnIndicatorPlayer; private GameObject turnIndicatorEnemy;
    // AttackEffects Animators
    private GameObject attackEffectOfPlayer; private Animator attackEffectOfPlayerAnim;
    private GameObject attackEffectOfEnemy; private Animator attackEffectOfEnemyAnim;
    //Hit Audio Sources
    private AudioSource hitOnPlayer;
    private AudioSource hitOnEnemy;

    private List<float> musicalFiguresInput = new();
    private List<int> notesInput = new();
    private List<GameObject> instantiatedPentagramNotes = new();
    private int[] musicalFiguresLimits = { 0, 0, 0, 0 };
    private List<float> enemyNotes = new();
    private List<Sound[]> musicalNotes;
    private List<Sound[]> enemyMusicalNotes;
    private float enemyRequestDecimal;
    private readonly string[] moods = { "Calmado", "Serio", "Molesto", "Furioso" };
    private int enemyMood;
    private List<int> enemyMoodSpecificNotes = new();
    private float multiplier = 1.50f;

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
        //gameManager.OnCombatLevelLoaded += ActivateCombatLevelHolder;
        StartCoroutine(SetupBattle());
    }

    private void OnDestroy()
    {
        timer.OnTimerEnd -= RequestTimeEnd;
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
            GenerateRandomEnemyMood();
            TriggerStartAnimationOfHUDElements();
            pentagramAnimator.SetInteger("state", 1);
        }
        timer.RunTimer();
    }

    public bool GenerateRandomOperationForEnemyRequest()
    {
        int upperLimit = 33; int lowerLimit = 5;
        int multiplicatorResult = Random.Range(lowerLimit, upperLimit);
        enemyRequestDecimal = 0.125f * multiplicatorResult;
        enemyRequestText.text = ConvertDecimalToFractionString(enemyRequestDecimal);
        GenerateRandomLimitsForMusicalFigures(multiplicatorResult);
        return true;
    }

    public void GenerateRandomEnemyMood()
    {
        enemyMood = Random.Range(0, 4);
        moodIndicatorText.text = moods[enemyMood];
        switch (enemyMood)
        {
            case 0: 
                {
                    //for (int i = 0; i < Random.Range(1, 3); i++) { enemyMoodSpecificNotes.Add(Random.Range(0, 2)); }
                    for (int i = 0; i < 2; i++) { enemyMoodSpecificNotes.Add(Random.Range(0, 2)); }
                    moodIndicatorText.color = new Color(0f, 140f/255f, 0f); break; 
                }
            case 1: 
                {
                    //for (int i = 0; i < Random.Range(1, 3); i++) { enemyMoodSpecificNotes.Add(Random.Range(2,4)); }
                    for (int i = 0; i < 2; i++) { enemyMoodSpecificNotes.Add(Random.Range(2, 4)); }
                    moodIndicatorText.color = new Color(140f/255f, 140f/255f, 0f); break; 
                }
            case 2: 
                {
                    for (int i = 0; i < 2; i++) { enemyMoodSpecificNotes.Add(Random.Range(4, 6)); }
                    moodIndicatorText.color = new Color(140f/255f, 70f/255f, 0f); break; 
                }
            case 3: 
                {
                    for (int i = 0; i < 2; i++) { enemyMoodSpecificNotes.Add(Random.Range(6, 8)); }
                    moodIndicatorText.color = new Color(140f/255f, 0f, 0f); break; 
                }
        }
    }

    public void OnPressNoteButton(Button clickedButton)
    {
        if (state != CombatState.PLAYERTURN) return;
        switch (clickedButton.name)
        {
            case "Redonda":{
                    if (SumNotesInputValues() + 1.000f > 4) return;
                    musicalFiguresInput.Add(1f);
                    if (randomNoteToggle.isOn) notesInput.Add(Random.Range(0, 8)); 
                    else notesInput.Add(noteSelectorSwipe.GetSelected());
                    InstantiateLastNoteInputInPentagram(0, notesInput[^1]);
                    if (!formulationText || formulationText.text == "")
                    {
                        formulationText.text += "1";
                        musicalFiguresLimits[0] -= 1;
                        if (musicalFiguresLimits[0] == 0) noteButtonsAndDeleteButton[0].interactable = false;
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
                    if (SumNotesInputValues() + 0.500f > 4) return;
                    musicalFiguresInput.Add(0.5f);
                    if (randomNoteToggle.isOn) notesInput.Add(Random.Range(0, 8));
                    else notesInput.Add(noteSelectorSwipe.GetSelected());
                    InstantiateLastNoteInputInPentagram(1, notesInput[^1]);
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
                    if (SumNotesInputValues() + 0.250f > 4) return;
                    musicalFiguresInput.Add(0.25f);
                    if (randomNoteToggle.isOn) notesInput.Add(Random.Range(0, 8));
                    else notesInput.Add(noteSelectorSwipe.GetSelected());
                    InstantiateLastNoteInputInPentagram(2, notesInput[^1]);
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
                    if (SumNotesInputValues() + 0.125f > 4) return;
                    musicalFiguresInput.Add(0.125f);
                    if (randomNoteToggle.isOn) notesInput.Add(Random.Range(0, 8));
                    else notesInput.Add(noteSelectorSwipe.GetSelected());
                    InstantiateLastNoteInputInPentagram(3, notesInput[^1]);
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
                    if (musicalFiguresInput.Count > 1)
                    {
                        switch (musicalFiguresInput[^1]) 
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
                        musicalFiguresInput.RemoveAt(musicalFiguresInput.Count - 1);
                        notesInput.RemoveAt(notesInput.Count - 1);
                        Destroy(instantiatedPentagramNotes[^1]);
                        instantiatedPentagramNotes.RemoveAt(instantiatedPentagramNotes.Count - 1);
                        int indexStartOfSubString = formulationText.text.LastIndexOf(" ") - 2;
                        formulationText.text = formulationText.text.Remove(indexStartOfSubString, formulationText.text.Length - indexStartOfSubString);
                    } 
                    else if (musicalFiguresInput.Count == 1)
                    {
                        switch (musicalFiguresInput[^1])
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
                        formulationText.text = "";
                        musicalFiguresInput.RemoveAt(musicalFiguresInput.Count - 1);
                        notesInput.RemoveAt(notesInput.Count - 1);
                        Destroy(instantiatedPentagramNotes[^1]);
                        instantiatedPentagramNotes.RemoveAt(instantiatedPentagramNotes.Count - 1);
                    }
                    break;
                }
        }
        resultText.text = ConvertDecimalToFractionString(SumNotesInputValues());
        moodIndicatorCorrectMark.SetActive(CheckResultIfSecondMultiplier());
        if (CheckResult())
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
        pentagramAnimator.SetInteger("state", 4);
        yield return new WaitForSeconds(2f);
        EnableNoteButtonsAndDeleteButton();
        moodIndicatorCorrectMark.SetActive(false);
        resultText.text = "0";
        formulationText.text = "";
        musicalFiguresInput.Clear();
        notesInput.Clear();
        DestroyInstantiatedNotesInPentagram();
        enemyMoodSpecificNotes.Clear();
        musicalFiguresLimits = new int[] {0,0,0,0};
        noteSelectorSwipe.EnableScroll();
        state = CombatState.ENEMYTURN;
        turnIndicatorPlayer.SetActive(false);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerAction()
    {
        yield return new WaitForSeconds(0.2f);
        audioManager.Play("CorrectAnswer");
        TriggerStartAnimationAndSetupCongratsMessage();
        yield return new WaitForSeconds(1.2f);
        TriggerEndAnimationOfHUDElements();
        yield return new WaitForSeconds(1.2f);
        TriggerEndAnimationOfCongratsMessage();
        yield return new WaitForSeconds(.5f);
        //TriggerStartAnimationOfPlayingNotesHUD();
        pentagramAnimator.SetInteger("state", 2);
        pentagramGuide.SetActiveGameObject(true);
        yield return new WaitForSeconds(1f);
        EnableNoteButtonsAndDeleteButton();
        resultText.text = "0";
        formulationText.text = "";

        //make fox singggg
        playerGO.GetComponent<Animator>().SetInteger("state", 5);
        //Add an image animated with the singing effect
        StartCoroutine(audioManager.FadeVolumeDown("Combat" + gameManager.savedSceneName));
        pentagramGuide.MoveToPos(new Vector2(0,0), 6f);
        for (int i = 0; i < musicalFiguresInput.Count; i++)
        {
            //Sound[] noteArray = musicalNotes[Random.Range(0, 8)];
            Sound[] noteArray = musicalNotes[notesInput[i]];
            switch (musicalFiguresInput[i]) 
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
        playerGO.GetComponent<Animator>().SetInteger("state", 0);
        StartCoroutine(audioManager.FadeVolumeUp("Combat" + gameManager.savedSceneName));

        yield return new WaitForSeconds(0.5f);
        //TriggerEndAnimationOfPlayingNotesHUD();
        pentagramAnimator.SetInteger("state", 3);
        //playingNotesHUDImage.sprite = musicalNotesImages[4];

        //////////////////
        float playerTotalDamage = playerStats.damage * multiplier;
        attackEffectOfEnemyAnim.SetTrigger("Start");
        damageIndicatorOfEnemy.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = playerTotalDamage.ToString("N0");
        damageIndicatorOfEnemyAnim.SetTrigger("start");
        hitOnEnemy.Play();
        //bool isEnemyDead = enemyEnemyComp.Numb(playerStats.damage * timer.GetTimerBonusMultiplicator());
        bool isEnemyDead = enemyEnemyComp.Numb(playerTotalDamage);
        //playerGO.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 560);
        musicalFiguresInput.Clear();
        notesInput.Clear();
        DestroyInstantiatedNotesInPentagram();
        moodIndicatorCorrectMark.SetActive(false);
        enemyMoodSpecificNotes.Clear();
        musicalFiguresLimits = new int[] { 0, 0, 0, 0 };
        noteSelectorSwipe.EnableScroll();
        multiplier = 1.50f;

        yield return new WaitForSeconds(1.5f);

        if (isEnemyDead)
        {
            /*Creating GameObject with audioSource to play death sound of enemy*/
            GameObject audioHelper = Instantiate(combatAssets.audioHelper, enemyEnemyComp.gameObject.transform.position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(audioHelper, SceneManager.GetSceneByName("CombatLevel"));
            AudioSource aS = audioHelper.GetComponent<AudioSource>();
            aS.clip = enemyEnemyComp.gameObject.GetComponent<AudioSource>().clip; aS.Play();
            /*******************************/
            enemyEnemyComp.gameObject.GetComponent<Animator>().SetTrigger("death");
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
        for (int i = 0; i < Random.Range(1, 7); i++) enemyNotes.Add(temp[Random.Range(0,4)]);
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
                        AudioSource aS = noteArray[2].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
                case 0.125f:
                    {
                        AudioSource aS = noteArray[3].source;
                        aS.Play();
                        yield return new WaitWhile(() => aS.isPlaying);
                        break;
                    }
            }
        }
        StartCoroutine(audioManager.FadeVolumeUp("Combat" + gameManager.savedSceneName));

        //Damage the player
        hitOnPlayer.Play();
        attackEffectOfPlayerAnim.SetTrigger("Start");
        damageIndicatorOfPlayer.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = enemyEnemyComp.GetTotalDamage().ToString("N0");
        damageIndicatorOfPlayerAnim.SetTrigger("start");
        playerGO.GetComponent<Animator>().SetInteger("state", 6);
        bool isPlayerDead = playerStats.NumbPlayer(enemyEnemyComp.GetTotalDamage());
        enemyNotes.Clear();
        yield return new WaitForSeconds(1f);

        //check if player is dead, actualizar estados y pasar a endcombat o playerTurn;
        if (isPlayerDead)
        {
            yield return new WaitForSeconds(0.2f);
            playerGO.GetComponent<Animator>().SetTrigger("death");
            yield return new WaitForSeconds(1f);
            turnIndicatorEnemy.SetActive(false);
            yield return new WaitForSeconds(1f);
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
            gameManager.ShowLostMessage();
        }
    }

    private bool CheckResultIfSecondMultiplier()
    {
        bool checkfirstRequestedNoteFlag = true;
        bool checkSecondRequestedNoteFlag = true;
        for (int i = 0; i < notesInput.Count; i++)
        {
            if (checkfirstRequestedNoteFlag == true && notesInput[i] == enemyMoodSpecificNotes[0])
            {
                Debug.Log("primera encontrada");
                checkfirstRequestedNoteFlag = false;
            }
            else if (checkSecondRequestedNoteFlag == true && notesInput[i] == enemyMoodSpecificNotes[1])
            {
                Debug.Log("segunda encontrada");
                checkSecondRequestedNoteFlag = false;
            }
            if (checkfirstRequestedNoteFlag == false && checkSecondRequestedNoteFlag == false)
            {
                multiplier = 2.50f;
                return true;
            }
        }
        multiplier = 1.50f;
        return false;
    }

    private bool CheckResult()
    {
        if (enemyRequestDecimal == SumNotesInputValues())
        {
            return true;
        }
        return false;
    }

    private void InstantiateLastNoteInputInPentagram(int musicalFigureUsed, int noteSelected)
    {
        //firstFrame.anchoredPosition.y - (noteYFactor * (7 - noteSelected))
        //float noteYFactor = firstFrame.sizeDelta.y;
        if (instantiatedPentagramNotes.Count == 0)
        {
            instantiatedPentagramNotes.Add(
                Instantiate(pentagramNotesPrefabs[musicalFigureUsed], pentagramNotesParent)
            );
            RectTransform temp = instantiatedPentagramNotes[^1].GetComponent<RectTransform>();
            temp.localScale = new Vector3(1f, 1f, 1f);
            temp.anchoredPosition = new Vector2(
                temp.sizeDelta.x/2,
                notesRectTransforms[noteSelected].anchoredPosition.y
                );
        }
        else
        {
            RectTransform instPentaNotesLast = instantiatedPentagramNotes[^1].GetComponent<RectTransform>();
            instantiatedPentagramNotes.Add(
                Instantiate(pentagramNotesPrefabs[musicalFigureUsed], pentagramNotesParent)
                );
            RectTransform temp = instantiatedPentagramNotes[^1].GetComponent<RectTransform>();
            temp.localScale = new Vector3(1f, 1f, 1f);
            temp.anchoredPosition = new Vector2(
                instPentaNotesLast.anchoredPosition.x + (instPentaNotesLast.sizeDelta.x / 2) + (temp.sizeDelta.x / 2),
                notesRectTransforms[noteSelected].anchoredPosition.y
                );
        }
    }

    private void DestroyInstantiatedNotesInPentagram()
    {
        foreach(GameObject gO in instantiatedPentagramNotes) 
        {
            Destroy(gO);
        }
        instantiatedPentagramNotes.Clear();
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
        for (int i = 0; i < musicalFiguresInput.Count; i++) result += musicalFiguresInput[i];
        return result;
    }

    public void PlaySpecificNotesAccordingToEnemyMood(Button _enemyMoodNotesButton) { StartCoroutine(PlaySpecificNotesAccordingToEnemyMoodCo(_enemyMoodNotesButton)); }

    private IEnumerator PlaySpecificNotesAccordingToEnemyMoodCo(Button _enemyMoodNotesButton)
    {
        _enemyMoodNotesButton.GetComponent<Button>().interactable = false;
        //Sound[] noteArray = enemyMusicalNotes[Random.Range(0, 8)];
        for (int i = 0; i < enemyMoodSpecificNotes.Count; i++)
        {
            Sound[] noteArray = enemyMusicalNotes[enemyMoodSpecificNotes[i]];
            AudioSource aS = noteArray[1].source;
            aS.Play();
            yield return new WaitWhile(() => aS.isPlaying);
        }
        _enemyMoodNotesButton.GetComponent<Button>().interactable = true;
        yield return null;
    }

    /*public void TriggerStartAnimationOfPlayingNotesHUD()
    {
        playingNotesHUDAnimator.SetInteger("state", 1);
    }

    public void TriggerEndAnimationOfPlayingNotesHUD()
    {
        playingNotesHUDAnimator.SetInteger("state", 2);
    }*/

    public void TriggerEndAnimationOfCongratsMessage()
    {
        congratsMessageAnimator.SetInteger("state", 2);
    }

    public void TriggerStartAnimationAndSetupCongratsMessage()
    {
        if (multiplier < 2f)
        {
            congratsMessage.text = "�Bien!";
            fractionMark.sprite = combatAssets.marksOnCongratsMessage[0];
            noteMark.sprite = combatAssets.marksOnCongratsMessage[1];
            starImage.gameObject.SetActive(false);
            multiplicatorText.text = multiplier.ToString("N1");
            multiplicatorText.color = new Color(140f / 255f, 70f / 255f, 0f);
        } else
        {
            congratsMessage.text = "�Bien Hecho!";
            fractionMark.sprite = combatAssets.marksOnCongratsMessage[0];
            noteMark.sprite = combatAssets.marksOnCongratsMessage[0];
            starImage.gameObject.SetActive(true);
            multiplicatorText.text = multiplier.ToString("N1");
            multiplicatorText.color = new Color(140f / 255f, 0f, 0f);
        }
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
        damageIndicatorOfPlayer = Instantiate(combatAssets.damageIndicator, playerGO.transform);
        damageIndicatorOfPlayerAnim = damageIndicatorOfPlayer.GetComponent<Animator>();
        attackEffectOfPlayerAnim = attackEffectOfPlayer.GetComponent<Animator>();
        turnIndicatorPlayer.SetActive(false);
        /*SceneManager.MoveGameObjectToScene(playerGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(turnIndicatorPlayer, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(attackEffectOfPlayer, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));*/
        SceneManager.MoveGameObjectToScene(playerGO, SceneManager.GetSceneByName("CombatLevel"));
        SceneManager.MoveGameObjectToScene(turnIndicatorPlayer, SceneManager.GetSceneByName("CombatLevel"));
        SceneManager.MoveGameObjectToScene(attackEffectOfPlayer, SceneManager.GetSceneByName("CombatLevel"));
        Vector3 instantiatePosOfEnemy = new(5.5f, 10f, 0f);
        if (combatData.GetEnemyToCombat().enemyType == EnemyType.eagle) instantiatePosOfEnemy = new Vector3(5.5f, -0.75f, 0f);
        GameObject enemyGO = Instantiate(combatAssets.GetEnemyPreFab(combatData.GetEnemyToCombat().enemyType), instantiatePosOfEnemy, Quaternion.identity);
        turnIndicatorEnemy = Instantiate(combatAssets.turnIndicatorEnemy, new Vector3(5.5f, 0.5f, 0f), Quaternion.identity);
        attackEffectOfEnemy = Instantiate(combatAssets.attackEffect, new Vector3(5.5f, -1.002905f, 0f), Quaternion.identity);
        damageIndicatorOfEnemy = Instantiate(combatAssets.damageIndicator, enemyGO.transform);
        damageIndicatorOfEnemyAnim = damageIndicatorOfEnemy.GetComponent<Animator>();
        attackEffectOfEnemyAnim = attackEffectOfEnemy.GetComponent<Animator>();
        turnIndicatorEnemy.SetActive(false);
        /*SceneManager.MoveGameObjectToScene(enemyGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(turnIndicatorEnemy, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        SceneManager.MoveGameObjectToScene(attackEffectOfEnemy, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));*/
        SceneManager.MoveGameObjectToScene(enemyGO, SceneManager.GetSceneByName("CombatLevel"));
        SceneManager.MoveGameObjectToScene(turnIndicatorEnemy, SceneManager.GetSceneByName("CombatLevel"));
        SceneManager.MoveGameObjectToScene(attackEffectOfEnemy, SceneManager.GetSceneByName("CombatLevel"));
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
        switch (enemyGO.GetComponent<Enemy>().enemyType)
        {
            case EnemyType.opossum:
                {
                    enemyGO.GetComponent<OpossumMovement>().inCombat = true;
                    break;
                }
            case EnemyType.frog:
                {
                    enemyGO.GetComponent<FrogMovement>().inCombat = true;
                    break;
                }
            case EnemyType.eagle:
                {
                    enemyGO.GetComponent<EagleMovement>().inCombat = true;
                    break;
                }
        }
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