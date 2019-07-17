#!/usr/bin/env python3
from pathlib import Path
import sys

# add project path to PYTHONPATH in order to run server.py as a script
project_path = str(Path().resolve().parent)
sys.path.append(project_path)

from flask import request
from flask import jsonify
from flask import Flask
from api import run_qasm
from api import backend_configuration
import json
import json_tricks

from model.circuit_grid_model import CircuitGridModel
from model import circuit_node_types as node_types
from controls.circuit_grid import CircuitGridNode

from qiskit import BasicAer, execute, ClassicalRegister
from utils.states import comp_basis_states
from copy import deepcopy

app = Flask(__name__)


@app.route('/')
def welcome():
    return "Hi Qiskiter!"


@app.route('/api/backend/configuration')
def backend_config():
    config = backend_configuration('qasm_simulator')
    return jsonify({"result": config})


@app.route('/api/run/qasm', methods=['POST'])
def qasm():
    qasm = request.form.get('qasm')
    api_token = None
    if request.form.get('api_token'):
        api_token = request.form.get('api_token')

    shots = 1024
    if request.form.get('shots'):
        shots = int(request.form.get('shots'))

    memory = False
    if request.form.get('memory'):
        memory = request.form.get('memory').lower() in ['true', '1']

    print("--------------")
    print(qasm)
    print(request.get_data())
    print(request.form)
    backend = 'qasm_simulator'
    output = run_qasm(qasm, backend, api_token=api_token, shots=shots, memory=memory)

    ret = {"result": output}
    return jsonify(ret)


@app.route('/api/run/get_statevector', methods=['POST'])
def get_statevector():
    gate_array_string = request.form.get('gate_array')
    print("--------------")
    print(gate_array_string)

    gate_array = gate_array_string.split(',')
    row_max = 3
    column_max = 15
    circuit_grid_model = CircuitGridModel(row_max, column_max)
    for i in range(row_max):
        for j in range(column_max):
            index = i * column_max + j
            node = CircuitGridNode(node_types.IDEN)
            if gate_array[index] == 'X':
                node = CircuitGridNode(node_types.X)
            elif gate_array[index] == 'Y':
                node = CircuitGridNode(node_types.Y)
            elif gate_array[index] == 'Z':
                node = CircuitGridNode(node_types.Z)
            elif gate_array[index] == 'H':
                node = CircuitGridNode(node_types.H)
            circuit_grid_model.set_node(i, j, node)

    circuit = circuit_grid_model.compute_circuit()
    shot_num = 1000
    backend_sv_sim = BasicAer.get_backend('statevector_simulator')
    job_sim = execute(circuit, backend_sv_sim, shots=shot_num)
    result_sim = job_sim.result()
    quantum_state = result_sim.get_statevector(circuit, decimals=3)

    return json_tricks.dumps(quantum_state)


@app.route('/api/run/do_measurement', methods=['POST'])
def do_measurement():
    gate_array_string = request.form.get('gate_array')
    print("--------------")
    print(gate_array_string)

    gate_array = gate_array_string.split(',')
    row_max = 3
    column_max = 15
    qubit_num = row_max
    circuit_grid_model = CircuitGridModel(row_max, column_max)
    for i in range(row_max):
        for j in range(column_max):
            index = i * column_max + j
            node = CircuitGridNode(node_types.IDEN)
            if gate_array[index] == 'X':
                node = CircuitGridNode(node_types.X)
            elif gate_array[index] == 'Y':
                node = CircuitGridNode(node_types.Y)
            elif gate_array[index] == 'Z':
                node = CircuitGridNode(node_types.Z)
            elif gate_array[index] == 'H':
                node = CircuitGridNode(node_types.H)
            circuit_grid_model.set_node(i, j, node)

    circuit = circuit_grid_model.compute_circuit()
    shot_num = 1

    backend_sv_sim = BasicAer.get_backend('qasm_simulator')
    cr = ClassicalRegister(qubit_num)
    measure_circuit = deepcopy(circuit)  # make a copy of circuit
    measure_circuit.add_register(cr)  # add classical registers for measurement readout
    measure_circuit.measure(measure_circuit.qregs[0], measure_circuit.cregs[0])
    job_sim = execute(measure_circuit, backend_sv_sim, shots=shot_num)
    result_sim = job_sim.result()
    counts = result_sim.get_counts(circuit)

    state_in_decimal = int(list(counts.keys())[0], 2)

    return str(state_in_decimal)


if __name__ == '__main__':
    app.run(host='127.0.0.1', port=8008)
