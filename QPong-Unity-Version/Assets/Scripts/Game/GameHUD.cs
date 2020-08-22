using UnityEngine;

public class GameHUD : MonoBehaviour
{

    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public float ScoreXOffset = 0.1f;
    public float ScoreYOffset = 0.005f;
    public float ScoreCenterOffset = 0.05f;
    public float WinMessageXOffset = 0.05f;
    public float WinMessageYOffset = 0.3f;
    public string playerWinMessage = "You demonstrated quantum supremacy for the first time in human history!";
    public string computerWinMessage = "Classical computer still rules the world.";
    private bool showEndMessage = false;
    private string messageToShow;

    public GUISkin layout;
    void OnGUI(){
        GUI.skin = layout;

        var labelStyle = GUI.skin.GetStyle("Label");
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = Screen.height / 15;

        var winMessageStyle = GUI.skin.GetStyle("TextField");
        winMessageStyle.alignment = TextAnchor.MiddleCenter;
        winMessageStyle.fontSize = Screen.height / 20;

        // Show scores
        GUI.Label(new Rect(Screen.width * ScoreXOffset, Screen.height*(0.5f-ScoreCenterOffset+ScoreYOffset), 
            Screen.height/20, Screen.height/20), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width * ScoreXOffset, Screen.height*(0.5f-ScoreCenterOffset-ScoreYOffset), 
            Screen.height/20, Screen.height/20), "" + PlayerScore2);

        // Show game over message and credits
        if (showEndMessage) {
            GUI.Label(new Rect(Screen.width*(WinMessageXOffset), Screen.height*(WinMessageYOffset), 
                Screen.width*(1-2*WinMessageXOffset), Screen.height*0.25f), messageToShow, winMessageStyle);
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
