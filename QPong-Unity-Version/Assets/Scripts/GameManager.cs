using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public int xOffset = 50;
    public int yOffset = 120;
    public int centerOffset = 70;
    public int winScore = 7;
    public int yOffsetWinMessage = 150;

    public GUISkin layout;

    GameObject theBall;
    BallControl ballControlScript;
    GameObject theCircuitGrid;
    CircuitGridControl circuitGridControlScript;
    GameObject theClassicalPaddle;
    ComputerControls classicalPaddleControlScript;

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

    public static void Score(string wallID){
        if (wallID == "TopWall"){
            PlayerScore1++;
        } else {
            PlayerScore2++;
        }
    }

    void OnGUI(){
        GUI.skin = layout;

        var centeredLabelStyle = GUI.skin.GetStyle("Label");
        var centeredButtonStyle = GUI.skin.GetStyle("Button");
        centeredLabelStyle.alignment = TextAnchor.UpperCenter;
        centeredButtonStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(0 + xOffset, Screen.height/2 - centerOffset + yOffset, 100, 200), "" + PlayerScore1);
        GUI.Label(new Rect(0 + xOffset, Screen.height/2 - centerOffset - yOffset, 100, 200), "" + PlayerScore2);

        if (GUI.Button(new Rect(Screen.width / 2 - 200, 50, 400, 150), "RESTART", centeredButtonStyle)){
            RestartGame();
        }

        if (PlayerScore1 >= winScore){
            Debug.Log("Quantum computer wins");
            GUI.Label(new Rect(Screen.width / 2 - 1000, Screen.height/2 - 500 + yOffsetWinMessage, 2000, 1000), 
                "You demonstrated quantum supremacy for the first time in huaman history!",
                    centeredLabelStyle);
            ballControlScript.ResetBall(-1f);
        } else if (PlayerScore2 >= winScore){
            Debug.Log("Classical computer wins");
            GUI.Label(new Rect(Screen.width / 2 - 1000, Screen.height/2 - 500 + yOffsetWinMessage, 2000, 1000), 
                "Classical computer still rules the world.", centeredLabelStyle);
            ballControlScript.ResetBall(-1f);
        }
    }

    void RestartGame()
    {
        PlayerScore1 = 0;
        PlayerScore2 = 0;
        ballControlScript.RestartRound(-1f);
        circuitGridControlScript.ResetCircuit();
        classicalPaddleControlScript.ResetPaddle();
    }
}
