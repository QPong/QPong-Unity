using UnityEngine;

// Game enums
public enum Scenes { MainMenu = 0, Game, Ranking, Credits};

public class Definitions : MonoBehaviour {
    // Static singleton Instance
    private static Definitions instance;

    // Static singleton instance
    public static Definitions Instance
    {
        get { return instance ?? (instance = new GameObject("Definitions").AddComponent<Definitions>()); }
    }
}
