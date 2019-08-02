using System.Collections;
using System.Collections.Generic;
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
        player = GameController.Instance.player;
        theBall = GameObject.FindGameObjectWithTag("Ball");
        ballControlScript = theBall.GetComponent<BallControl>();

        theCircuitGrid = GameObject.FindGameObjectWithTag("CircuitGrid");
        circuitGridControlScript = theCircuitGrid.GetComponent<CircuitGridControl>();

        theClassicalPaddle = GameObject.FindGameObjectWithTag("ClassicalPaddle");
        classicalPaddleControlScript = theClassicalPaddle.GetComponent<ComputerControls>();
    }

    public static void Score(string wallID){
        if (wallID == "TopWall"){
            GameController.Instance.player.AddPointsToPlayer();
        } else {
            GameController.Instance.player.AddPointsToComputer();
        }
    }

    void update()
    {
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
        ballControlScript.RestartRound(-1f);
        circuitGridControlScript.ResetCircuit();
        classicalPaddleControlScript.ResetPaddle();
    }
}
