# QPong

This is a quantum version of classic game Pong using IBM [Qikist](https://github.com/Qiskit/qiskit) and Unity. This game was initiated in IBM Qiskit Camp 2019 by [Huang Junye](https://github.com/HuangJunye), Jarrod Reilly, Anastasia Jeffery and [James Weaver](https://github.com/JavaFXpert). (The original python version can be found [here](https://github.com/HuangJunye/QPong))  This port over to Unity is being done by [Huang Junye](https://github.com/HuangJunye), [Gregory Boland](https://github.com/boland25), & [Ivan Duran](https://github.com/filemaster).

## Story
In the dawn of the Quantum Era, a primitive 3-qubit Quantum Computer is trying to challenge the Classical Computer, the long-time ruler of the Computer Empire. Your mission is to use your Human Intelligence to help the Quantum Computer defeat the Classical Computer and demonstrate "quantum supremacy" for the first time in human history. The battle field of the Quantum-Classic war is none other than the classic Pong game.

## Installation
To compile the game, you will need to install Python and three required packages. To do that, you need to use command line tool.

The python libraries that you need to use are `qiskit`, `flask`, and `json_tricks`. To install these python libraries, `cd` to `QPong-Python-Server` and run 

    pip install -r requirements.txt

The Python server lives in a subfolder of the project.  `cd` to `QPong-Python-Server/flask_server` and run 

    python3 server.py
    
    
To compile the visual part of the game, you will need Unity version at least 2019.1.8f1.

import the project into Unity from `QPong-Unity-Version`.

If your server is working correctly you will see a paddle at the bottom of the screen, and the game will start.


To play the game, you add gates to the input area at the bottom of the screen.  Right now we only allow 2 gates.  X & H.  To move around the controls at the bottom, you use your keypad, or soon JOYSTICK!



