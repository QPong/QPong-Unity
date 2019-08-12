using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameHUD gameHUD;
    public int winScore = 7;
    public int showGameOverTime = 5;

    GameObject theBall;
    BallControl ballControlScript;
    GameObject theCircuitGrid;
    CircuitGridControl circuitGridControlScript;
    GameObject theClassicalPaddle;
    ComputerControls classicalPaddleControlScript;
    Player player;


    // Start is called before the first frame update
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
        ballControlScript = theBall.GetComponent<BallControl>();

        theCircuitGrid = GameObject.FindGameObjectWithTag("CircuitGrid");
        circuitGridControlScript = theCircuitGrid.GetComponent<CircuitGridControl>();

        theClassicalPaddle = GameObject.FindGameObjectWithTag("ClassicalPaddle");
        classicalPaddleControlScript = theClassicalPaddle.GetComponent<ComputerControls>();

        player = GameController.Instance.player;
        RestartGame();
    }

    public void Score(string wallID){
        if (wallID == "TopWall"){
            player.AddPointsToPlayer();
        } else {
            player.AddPointsToComputer();
        }
        gameHUD.UpdateScores();
    }

    void Update()
    {
        if (player == null) {
            player = GameController.Instance.player;
        }

        if (player.playerScore >= winScore){
            Debug.Log("Quantum computer wins");
            gameHUD.showPlayerWinMessage();
            StartCoroutine(GameOver());
        } else if (player.computerScore >= winScore){
            Debug.Log("Classical computer wins");
            gameHUD.showComputerWinMessage();
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver() {
        ballControlScript.ResetBall(-1f);
        yield return new WaitForSeconds(showGameOverTime);

        // TODO: Check high scores before to move to main menu
        GameController.Instance.LoadMainMenu();
    }


    public void RestartGame()
    {
        player.ResetScores();
        gameHUD.UpdateScores();
        ballControlScript.RestartRound(-1f);
        circuitGridControlScript.ResetCircuit();
        classicalPaddleControlScript.ResetPaddle();
    }
}
