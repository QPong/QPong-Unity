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
    public float columnHeight = 0.01f;
    public float rowHeight = 5;
    public float xOffset = -0.03f;
    public float yOffset = -35f;

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
        //TODO: this wll probably have to change
        // set screen resolution to mini-arcade
        Screen.SetResolution(900, 1440, true);
        var width = Camera.main.orthographicSize * 8.0f * Screen.width / Screen.height; // Width of the screen
        transform.localScale = new Vector2(width/5.0f, width);
        gateArray = new string[qubitNumber * circuitDepth];
        gateObjectArray = new GameObject[qubitNumber * circuitDepth];
        circuitGridClientScript = GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>();
        measureWallScript = GameObject.Find("BottomMeasurementWall").GetComponent<MeasureWalls>();
        ResetCircuit();
        print("START CIRCUIT GRID");
        for (int i = 0; i < qubitNumber; i++)
        {
            for (int j = 0; j < circuitDepth; j++)
            {
                int index = i * circuitDepth + j;
                gateArray[index] = "I";
                gateObjectArray[index] = (GameObject) Instantiate(emptyGate, new Vector2(Screen.width*(xOffset - i*columnHeight), -64), Quaternion.identity);
                gateObjectArray[index].name = "gate["+i+"]["+j+"]";
                print("Spacing out the qubits");
            }
        }
        selectedGate = GameObject.Find("gate[0][0]");
        cursor = GameObject.Find("Cursor");

        int numberOfState = (int) Math.Pow(2, qubitNumber);
        paddleArray = new GameObject[numberOfState];
        for (int i = 0; i < numberOfState; i++)
        {
            Vector3 paddlePosition = Camera.main.ScreenToWorldPoint(new Vector3((i+0.5f)*Screen.width/numberOfState, Screen.height*0.18f,0));
            paddlePosition.z = 0f;
            paddleArray[i] = (GameObject)Instantiate(paddle, paddlePosition, Quaternion.identity);
            // set paddle width to 1/8 of screen width
            paddleArray[i].transform.localScale = new Vector2(width/8.0f, width/40.0f);
            paddleArray[i].name = "paddle1["+i+"]";

        }
    }

    // Update is called once per frame
    void Update()
    {
        var index = FindSelectedQubitIDs(selectedGate.name);
        selectedColNum = index[0];
        selectedRowNum = index[1];
        MoveCursorToSelectedQubit(selectedColNum, selectedRowNum);
    }

    public void AddGate(ArcadeButtonGates gate)
    {
        // Extract column number and row number from name
        var index = FindSelectedQubitIDs(selectedGate.name);
        selectedColNum = index[0];
        selectedRowNum = index[1];
        MoveCursorToSelectedQubit(selectedColNum, selectedRowNum);

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
        if (gate == ArcadeButtonGates.None) 
        {
            updateCircuit = true;
            gateArray[selectedIndex] = "I";
        }
        UpdateCircuit();

    }


    public void MoveCursor(JoystickButtonMaps direction)
    {
        // Extract column number and row number from name
        int[] result = FindSelectedQubitIDs(selectedGate.name);
        selectedColNum = result[0];
        selectedRowNum = result[1];

        if (direction == JoystickButtonMaps.left)
        {
            selectedColNum++;
        } else
        {
            selectedColNum--;
        }


        if (selectedColNum >= qubitNumber)
        {
            selectedColNum = qubitNumber - 1;
        }
        else if (selectedColNum < 0)
        {
            selectedColNum = 0;
        }
        MoveCursorToSelectedQubit(selectedColNum, selectedRowNum);

    }

    public void SetCursorToSelected(string selectedName)
    {
        int[] result = FindSelectedQubitIDs(selectedName);
        MoveCursorToSelectedQubit(result[0], result[1]);
    }

    // Extract column number and row number from name
    private int[] FindSelectedQubitIDs(string qubitName)
    {
        // Extract column number and row number from name
        var index = Regex.Matches(qubitName, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
        return index;
    }

    private void MoveCursorToSelectedQubit(int selectedColumnIndex, int selectedRowIndex)
    {
        selectedIndex = selectedColumnIndex * circuitDepth + selectedRowIndex;
        selectedGate = GameObject.Find("gate[" + selectedColumnIndex + "][" + selectedRowIndex + "]");
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
