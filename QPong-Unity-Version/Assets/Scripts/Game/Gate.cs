using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public GameObject gateIcon;
    public GameObject background;
    private GameManager gameManager;
    private readonly ArcadeButtonGates[] gateStates = { ArcadeButtonGates.None, ArcadeButtonGates.xi, ArcadeButtonGates.hi };
    private int currentGate = 0;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BoxCollider2D gateCollider = GetComponent<BoxCollider2D>();
            if (gateCollider.OverlapPoint(wp))
            {
                Debug.Log("GATE IS PRESSED " + name);
                PressedGate();
            }
        }
    }

    public void SetGateIcon(Sprite icon)
    {
        gateIcon.GetComponent<SpriteRenderer>().sprite = icon;
    }

    private void PressedGate()
    {
        currentGate += 1;
        if (currentGate > 2) {
            currentGate = 0;
        }
        ArcadeButtonGates currGate = gateStates[currentGate];
        print("Gate To be SHOWN " + currGate);
        gameManager.TouchedGate(currGate, name);
    }


}
