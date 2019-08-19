using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingHUD : MonoBehaviour
    {
    public RowHUD[] row;
    List<HSPlayer> playerRanking;

    private void Start() {
        playerRanking = GameController.Instance.player.playersRanking;
        UpdateScores();
    }

    void UpdateScores() {
        int index = 0;
        foreach (HSPlayer player in playerRanking)
        {
            row[index].UpdateScore(index, player);
            index ++;
        }
    }
}
