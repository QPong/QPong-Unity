using System.Collections;
using System.Collections.Generic;
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
    private GameObject[] gateArray;  //1D array of gate
    public GameObject selectedGate;
    public int selectedColNum;
    public int selectedRowNum;
    public GameObject cursor;

    public Sprite XGateSprite;
    public Sprite YGateSprite;
    public Sprite ZGateSprite;
    public Sprite HGateSprite;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode addXGate = KeyCode.X;
    public KeyCode addYGate = KeyCode.Y;
    public KeyCode addZGate = KeyCode.Z;
    public KeyCode addHGate = KeyCode.H;

    // Start is called before the first frame update
    void Start()
    {
        gateArray = new GameObject[columnMax * rowMax];
        for (int i = 0; i < columnMax; i++)
        {
            for (int j = 0; j < rowMax; j++)
            {
                int index = i + j * rowMax;
                gateArray[index] = (GameObject)Instantiate(emptyGate, new Vector2(xOffset + j * rowHeight, yOffset + i * columnHeight), 
                    Quaternion.Euler(new Vector3(0f, 0f, 90f)));  // initiate gates with 90 degrees rotation
                gateArray[index].name = "gate["+i+"]["+j+"]";
            }
        }
        selectedGate = GameObject.Find("gate[0][0]");
        cursor = GameObject.Find("Cursor");
    }

    // Update is called once per frame
    void Update()
    {
        selectedColNum = (int) System.Char.GetNumericValue(selectedGate.name[5]);
        selectedRowNum = (int) System.Char.GetNumericValue(selectedGate.name[8]);

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

        if (Input.GetKeyDown(addXGate)) {
            selectedGate.GetComponent<SpriteRenderer>().sprite = XGateSprite;
        } else if (Input.GetKeyDown(addYGate)) {
            selectedGate.GetComponent<SpriteRenderer>().sprite = YGateSprite;
        } else if (Input.GetKeyDown(addZGate)) {
            selectedGate.GetComponent<SpriteRenderer>().sprite = ZGateSprite;
        } else if (Input.GetKeyDown(addHGate)) {
            selectedGate.GetComponent<SpriteRenderer>().sprite = HGateSprite;
        }

        selectedGate = GameObject.Find("gate["+selectedColNum+"]["+selectedRowNum+"]");
        cursor.transform.position = selectedGate.transform.position;
    }
}
