using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private LevelCameraLimits[] levelCameraLimitsArray = new LevelCameraLimits[3]
    {
        new LevelCameraLimits("Level1", 0f, 18f, 0f, 0f, new Vector3(0f, 0f, 0f)),
        new LevelCameraLimits("Level2", -2.5f, 60.5f, -30f, 20f, new Vector3(0f, 1f, 0f)),
        new LevelCameraLimits("Level3", -2.5f, 51.5f, -30f, 20f, new Vector3(0f, 1f, 0f))
    };

    private LevelPlayerSpawnPoints[] levelPlayerSpawnPointsArray = new LevelPlayerSpawnPoints[3]
    {
        new LevelPlayerSpawnPoints("level1", new Vector3(-2.3f, -1f, 0f), new Vector3(21.7f, -1f, 0f)),
        new LevelPlayerSpawnPoints("level2", new Vector3(-6.5f, -1f, 0f), new Vector3(64.5f, -7f, 0f)),
        new LevelPlayerSpawnPoints("level3", new Vector3(-6.5f, -1f, 0f), new Vector3(55.3f, -1f, 0f))
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
            audioManager.Play(savedSceneName);
            int actualLevelNumber = int.Parse(savedSceneName[5..]);
            gameCamera.GetComponent<CameraMovement>().FindPlayer();
            gameCamera.GetComponent<CameraMovement>().SetCameraLimits(levelCameraLimitsArray[actualLevelNumber-1]);
            player = GameObject.Find("Player");
            crossFadeTransition.speed = 1f;
        };
    }

    public IEnumerator AdvanceToNextLevel()
    {
        crossFadeTransition.SetInteger("state", 0);
        yield return new WaitForSeconds(0.8f);
        int actualLevelNumber = int.Parse(savedSceneName[5..]);
        string nextLevel = "Level" + (actualLevelNumber + 1);
        string actualLevel = savedSceneName;
        savedSceneName = nextLevel;
        StartCoroutine(audioManager.Crossfade(actualLevel, nextLevel));
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(actualLevel, UnloadSceneOptions.None);
        AsyncOperation progress = SceneManager.LoadSceneAsync(savedSceneName, LoadSceneMode.Additive);
        progress.completed += (op) => 
        {
            gameCamera.GetComponent<CameraMovement>().FindPlayer();
            gameCamera.GetComponent<CameraMovement>().SetCameraLimits(levelCameraLimitsArray[actualLevelNumber]);
            player = GameObject.Find("Player");
            player.GetComponent<PlayerMovement>().ActivateAndMovePlayerOnLevelPass
            (
                levelPlayerSpawnPointsArray[actualLevelNumber].leftSpawnPoint,
                Quaternion.Euler(0,0,0)
            );
            crossFadeTransition.SetInteger("state", 1);
        };
    }

    public IEnumerator GoBackToPreviousLevel()
    {
        crossFadeTransition.SetInteger("state", 0);
        yield return new WaitForSeconds(0.8f);
        int actualLevelNumber = int.Parse(savedSceneName[5..]);
        string previousLevel = "Level" + (actualLevelNumber + -1);
        string actualLevel = savedSceneName;
        savedSceneName = previousLevel;
        StartCoroutine(audioManager.Crossfade(actualLevel, previousLevel));
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(actualLevel, UnloadSceneOptions.None);
        AsyncOperation progress = SceneManager.LoadSceneAsync(savedSceneName, LoadSceneMode.Additive);
        progress.completed += (op) =>
        {
            gameCamera.GetComponent<CameraMovement>().FindPlayer();
            gameCamera.GetComponent<CameraMovement>().SetCameraLimits(levelCameraLimitsArray[actualLevelNumber - 2]);
            player = GameObject.Find("Player");
            player.GetComponent<PlayerMovement>().ActivateAndMovePlayerOnLevelPass
            (
                levelPlayerSpawnPointsArray[actualLevelNumber-2].rightSpawnPoint,
                Quaternion.Euler(0, 180, 0)
            );
            crossFadeTransition.SetInteger("state", 1);
        };
    }

    public void StartAFight(GameObject enemyGO, GameObject levelHolder)
    {
        player.GetComponent<PlayerMovement>().FreezePlayer();
        Enemy enemyCompTemp = enemyGO.GetComponent<Enemy>();
        switch (enemyCompTemp.enemyType)
        {
            case EnemyType.opossum: 
                {
                    enemyGO.GetComponent<OpossumMovement>().FreezeEnemy();
                    break;
                }
            case EnemyType.frog:
                {
                    enemyGO.GetComponent<FrogMovement>().FreezeEnemy();
                    break;
                }
            case EnemyType.eagle:
                {
                    enemyGO.GetComponent<EagleMovement>().FreezeEnemy();
                    break;
                }
        }
        combatData.SetEnemyToCombat(enemyCompTemp);
        combatData.SetOriginScene(savedSceneName);
        combatData.SetPreviousPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        combatData.SetPreviousCameraPosition(gameCamera.transform.position.x, gameCamera.transform.position.y, -10f);
        activeLevelHolder = levelHolder;
        StartCoroutine(PassToCombatScene());
    }

    IEnumerator PassToCombatScene()
    {
        twoSidedTransition.SetInteger("state", 1);
        StartCoroutine(audioManager.Crossfade(savedSceneName, "Combat" + savedSceneName));
        yield return new WaitForSeconds(1f);
        if (activeLevelHolder != null)
        {
            activeLevelHolder.SetActive(false);
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
        combatData.GetEnemyToCombat().MakeColliderTrigger();
        gameCamera.transform.position = combatData.GetPreviousCameraPosition();
        gameCamera.GetComponent<CameraMovement>().inCombat = false;
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
        enemyToCombatAnimator.SetTrigger("death");
        player.GetComponent<PlayerMovement>().UnfreezePlayer(); 
    }
}
