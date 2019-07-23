#!/usr/bin/env python3

from qiskit import BasicAer, execute, ClassicalRegister
from copy import deepcopy
import json_tricks

from model.circuit_grid_model import CircuitGridModel, CircuitGridNode
from model import circuit_node_types as node_types


def statevector(circuit_dimension, gate_string):
    circuit = circuit_from_string(circuit_dimension, gate_string)
    shot_num = 1000

    backend_sv_sim = BasicAer.get_backend('statevector_simulator')
    job_sim = execute(circuit, backend_sv_sim, shots=shot_num)
    result_sim = job_sim.result()
    quantum_state = result_sim.get_statevector(circuit, decimals=3)

    return json_tricks.dumps(quantum_state)


def measurement(circuit_dimension, gate_string):
    circuit = circuit_from_string(circuit_dimension, gate_string)
    shot_num = 1

    backend_sv_sim = BasicAer.get_backend('qasm_simulator')
    cr = ClassicalRegister(circuit.width())  # create classical register for each quantum register
    measure_circuit = deepcopy(circuit)  # make a copy of circuit
    measure_circuit.add_register(cr)  # add classical registers for measurement readout
    measure_circuit.measure(measure_circuit.qregs[0], measure_circuit.cregs[0])
    job_sim = execute(measure_circuit, backend_sv_sim, shots=shot_num)
    result_sim = job_sim.result()
    counts = result_sim.get_counts(circuit)

    state_in_decimal = int(list(counts.keys())[0], 2)

    return str(state_in_decimal)


def circuit_from_string(circuit_dimension, gate_string):
    gate_array = gate_string.split(',')
    row_max = int(circuit_dimension.split(',')[0])
    column_max = int(circuit_dimension.split(',')[1])
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
    return circuit
