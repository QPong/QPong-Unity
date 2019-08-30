using UnityEngine;

// Game enums
public enum Scenes { MainMenu = 0, Game, Ranking, Credits, HighScore};
public enum ArcadeButtonGates { None, iz, zi, ih, hi, ix, xi, cz };
public enum JoystickButtonMaps { left, right, up, down, a, d, w, z }

public class Definitions : MonoBehaviour {
    // Static singleton Instance
    private static Definitions instance;

    // Static singleton instance
    public static Definitions Instance
    {
        get { return instance ?? (instance = new GameObject("Definitions").AddComponent<Definitions>()); }
    }
}
