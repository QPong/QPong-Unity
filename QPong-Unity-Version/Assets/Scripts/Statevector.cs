using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statevector : MonoBehaviour
{
    public GUISkin layout;
    public float xOffset = 30;
    public float yOffset = 0.84f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI(){
        GUI.skin = layout;
        GUI.Label(new Rect(xOffset + Screen.width * 0/8, Screen.height * yOffset, 300, 200), "|000>");
        GUI.Label(new Rect(xOffset + Screen.width * 1/8, Screen.height * yOffset, 300, 200), "|001>");
        GUI.Label(new Rect(xOffset + Screen.width * 2/8, Screen.height * yOffset, 300, 200), "|010>");
        GUI.Label(new Rect(xOffset + Screen.width * 3/8, Screen.height * yOffset, 300, 200), "|011>");
        GUI.Label(new Rect(xOffset + Screen.width * 4/8, Screen.height * yOffset, 300, 200), "|100>");
        GUI.Label(new Rect(xOffset + Screen.width * 5/8, Screen.height * yOffset, 300, 200), "|101>");
        GUI.Label(new Rect(xOffset + Screen.width * 6/8, Screen.height * yOffset, 300, 200), "|110>");
        GUI.Label(new Rect(xOffset + Screen.width * 7/8, Screen.height * yOffset, 300, 200), "|111>");
    }
}
