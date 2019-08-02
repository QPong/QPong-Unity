using System.Collections.Generic;

[System.Serializable]

public class Player {
    public int playerScore { get; private set; }
    public int computerScore { get; private set; }

    public void ResetScores() {
        playerScore = 0;
        computerScore = 0;
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
}
