using UnityEngine;

public class GameHUD : MonoBehaviour
{

    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public int xOffset = 50;
    public int yOffset = 120;
    public int centerOffset = 70;
    public int yOffsetWinMessage = 150;
    public int yOffsetCreditString = 500;

    public string playerWinMessage = "You demonstrated quantum supremacy for the first time in human history!";
    public string computerWinMessage = "Classical computer still rules the world.";
    public string creditString = @"This game was initiated in 
IBM Qiskit Camp 2019 by 

Huang Junye
Jarrod Reilly
Anastasia Jeffery
James Weaver

Arcade version was made by 

Huang Junye
Gregory Boland
Ivan Duran
";
    private bool showEndMessage = false;
    private string messageToShow;

    public GUISkin layout;
    void OnGUI(){
        GUI.skin = layout;

        var centeredLabelStyle = GUI.skin.GetStyle("Label");
        var centeredCreditStyle = GUI.skin.GetStyle("TextField");

        centeredLabelStyle.alignment = TextAnchor.UpperCenter;
        centeredCreditStyle.alignment = TextAnchor.UpperCenter;

        // Show scores
        GUI.Label(new Rect(0 + xOffset, Screen.height/2 - centerOffset + yOffset, 100, 200), "" + PlayerScore1);
        GUI.Label(new Rect(0 + xOffset, Screen.height/2 - centerOffset - yOffset, 100, 200), "" + PlayerScore2);

        // Show game over message and credits
        if (showEndMessage) {
            GUI.Label(new Rect(Screen.width / 2 - 1000, Screen.height/2 - 500 + yOffsetWinMessage, 2000, 1000),
                messageToShow, centeredLabelStyle);
            GUI.Label(new Rect(Screen.width / 2 - 1000, Screen.height/2 - 500 + yOffsetCreditString, 2000, 1200),
                creditString, centeredCreditStyle);
        }
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
