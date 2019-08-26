using UnityEngine;
using UnityEngine.UI;

public class RowHUD : MonoBehaviour
{
    public Text position;
    public Text initials;
    public Text score;
    public Text time;

    public void UpdateScore(HSPlayer player) {
        initials.text = player.initials;
        score.text = player.playerScore.ToString() + "-" + player.computerScore.ToString();
        time.text = player.timeScore.ToString("F1");
    }

    public void UpdateIndex(int index) {
        position.text = index.ToString();
    }
}
