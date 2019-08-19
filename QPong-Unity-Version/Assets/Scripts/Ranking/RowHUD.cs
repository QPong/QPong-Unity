using UnityEngine;
using UnityEngine.UI;

public class RowHUD : MonoBehaviour
{
    public Text position;
    public Text initials;
    public Text score;
    public Text time;

    public void UpdateScore(int index, HSPlayer player) {
        position.text = index.ToString();
        initials.text = player.initials;
        score.text = player.playerScore.ToString() + "-" + player.computerScore.ToString();
        time.text = player.timeScore.ToString("F1");
    }
}
