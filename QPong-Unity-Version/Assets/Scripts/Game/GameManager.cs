using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameHUD gameHUD;
    public int winScore = 7;

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
            ballControlScript.ResetBall(-1f);
        } else if (player.computerScore >= winScore){
            Debug.Log("Classical computer wins");
            gameHUD.showComputerWinMessage();
            ballControlScript.ResetBall(-1f);
        }
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
