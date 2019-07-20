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
    GameObject theCircuitGrid;
    GameObject theClassicalPaddle;

    // Start is called before the first frame update
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
        theCircuitGrid = GameObject.FindGameObjectWithTag("CircuitGrid");
        theClassicalPaddle = GameObject.FindGameObjectWithTag("ClassicalPaddle");
        //Output the current screen window width in the console
        Debug.Log("Screen Width : " + Screen.width);
        Debug.Log("Screen Height : " + Screen.height);
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
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        } else if (PlayerScore2 >= winScore){
            Debug.Log("Classical computer wins");
            GUI.Label(new Rect(Screen.width / 2 - 1000, Screen.height/2 - 500 + yOffsetWinMessage, 2000, 1000), 
                "Classical computer still rules the world.", centeredLabelStyle);
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
    }

    void RestartGame()
    {
        PlayerScore1 = 0;
            PlayerScore2 = 0;
            theBall.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
            theCircuitGrid.SendMessage("ResetCircuit", 0.5f, SendMessageOptions.RequireReceiver);
            theClassicalPaddle.SendMessage("ResetPaddle", 0.5f, SendMessageOptions.RequireReceiver);
    }
}
