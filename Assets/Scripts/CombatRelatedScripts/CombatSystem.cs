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
    private GameObject enemyPreFab;
    private Enemy enemyEnemyComp;
    private GameObject playerPreFab;
    private GameObject playerGO;

    private List<float> notesInput = new();
    private List<Sound[]> musicalNotes;
    private float enemyRequestDecimal;

    private CombatData combatData; private PlayerStats playerStats; private CombatAssets combatAssets;
    private GameManager gameManager; private AudioManager audioManager;

    void Start()
    {
        state = CombatState.NONE;
        combatData = CombatData.instance; playerStats = PlayerStats.instance; combatAssets = CombatAssets.instance;
        gameManager = GameManager.instance; audioManager = AudioManager.instance;
        musicalNotes = new List<Sound[]>(audioManager.musicalNotes);
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
        yield return null;
    }

    void PlayerTurn()
    {
        if (GenerateRandomOperationForEnemyRequest())
        {
            TriggerStartAnimationOfHUDElements();
        }
        timer.RunTimer();
    }

    public bool GenerateRandomOperationForEnemyRequest()
    {
        float upperLimit = 40; float lowerLimit = 5;
        int multiplicatorResult = (int) Random.Range(lowerLimit, upperLimit);
        enemyRequestDecimal = 0.125f * multiplicatorResult;
        enemyRequestText.text = ConvertToFractionString(enemyRequestDecimal);
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
        state = CombatState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerAction()
    {
        yield return new WaitForSeconds(0.3f);
        TriggerStartAnimationOfCongratsMessage();
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
        StartCoroutine(audioManager.FadeVolumeDown("CombatLevel01"));
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
        StartCoroutine(audioManager.FadeVolumeUp("CombatLevel01"));

        yield return new WaitForSeconds(0.5f);
        TriggerEndAnimationOfPlayingNotesHUD();
        playingNotesHUDImage.sprite = musicalNotesImages[4];

        //////////////////
        bool isEnemyDead = enemyEnemyComp.Numb(playerStats.damage * timer.GetTimerBonusMultiplicator());
        //playerGO.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 560);
        notesInput.Clear();

        yield return new WaitForSeconds(1.5f);

        if (isEnemyDead)
        {
            state = CombatState.WON;
            int expObtained = ((int)enemyEnemyComp.maxHealthPoints) / 2;
            wMCExpText.text = "+" + expObtained;
            winnerMessage.SetInteger("state", 1);
            backOverlay.SetInteger("state", 1);
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
            _ = playerStats.AddExpPointsAndCheck(expObtained);
            yield return new WaitForSeconds(2.6f);
            EndCombat();
        }
        else
        {
            state = CombatState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        // Make Enemy Sing
        //Damage the player
        bool isPlayerDead = playerStats.NumbPlayer(enemyEnemyComp.GetTotalDamage());
        yield return new WaitForSeconds(1f);

        //check if player is dead, actualizar estados y pasar a endcombat o playerTurn;
        if (isPlayerDead)
        {
            state = CombatState.LOST;
            EndCombat();
        }
        else
        {
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
        playerGO = Instantiate(playerPreFab, new Vector3(-5.5f, 10f, 0f), Quaternion.identity);
        SceneManager.MoveGameObjectToScene(playerGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        GameObject enemyGO = Instantiate(enemyPreFab, new Vector3(5.5f, 10f, 0f), Quaternion.identity);
        SceneManager.MoveGameObjectToScene(enemyGO, SceneManager.GetSceneByName("Combat" + combatData.GetOriginScene()));
        playerGO.GetComponent<PlayerMovement>().SetInCombat(true);
        /**/

        foreach (Sound[] sArray in musicalNotes)
        {
            foreach (Sound s in sArray)
            {
                s.source = playerGO.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
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
}