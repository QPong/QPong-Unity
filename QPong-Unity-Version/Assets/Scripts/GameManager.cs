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

    public GUISkin layout;

    GameObject theBall;

    // Start is called before the first frame update
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
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

        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 60, 120, 50), "RESTART", centeredButtonStyle)){
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            theBall.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if (PlayerScore1 >= winScore){
            Debug.Log("PLAYER ONE WINS");
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        } else if (PlayerScore2 >= winScore){
            Debug.Log("PLAYER TWO WINS");
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
    }
}
