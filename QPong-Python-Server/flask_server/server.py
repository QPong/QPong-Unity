#!/usr/bin/env python3
from pathlib import Path
import sys

# add project path to PYTHONPATH in order to run server.py as a script
project_path = str(Path().resolve().parent)
sys.path.append(project_path)

from flask import request
from flask import jsonify
from flask import Flask

from api import statevector, measurement

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

    reply = statevector(gate_array_string)
    return reply


@app.route('/api/run/do_measurement', methods=['POST'])
def do_measurement():
    gate_array_string = request.form.get('gate_array')
    print("--------------")
    print(gate_array_string)

    reply = measurement(gate_array_string)
    return reply


if __name__ == '__main__':
    app.run(host='127.0.0.1', port=8008)
