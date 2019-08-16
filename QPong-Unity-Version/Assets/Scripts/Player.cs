using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HSPlayer {
    public string initials;
    public int playerScore;
    public int computerScore;
    public float timeScore;

}

[System.Serializable]
public class Player {
    public int playerScore { get; private set; }
    public int computerScore { get; private set; }
    public float timeScore = Mathf.Infinity;
    public int rankLength = 10;

    private List<HSPlayer> playersRanking = new List<HSPlayer>();

    public void ResetScores() {
        playerScore = 0;
        computerScore = 0;
        timeScore = Mathf.Infinity;
    }

    public void AddPointsToPlayer(int points=1){
        playerScore += points;
    }

    public void AddPointsToComputer(int points=1){
        computerScore += points;
    }

    private void SavePlayer() {
        Storage.Instance.SavePlayerData(this);
    }

    public void StoreNewHighScore(string initials) {
        HSPlayer newScore = new HSPlayer();
        newScore.initials = initials;
        newScore.playerScore = playerScore;
        newScore.computerScore = computerScore;
        newScore.timeScore = timeScore;

        playersRanking.Add(newScore);

        playersRanking = playersRanking.OrderBy(x => x.timeScore).ToList();

        while (playersRanking.Count > rankLength) {
            playersRanking.Remove(playersRanking.Last());
        }
    }

    public float WorstScoreInRanking() {
        // If rank is not full return infinite to allow any score
        if (playersRanking.Count < rankLength) return Mathf.Infinity;
        return playersRanking.Last().timeScore;
    }
}
