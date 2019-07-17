using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public GameObject gateIcon;
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGateIcon(Sprite icon)
    {
        gateIcon.GetComponent<SpriteRenderer>().sprite = icon;
    }
}
