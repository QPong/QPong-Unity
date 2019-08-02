using UnityEngine;

// Game enums
public enum Scenes { MainScreen = 0, Game, Levels, HowToPlay, About};

public class Definitions : MonoBehaviour {
    // Static singleton Instance
    private static Definitions instance;

    // Static singleton instance
    public static Definitions Instance
    {
        get { return instance ?? (instance = new GameObject("Definitions").AddComponent<Definitions>()); }
    }
}
