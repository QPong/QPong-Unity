using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statevector : MonoBehaviour
{
    public GUISkin layout;
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
        GUI.Label(new Rect(100, 20, 100, 100), "|000>");
        // GUI.Label(new Rect(100, 20, 100, 100), "|001>");

        // state0.transform.rotation =Quaternion.Eulers(0,0,90.0f);

    }
}
