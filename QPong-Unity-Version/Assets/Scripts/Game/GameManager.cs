using UnityEngine;
using System.Collections;
using System;

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
    ArcadeAPIController arcadeAPIController;
    float startButtonPressCount = 0f;
    bool showGameOverLEDAnimation = false;


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
        player = GameController.Instance.player;
        arcadeAPIController = gameObject.GetComponent<ArcadeAPIController>();

        RestartGame();
    }

    public void Score(string wallID){
        if (wallID == "TopWall"){
            player.AddPointsToPlayer();
            arcadeAPIController.PuzzleSolved();
        } else {
            //an animation for when the computer wins a point
            player.AddPointsToComputer();
            arcadeAPIController.LostPoint();
        }
        gameHUD.UpdateScores();
        SetupArcadeEnabledBoard();
    }

    void Update()
    {
        if (player == null) {
            player = GameController.Instance.player;
        }

        if (player.playerScore >= winScore){
            gameHUD.showPlayerWinMessage();
            StartCoroutine(GameOver());
            if (showGameOverLEDAnimation == false)
            {
                showGameOverLEDAnimation = true;
                //add an animation for when the player wins
                arcadeAPIController.PuzzleSolved();
            }
        } else if (player.computerScore >= winScore){
            gameHUD.showComputerWinMessage();
            StartCoroutine(GameOver());
            if (showGameOverLEDAnimation == false)
            {
                showGameOverLEDAnimation = true;
                //add an animation for when the computer wins
                arcadeAPIController.GameLost();
            }
        }

        // Check for Arcade controls
        PollForButtonInput();
    }

    IEnumerator GameOver() {
        ballControlScript.ResetBall(-1f);
        yield return new WaitForSeconds(showGameOverTime);

        // Check high scores before to move to main menu
        player.timeScore = Time.timeSinceLevelLoad;

        if (player.CheckHighScore()) {
            GameController.Instance.ShowHighscore();
        } else {
            GameController.Instance.LoadMainMenu();
        }
    }

    public void SetupArcadeEnabledBoard()
    {
        ArcadeButtonGates[] disabledGates = { ArcadeButtonGates.cz, ArcadeButtonGates.iz, ArcadeButtonGates.hi, ArcadeButtonGates.xi, ArcadeButtonGates.zi };
        arcadeAPIController.SetupPuzzle(disabledGates);
    }

    public void RestartGame()
    {
        SetupArcadeEnabledBoard();
        player.ResetScores();
        gameHUD.UpdateScores();
        ballControlScript.RestartRound(-1f);
        classicalPaddleControlScript.ResetPaddle();
        showGameOverLEDAnimation = false;
    }

    #region Board Input
    private void PollForButtonInput()
    {

        if (Input.GetButtonDown("Start"))
        {
            //the game or maybe even close it out and go back to the app selection screen
            startButtonPressCount = Time.time;
            print("Start button press " + startButtonPressCount);

        }
        if (Input.GetButtonDown("Menu"))
        {
            print("QUIT APP!!");
            System.Diagnostics.Process.Start("osascript", "-e 'tell application \"Quantum Arcade\" to activate'");
            Application.Quit();
        }

        if (Input.GetKeyDown(JoystickButtonMaps.left.ToString()) || Input.GetKeyDown(JoystickButtonMaps.a.ToString()))
        {
            //TODO: set up the move cursor to the left
            //print("BACK");
            circuitGridControlScript.MoveCursor(JoystickButtonMaps.left);
        }
        if (Input.GetKeyDown(JoystickButtonMaps.right.ToString()) || Input.GetKeyDown(JoystickButtonMaps.d.ToString()))
        {
            //TODO: setup the move cursor to the right
            //print("FORWARD");
            circuitGridControlScript.MoveCursor(JoystickButtonMaps.right);
        }


        ArcadeButtonGates gateButtonPressed = arcadeButtonInput.isButtonPressed();
        if (gateButtonPressed != ArcadeButtonGates.None)
        {
            // print("what is gate buton " + gateButtonPressed);
            PressedGate(gateButtonPressed);
        }

        if (Input.GetButtonUp("Start"))
        {

            startButtonPressCount = Math.Abs(Time.time - startButtonPressCount);

            print("time down " + startButtonPressCount);

            if (startButtonPressCount >= 3.0f) 
            {
                print("QUIT APP!!");
                startButtonPressCount = 0f;

                System.Diagnostics.Process.Start("osascript", "-e 'tell application \"AppMenu\" to activate'");
                Application.Quit();
            }
            startButtonPressCount = 0f;
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }


    public void PressedGate(ArcadeButtonGates gateName)
    {

        //NOTE: animate when the button is pressed
        arcadeAPIController.ButtonPressed(gateName);
        switch (gateName)
        {
            case ArcadeButtonGates.xi:
                print("XI");
                circuitGridControlScript.AddGate(gateName);
                break;
            case ArcadeButtonGates.hi:
                print("HI");
                circuitGridControlScript.AddGate(gateName);
                break;
            default:
                print("we no use these yet");
                break;
        }
    }
    #endregion
}
