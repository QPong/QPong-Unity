using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[System.Serializable]
public class HSPlayer: IEquatable<HSPlayer>, IComparable<HSPlayer> {
    public string initials;
    public int playerScore;
    public int computerScore;
    public float timeScore;

    #region Sort (IEquatable - IComparable)
    public override bool Equals(object obj) {
        if (obj == null)
            return false;
        HSPlayer objAsHSPlayer = obj as HSPlayer;
        if (objAsHSPlayer == null)
            return false;
        else
            return Equals(objAsHSPlayer);
    }

    public int CompareTo(HSPlayer other) {
        if (other == null)
            return 1;
        else if (this.playerScore != other.playerScore)
            return other.playerScore.CompareTo(this.playerScore);
        else if (this.computerScore != other.computerScore)
            return this.computerScore.CompareTo(other.computerScore);
        return this.timeScore.CompareTo(other.timeScore);
    }

    public override int GetHashCode() {
        return playerScore;
    }

    public bool Equals(HSPlayer other) {
        if (other == null)
            return false;
        else if (this.playerScore != other.playerScore)
            return (this.playerScore.Equals(other.playerScore));
        else if (this.computerScore != other.computerScore)
            return (other.computerScore.Equals(this.computerScore));
        else
            return (other.timeScore.Equals(this.timeScore));
    }
    #endregion
}

[System.Serializable]
public class Player {
    public int playerScore { get; private set; }
    public int computerScore { get; private set; }
    public float timeScore = Mathf.Infinity;
    public int rankLength = 10;

    public List<HSPlayer> playersRanking = new List<HSPlayer>();

    public void ResetScores() {
        Debug.Log("get this far");
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
        HSPlayer newScore = new HSPlayer {
            initials = initials,
            playerScore = playerScore,
            computerScore = computerScore,
            timeScore = timeScore
        };

        playersRanking.Add(newScore);
        playersRanking.Sort();

        while (playersRanking.Count > rankLength) {
            playersRanking.Remove(playersRanking.Last());
        }

        ResetScores();
        Storage.Instance.SavePlayerData(this);
    }

    public bool CheckHighScore() {
        // If rank is not full return infinite to allow any score
        if (playersRanking.Count < rankLength) return true;

        // Compare current score with worst high score
        HSPlayer newScore = new HSPlayer {
            playerScore = playerScore,
            computerScore = computerScore,
            timeScore = timeScore};

        List <HSPlayer> hs = new List <HSPlayer> {playersRanking.Last(), newScore};

        hs.Sort();
        return (hs.First() == newScore);
    }
}
