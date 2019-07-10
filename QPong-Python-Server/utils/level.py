import pygame

from model.circuit_grid_model import CircuitGridModel
from model import circuit_node_types as node_types
from containers.vbox import VBox
from viz.statevector_grid import StatevectorGrid
from controls.circuit_grid import CircuitGrid, CircuitGridNode
from utils.parameters import WIDTH_UNIT, CIRCUIT_DEPTH, WINDOW_HEIGHT


class Level:
    """Start up a level"""
    def __init__(self):
        self.level = 3  # game level
        self.win = False  # flag for winning the game
        self.paddle = [pygame.sprite.Sprite(), pygame.sprite.Sprite()]

    def setup(self, scene, ball):
        """Setup a level with a certain level number"""
        scene.qubit_num = self.level

        self.circuit_grid_model = [CircuitGridModel(scene.qubit_num, CIRCUIT_DEPTH),
                                   CircuitGridModel(scene.qubit_num, CIRCUIT_DEPTH)]

        # the game crashes if the circuit is empty
        # initialize circuit with identity gate at the end of each line to prevent crash
        # identity gate are displayed by completely transparent PNG
        for i in range(scene.qubit_num):
            self.circuit_grid_model[0].set_node(i, CIRCUIT_DEPTH - 1, CircuitGridNode(node_types.IDEN))
            self.circuit_grid_model[1].set_node(i, CIRCUIT_DEPTH - 1, CircuitGridNode(node_types.IDEN))

        self.circuit = [self.circuit_grid_model[0].compute_circuit(),
                        self.circuit_grid_model[1].compute_circuit()]

        self.statevector_grid = [StatevectorGrid(self.circuit[0], scene.qubit_num, 100, position='left'),
                                 StatevectorGrid(self.circuit[1], scene.qubit_num, 100, position='right')]

        self.statevector = [VBox(WIDTH_UNIT * 13, WIDTH_UNIT * 0, self.statevector_grid[0]),
                            VBox(WIDTH_UNIT * 81, WIDTH_UNIT * 0, self.statevector_grid[1])]

        self.circuit_grid = [CircuitGrid(WIDTH_UNIT * 0, 0, self.circuit_grid_model[0], position='left'),
                             CircuitGrid(WIDTH_UNIT * 85, 0, self.circuit_grid_model[1], position='right')]

        paddle_size = int(round(WINDOW_HEIGHT / 2 ** scene.qubit_num))

        # player 0 paddle for detection of collision. It is invisible on the screen
        self.paddle[0].image = pygame.Surface([WIDTH_UNIT, paddle_size])
        self.paddle[0].image.fill((255, 0, 255))
        self.paddle[0].image.set_alpha(255)
        self.paddle[0].rect = self.paddle[0].image.get_rect()
        self.paddle[0].rect.x = self.statevector[0].xpos + 5 * WIDTH_UNIT

        # player 1 paddle for detection of collision. It is invisible on the screen
        self.paddle[1].image = pygame.Surface([WIDTH_UNIT, paddle_size])
        self.paddle[1].image.fill((255, 0, 255))
        self.paddle[1].image.set_alpha(255)
        self.paddle[1].rect = self.paddle[1].image.get_rect()
        self.paddle[1].rect.x = self.statevector[1].xpos

    def levelup(self):
        """Increase level by 1"""
        if self.level <= 3:
            self.level += self.level
            self.setup()
        else:
            self.win = True  # win the game if level is higher than 3
