using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{

    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public int xOffset = 50;
    public int yOffset = 120;
    public int centerOffset = 70;
    public int winScore = 7;
    public int yOffsetWinMessage = 150;

    public string playerWinMessage = "You demonstrated quantum supremacy for the first time in human history!";
    public string computerWinMessage = "Classical computer still rules the world.";
    private bool showEndMessage = false;
    private string messageToShow;
    // private GUIStyle centeredLabelStyle = GUI.skin.GetStyle("Label");
    // private GUIStyle centeredButtonStyle = GUI.skin.GetStyle("Button");

    public GUISkin layout;
    void OnGUI(){
        GUI.skin = layout;

        var centeredLabelStyle = GUI.skin.GetStyle("Label");
        var centeredButtonStyle = GUI.skin.GetStyle("Button");

        centeredLabelStyle.alignment = TextAnchor.UpperCenter;
        centeredButtonStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(0 + xOffset, Screen.height/2 - centerOffset + yOffset, 100, 200), "" + PlayerScore1);
        GUI.Label(new Rect(0 + xOffset, Screen.height/2 - centerOffset - yOffset, 100, 200), "" + PlayerScore2);

        if (showEndMessage) {
            GUI.Label(new Rect(Screen.width / 2 - 1000, Screen.height/2 - 500 + yOffsetWinMessage, 2000, 1000),
                messageToShow, centeredLabelStyle);
        }

        // Are we going to allow the user to restart the game.
        /*
        if (GUI.Button(new Rect(Screen.width / 2 - 200, 50, 400, 150), "RESTART", centeredButtonStyle)){

            //GameController.RestartGame();
        }
         */
    }

    public void showPlayerWinMessage() {
        messageToShow = playerWinMessage;
        showEndMessage = true;
    }
    public void showComputerWinMessage() {
        messageToShow = computerWinMessage;
        showEndMessage = true;
    }

    public void removeWinMessage() {
        showEndMessage = false;
    }

    public void UpdateScores() {
        PlayerScore1 = GameController.Instance.player.playerScore;
        PlayerScore2 = GameController.Instance.player.computerScore;
    }
}
