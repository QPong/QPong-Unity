using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Static singleton Instance
    private static GameController instance;

    // Static singleton instance
    public static GameController Instance
    {
        get { return instance ?? (instance = new GameObject("GameController").AddComponent<GameController>()); }
    }

    // Global vars
    [HideInInspector]
    public Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    private void OnDestroy()
    {
        player.ResetScores();
        SceneManager.activeSceneChanged -= LevelWasLoaded;
    }

    // Use this for initialization
    private void Start () {
        player = GetComponent<Player>();
	}

    public void OpenScene (Scenes scene) {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LevelWasLoaded(Scene previousScene, Scene newScene)
    {
        // Check if exit from game
        if (previousScene.name == Scenes.Game.ToString()) {

        }
    }
}
