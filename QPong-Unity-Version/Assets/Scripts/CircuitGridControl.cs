using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

public class CircuitGridControl : MonoBehaviour
{
    public GameObject emptyGate;
    public int columnMax = 15;
    public int rowMax = 3;
    public int columnHeight = 5;
    public int rowHeight = 5;
    public float xOffset = -51f;
    public float yOffset = -35f;
    public string[] gateArray; // array of string representing gates
    private GameObject[] gateObjectArray;  //1D array of gate
    public GameObject selectedGate;
    public int selectedColNum;
    public int selectedRowNum;
    public int selectedIndex;
    public GameObject cursor;

    public Sprite XGateSprite;
    public Sprite YGateSprite;
    public Sprite ZGateSprite;
    public Sprite HGateSprite;
    public Sprite emptyGateSprite;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode addXGate = KeyCode.X;
    public KeyCode addYGate = KeyCode.Y;
    public KeyCode addZGate = KeyCode.Z;
    public KeyCode addHGate = KeyCode.H;
    public KeyCode deleteGate = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        gateArray = new string[rowMax * columnMax];
        gateObjectArray = new GameObject[rowMax * columnMax];
        
        for (int i = 0; i < rowMax; i++)
        {
            for (int j = 0; j < columnMax; j++)
            {
                int index = i * columnMax + j;
                gateArray[index] = "I";
                gateObjectArray[index] = (GameObject)Instantiate(emptyGate, new Vector2(xOffset + j * columnHeight, yOffset + -i * rowHeight), 
                    Quaternion.identity);
                gateObjectArray[index].name = "gate["+i+"]["+j+"]";
            }
        }
        selectedGate = GameObject.Find("gate[0][0]");
        cursor = GameObject.Find("Cursor");
    }

    // Update is called once per frame
    void Update()
    {
        // extract column number and row number from name
        var index = Regex.Matches(selectedGate.name, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
        selectedRowNum = index[0];
        selectedColNum = index[1];

        if (Input.GetKeyDown(moveDown)) {
            selectedRowNum ++;
        } else if (Input.GetKeyDown(moveUp)) {
            selectedRowNum --;
        } else if (Input.GetKeyDown(moveRight)) {
            selectedColNum ++;
        } else if (Input.GetKeyDown(moveLeft)) {
            selectedColNum --;
        }

        if (selectedColNum >= columnMax) {
            selectedColNum = columnMax - 1;
        } else if (selectedColNum < 0) {
            selectedColNum = 0;
        }

        if (selectedRowNum >= rowMax) {
            selectedRowNum = rowMax - 1;
        } else if (selectedRowNum < 0) {
            selectedRowNum = 0;
        }

        selectedIndex = selectedRowNum * columnMax + selectedColNum;
        if (Input.GetKeyDown(addXGate)) {
            if (gateArray[selectedIndex] == "X") {
                gateArray[selectedIndex] = "I";
            } else {
                gateArray[selectedIndex] = "X";
            }
        } else if (Input.GetKeyDown(addYGate)) {
            if (gateArray[selectedIndex] == "Y") {
                gateArray[selectedIndex] = "I";
            } else {
                gateArray[selectedIndex] = "Y";
            }
        } else if (Input.GetKeyDown(addZGate)) {
            if (gateArray[selectedIndex] == "Z") {
                gateArray[selectedIndex] = "I";
            } else {
                gateArray[selectedIndex] = "Z";
            }
        } else if (Input.GetKeyDown(addHGate)) {
            if (gateArray[selectedIndex] == "H") {
                gateArray[selectedIndex] = "I";
            } else {
                gateArray[selectedIndex] = "H";
            }
        }

        // Update gateObjectArray based on changes in the gateArray
        for (int i = 0; i < rowMax; i++)
        {
            for (int j = 0; j < columnMax; j++)
            {
                int gate_index = i * columnMax + j;
                if (gateArray[gate_index] == "I"){
                    gateObjectArray[gate_index].GetComponent<SpriteRenderer>().sprite = emptyGateSprite;
                } else if (gateArray[gate_index] == "X") {
                    gateObjectArray[gate_index].GetComponent<SpriteRenderer>().sprite = XGateSprite;
                } else if (gateArray[gate_index] == "Y") {
                    gateObjectArray[gate_index].GetComponent<SpriteRenderer>().sprite = YGateSprite;
                } else if (gateArray[gate_index] == "Z") {
                    gateObjectArray[gate_index].GetComponent<SpriteRenderer>().sprite = ZGateSprite;
                } else if (gateArray[gate_index] == "H") {
                    gateObjectArray[gate_index].GetComponent<SpriteRenderer>().sprite = HGateSprite;
                }
            }
        }
        selectedGate = GameObject.Find("gate["+selectedRowNum+"]["+selectedColNum+"]");
        cursor.transform.position = selectedGate.transform.position;
    }
}
