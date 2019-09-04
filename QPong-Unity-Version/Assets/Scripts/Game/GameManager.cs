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
    float startButtonPressCount = 0f;


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
            gameHUD.showPlayerWinMessage();
            StartCoroutine(GameOver());
        } else if (player.computerScore >= winScore){
            gameHUD.showComputerWinMessage();
            StartCoroutine(GameOver());
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
            //the game or maybe even close it out and go back to the app selection screen
            startButtonPressCount = Time.time;
            print("Start button press " + startButtonPressCount);

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

            startButtonPressCount = Time.time - startButtonPressCount;

            print("time down " + startButtonPressCount);
            if (startButtonPressCount > 2.0) 
            {
                startButtonPressCount = 0f;
                print("QUIT APP!!");
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

        //NOTE: my next step is to implement this
        //arcadeButtonController.ButtonPressed(gateName);
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
