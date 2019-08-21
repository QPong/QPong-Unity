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
    ArcadeButtonInput arcadeButtonInput;


    // Start is called before the first frame update
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
        ballControlScript = theBall.GetComponent<BallControl>();

        theCircuitGrid = GameObject.FindGameObjectWithTag("CircuitGrid");
        circuitGridControlScript = theCircuitGrid.GetComponent<CircuitGridControl>();

        theClassicalPaddle = GameObject.FindGameObjectWithTag("ClassicalPaddle");
        classicalPaddleControlScript = theClassicalPaddle.GetComponent<ComputerControls>();

        arcadeButtonInput = gameObject.GetComponent<ArcadeButtonInput>();
        print("arcade buton input " + arcadeButtonInput);
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
        print("Update while game is playing");

        // Check for Arcade controls
        PollForButtonInput();
    }

    IEnumerator GameOver() {
        ballControlScript.ResetBall(-1f);
        yield return new WaitForSeconds(showGameOverTime);

        // TODO: Check high scores before to move to main menu
        if (player.WorstScoreInRanking() > Time.timeSinceLevelLoad) {
            player.timeScore = Time.timeSinceLevelLoad;
            GameController.Instance.ShowHighscore();
        } else {
            GameController.Instance.LoadMainMenu();
        }
    }

    public void RestartGame()
    {
        player.ResetScores();
        gameHUD.UpdateScores();
        ballControlScript.RestartRound(-1f);
        classicalPaddleControlScript.ResetPaddle();
    }

    #region Board Input
    private void PollForButtonInput()
    {

        if (Input.GetButtonDown("Start"))
        {
                //TODO: this is where we can go to reset the game or maybe even close it out and go back to the app selection screen
            
        }
        if (Input.GetKeyDown(JoystickButtonMaps.left.ToString()) || Input.GetKeyDown(JoystickButtonMaps.a.ToString()))
        {
            //TODO: set up the move cursor to the left
            print("BACK");
            circuitGridControlScript.MoveCursor(JoystickButtonMaps.left);
        }
        if (Input.GetKeyDown(JoystickButtonMaps.right.ToString()) || Input.GetKeyDown(JoystickButtonMaps.d.ToString()))
        {
            //TODO: setup the move cursor to the right
            print("FORWARD");
            circuitGridControlScript.MoveCursor(JoystickButtonMaps.right);
        }

        ArcadeButtonGates gateButtonPressed = arcadeButtonInput.isButtonPressed();
        if (gateButtonPressed != ArcadeButtonGates.None) 
        {
            // print("what is gate buton " + gateButtonPressed);
            PressedGate(gateButtonPressed);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }


    public void PressedGate(ArcadeButtonGates gateName)
    {
       
        //NOTE: my next step is to implement this
        //arcadeButtonController.ButtonPressed(gateName);
        switch (gateName)
        {
            case ArcadeButtonGates.xi:
                print("XI");
                break;
            case ArcadeButtonGates.hi:
                print("HI");
                break;
            default:
                print("we no use these yet");
                break;
        }
    }
    #endregion
}
