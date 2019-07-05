#!/usr/bin/env python3
from flask import request
from flask import jsonify
from flask import Flask
from api import run_qasm
from api import backend_configuration
import json

from model.circuit_grid_model import CircuitGridModel
from model import circuit_node_types as node_types
from controls.circuit_grid import CircuitGridNode

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


@app.route('/api/run/gate_array', methods=['POST'])
def send_gate_array():
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

    print(circuit_grid_model.compute_circuit().qasm())
    return circuit_grid_model.compute_circuit().qasm()


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8008)
