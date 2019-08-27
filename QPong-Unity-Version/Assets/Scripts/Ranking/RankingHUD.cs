using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingHUD : MonoBehaviour
    {
    public RowHUD[] row;
    List<HSPlayer> playerRanking;

    private void Start() {
        playerRanking = GameController.Instance.player.playersRanking;
        UpdateTableIndex();
        UpdateScores();
    }

    void UpdateTableIndex() {
        for (int index = 0; index < row.Length; index++)
        {
            row[index].UpdateIndex(index + 1);
        }
    }

    void UpdateScores() {
        int index = 0;
        foreach (HSPlayer player in playerRanking)
        {
            row[index].UpdateScore(player);
            index ++;
        }
    }
}
