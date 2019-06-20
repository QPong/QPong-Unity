using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitGridControl : MonoBehaviour
{
    public GameObject emptyGate;
    public int columnMax = 3;
    public int rowMax = 4;
    public int columnHeight = 5;
    public int rowHeight = 5;
    public GameObject[] gateArray;  //1D array of gate
    public GameObject selectedGate;
    public int selectedGateIndex;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode addXGate = KeyCode.X;

    // Start is called before the first frame update
    void Start()
    {
        gateArray = new GameObject[columnMax * rowMax];
        for (int i = 0; i < columnMax; i++)
        {
            for (int j = 0; j < rowMax; j++)
            {
                int index = i + j * rowMax;
                gateArray[index] = (GameObject)Instantiate(emptyGate, new Vector2(i * columnHeight, j * rowHeight), Quaternion.identity);
            }
        }
        selectedGate = gateArray[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(moveDown)) {
            selectedGateIndex = System.Array.IndexOf(gateArray, selectedGate);
            selectedGate = gateArray[selectedGateIndex + rowMax];
            Debug.Log("Selected gate index is"+selectedGateIndex);
        }
    }
}
