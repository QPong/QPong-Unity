using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

public class CircuitGridControl : MonoBehaviour
{
    public GameObject emptyGate;
    public GameObject paddle;
    public GameObject[] paddleArray;
    public int circuitDepth = 3;
    public int qubitNumber = 3;
    public int columnHeight = 5;
    public int rowHeight = 5;
    public float xOffset = -51f;
    public float yOffset = -35f;
    public float spacer = 10f;

    // Variables for gate array
    public string[] gateArray; // array of string representing gates
    private GameObject[] gateObjectArray;  //1D array of gate
    public GameObject selectedGate;
    public int selectedRowNum;
    public int selectedColNum;
    public int selectedIndex;
    public GameObject cursor;
    public bool updateCircuit;

    // Variables for gate sprites
    public Sprite XGateSprite;
    public Sprite HGateSprite;
    public Sprite emptyGateSprite;

    // Variables for inputs
    public KeyCode addXGate = KeyCode.X;
    public KeyCode addHGate = KeyCode.H;
    public KeyCode deleteGate = KeyCode.Space;
    CircuitGridClient circuitGridClientScript;
    MeasureWalls measureWallScript;

    // Start is called before the first frame update
    void Start()
    {
        gateArray = new string[qubitNumber * circuitDepth];
        gateObjectArray = new GameObject[qubitNumber * circuitDepth];
        circuitGridClientScript = GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>();
        measureWallScript = GameObject.Find("BottomMeasurementWall").GetComponent<MeasureWalls>();
        ResetCircuit();
        for (int i = 0; i < qubitNumber; i++)
        {
            for (int j = 0; j < circuitDepth; j++)
            {
                int index = i * circuitDepth + j;
                gateArray[index] = "I";
                //NOTE: this placement is for the large arcade screen, if the screen changes size, this will probably have to be updated
                gateObjectArray[index] = (GameObject) Instantiate(emptyGate, new Vector2((xOffset + i * columnHeight)+spacer, -64), Quaternion.identity);
                gateObjectArray[index].name = "gate["+i+"]["+j+"]";
            }
            spacer += 6;
        }
        selectedGate = GameObject.Find("gate[0][0]");
        cursor = GameObject.Find("Cursor");

        int numberOfState = (int) Math.Pow(2, qubitNumber);
        paddleArray = new GameObject[numberOfState];
        var width = Camera.main.orthographicSize * 8.0f * Screen.width / Screen.height; // Width of the screen
        for (int i = 0; i < numberOfState; i++)
        {
            Vector3 paddlePosition = Camera.main.ScreenToWorldPoint(new Vector3((i+0.5f)*Screen.width/numberOfState, Screen.height*0.18f,0));
            paddlePosition.z = 0f;
            paddleArray[i] = (GameObject)Instantiate(paddle, paddlePosition, Quaternion.identity);
            paddleArray[i].transform.localScale = new Vector2(width/8.0f, width/40.0f);
            paddleArray[i].name = "paddle1["+i+"]";

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Extract column number and row number from name
        var index = Regex.Matches(selectedGate.name, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
        selectedColNum = index[0];
        selectedRowNum = index[1];



        selectedIndex = selectedColNum * circuitDepth + selectedRowNum;
        selectedGate = GameObject.Find("gate[" + selectedColNum + "][" + selectedRowNum + "]");
        cursor.transform.position = selectedGate.transform.position;
    }

    public void AddGate(ArcadeButtonGates gate)
    {
        // Extract column number and row number from name
        var index = Regex.Matches(selectedGate.name, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
        selectedColNum = index[0];
        selectedRowNum = index[1];

        selectedIndex = selectedColNum * circuitDepth + selectedRowNum;
        selectedGate = GameObject.Find("gate[" + selectedColNum + "][" + selectedRowNum + "]");
        cursor.transform.position = selectedGate.transform.position;

        if (gate == ArcadeButtonGates.xi)
        {
            updateCircuit = true;
            if (gateArray[selectedIndex] == "X")
            {
                gateArray[selectedIndex] = "I";
            }
            else
            {
                gateArray[selectedIndex] = "X";
            }
        }
        if (gate == ArcadeButtonGates.hi)
        {
            updateCircuit = true;
            if (gateArray[selectedIndex] == "H")
            {
                gateArray[selectedIndex] = "I";
            }
            else
            {
                gateArray[selectedIndex] = "H";
            }
        }
        UpdateCircuit();

    }


    public void MoveCursor(JoystickButtonMaps direction)
    {
        // Extract column number and row number from name
        var index = Regex.Matches(selectedGate.name, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
        selectedColNum = index[0];
        selectedRowNum = index[1];

        if (direction == JoystickButtonMaps.left)
        {
            selectedColNum--;
        } else
        {
            selectedColNum++;
        }


        if (selectedColNum >= qubitNumber)
        {
            selectedColNum = qubitNumber - 1;
        }
        else if (selectedColNum < 0)
        {
            selectedColNum = 0;
        }
        selectedIndex = selectedColNum * circuitDepth + selectedRowNum;
        selectedGate = GameObject.Find("gate[" + selectedColNum + "][" + selectedRowNum + "]");
        cursor.transform.position = selectedGate.transform.position;

    }

    void UpdateCircuit()
    {
        if (updateCircuit)
        {
            updateCircuit = false;
            measureWallScript.updateCircuit = true;
            circuitGridClientScript.getStatevectorFlag = true;
            for (int i = 0; i < qubitNumber; i++)
            {
                for (int j = 0; j < circuitDepth; j++)
                {
                    int gate_index = i * circuitDepth + j;
                    if (gateArray[gate_index] == "I")
                    {
                        gateObjectArray[gate_index].GetComponent<Gate>().SetGateIcon(emptyGateSprite);
                    }
                    else if (gateArray[gate_index] == "X")
                    {
                        gateObjectArray[gate_index].GetComponent<Gate>().SetGateIcon(XGateSprite);
                    }
                    else if (gateArray[gate_index] == "H")
                    {
                        gateObjectArray[gate_index].GetComponent<Gate>().SetGateIcon(HGateSprite);
                    }
                }
            }
        }
    }

    void ResetCircuit()
    {
        for (int i = 0; i < qubitNumber; i++)
        {
            for (int j = 0; j < circuitDepth; j++)
            {
                int index = i * circuitDepth + j;
                gateArray[index] = "I";
            }
        }
        updateCircuit = true;
    }
}
