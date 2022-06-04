using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private LevelCameraLimits[] levelCameraLimitsArray = new LevelCameraLimits[2]
    {
        new LevelCameraLimits("Level1", 0f, 15f, 0f, 0f),
        new LevelCameraLimits("Level2", 0f, 30f, 20f, -30f)
    };

    // SINGLETON
    public static GameManager instance;

    public string savedSceneName;
    public Camera gameCamera;
    public Animator crossFadeTransition;
    public Animator twoSidedTransition;

    private GameObject player;
    private GameObject activeLevelHolder;
    private CombatData combatData;
    private AudioManager audioManager;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        crossFadeTransition.speed = 0f;
        combatData = CombatData.instance;
        audioManager = AudioManager.instance;
        LoadSavedScene();
    }

    public void LoadSavedScene()
    {
        AsyncOperation progress = SceneManager.LoadSceneAsync(savedSceneName, LoadSceneMode.Additive);
        progress.completed += (op) =>
        {
            gameCamera.GetComponent<CameraMovement>().FindPlayer();
            player = GameObject.Find("Player");
            crossFadeTransition.speed = 1f;
        };
    }

    public IEnumerator AdvanceToNextLevel()
    {
        crossFadeTransition.SetInteger("state", 0);
        string nextLevel = "Level" + (int.Parse(savedSceneName[5..]) + 1).ToString();
        Debug.Log(nextLevel);
        string previousLevel = savedSceneName;
        savedSceneName = nextLevel;
        StartCoroutine(audioManager.Crossfade(previousLevel, nextLevel));
        Debug.Log("hola");
        AsyncOperation progress = SceneManager.LoadSceneAsync(savedSceneName, LoadSceneMode.Additive);
        while (!progress.isDone) yield return null;
        Debug.Log("termino de lodear el level siguiente");
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(previousLevel, UnloadSceneOptions.None);
        //while (!asyncUnload.isDone) yield return null;
        //asyncUnload.completed += (op) => { Debug.Log("termino de unlodear el level anterior"); };
        gameCamera.transform.position = new Vector3(0f, 0f, -10);
        gameCamera.GetComponent<CameraMovement>().FindPlayer();
        player = GameObject.Find("Player");
        if (player) Debug.Log("gM encontro al jugador");
        crossFadeTransition.SetInteger("state", 1);
    }

    public void GoBackToPreviousLevel()
    {

    }

    public void StartAFight(GameObject enemyGO, GameObject levelHolder)
    {
        player.GetComponent<PlayerMovement>().FreezePlayer();
        enemyGO.GetComponent<EnemyMovement>().FreezeEnemy();
        combatData.SetEnemyToCombat(enemyGO.GetComponent<Enemy>());
        combatData.SetOriginScene(savedSceneName);
        combatData.SetPreviousPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        combatData.SetPreviousCameraPosition(gameCamera.transform.position.x, gameCamera.transform.position.y, -10f);
        activeLevelHolder = levelHolder;
        StartCoroutine(PassToCombatScene(activeLevelHolder));
    }

    IEnumerator PassToCombatScene(GameObject levelHolder)
    {
        twoSidedTransition.SetInteger("state", 1);
        StartCoroutine(audioManager.Crossfade(savedSceneName, "Combat" + savedSceneName));
        yield return new WaitForSeconds(1f);
        if (levelHolder != null)
        {
            levelHolder.SetActive(false);
            gameCamera.GetComponent<CameraMovement>().inCombat = true;
            gameCamera.transform.position = new Vector3(0f, 0f, -10);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Combat" + combatData.GetOriginScene(), LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        twoSidedTransition.SetInteger("state", 2);
    }

    public void ComeBackFromCombatScene()
    {
        //Set info to player, check state of enemy and destroy it when win etc (Maybe not necesary)
        StartCoroutine(UnloadCombatScene());
    }

    IEnumerator UnloadCombatScene()
    {
        twoSidedTransition.SetInteger("state", 1);
        StartCoroutine(audioManager.Crossfade("Combat" + savedSceneName, savedSceneName));
        yield return new WaitForSeconds(1f);
        //combatData.GetEnemyToCombat().DestroySelf();
        combatData.GetEnemyToCombat().gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameCamera.transform.position = combatData.GetPreviousCameraPosition();
        gameCamera.GetComponent<CameraMovement>().inCombat = false;
        //player.GetComponent<PlayerMovement>().UnfreezePlayer();
        activeLevelHolder.SetActive(true);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("Combat" + combatData.GetOriginScene(), UnloadSceneOptions.None);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        twoSidedTransition.SetInteger("state", 2);
        yield return new WaitForSeconds(1f);
        Animator enemyToCombatAnimator = combatData.GetEnemyToCombat().gameObject.GetComponent<Animator>();
        enemyToCombatAnimator.enabled = true;
        enemyToCombatAnimator.SetInteger("state", 2);
        player.GetComponent<PlayerMovement>().UnfreezePlayer(); 
    }
}
