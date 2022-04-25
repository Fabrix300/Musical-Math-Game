using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager instance;

    public string savedSceneName = "Level01";
    public Camera gameCamera;

    private GameObject player;
    private CombatData combatData;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadSavedScene();
        combatData = CombatData.instance;
    }

    public void LoadSavedScene()
    {
        AsyncOperation progress = SceneManager.LoadSceneAsync(savedSceneName, LoadSceneMode.Additive);
        progress.completed += (op) =>
        {
            gameCamera.GetComponent<CameraMovement>().FindPlayer();
            player = GameObject.Find("Player");
        };
    }

    public void StartAFight(GameObject enemyGO, GameObject levelHolder)
    {
        player.GetComponent<PlayerMovement>().FreezePlayer();
        enemyGO.GetComponent<EnemyMovement>().FreezeEnemy();
        combatData.SetEnemyToCombat(enemyGO.GetComponent<Enemy>());
        combatData.SetOriginScene(savedSceneName);
        combatData.SetPreviousPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        combatData.SetPreviousCameraPosition(gameCamera.transform.position.x, gameCamera.transform.position.y, gameCamera.transform.position.z);
        StartCoroutine(PassToCombatScene(levelHolder));
    }

    IEnumerator PassToCombatScene(GameObject levelHolder)
    {
        yield return new WaitForSeconds(2);
        if (levelHolder != null)
        {
            levelHolder.SetActive(false);
            gameCamera.GetComponent<CameraMovement>().inCombat = true;
            gameCamera.transform.position = new Vector3(0f, 0f, -10);
        }
        SceneManager.LoadSceneAsync("Combat" + combatData.GetOriginScene(), LoadSceneMode.Additive);
    }
}
