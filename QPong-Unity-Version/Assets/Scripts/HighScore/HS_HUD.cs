using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HS_HUD : MonoBehaviour
{
    public Text firstInitial;
    public Text secondInitial;
    public Text thirdInitial;

    char[] letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-<>".ToCharArray();
    int[] index = Enumerable.Repeat(0, 3).ToArray();

    private void Awake() {

    }
    void Start()
    {
        ResetIndex();
        UpdateInitials();
    }

    void ResetIndex() {
        index = Enumerable.Repeat(0, 3).ToArray();
    }

    void UpdateInitials() {
        firstInitial.text = letter[index[0]].ToString();
        secondInitial.text = letter[index[1]].ToString();
        thirdInitial.text = letter[index[2]].ToString();
    }

    public string UserName() {
        return firstInitial.text + secondInitial.text + thirdInitial.text;
    }

    public void ChangeInitial(int position, int increment) {
        index[position] = CalculateIndex(index[position], increment);
        UpdateInitials();
    }

    int CalculateIndex(int oldIndex, int increment) {
        // Calculate new index
        var index = oldIndex + increment + letter.Length;
        // Check index value
        index = index % letter.Length;
        return index;
    }
}
