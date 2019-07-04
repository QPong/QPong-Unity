#!/usr/bin/env python
#
# Copyright 2019 the original author or authors.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
#
"""Quantum version of the classic Pong game"""

import random

import pygame
from pygame import DOUBLEBUF, HWSURFACE, FULLSCREEN

from utils.ball import Ball
from utils.input import Input
from utils.level import Level
from utils.scene import Scene
from utils.parameters import WINDOW_SIZE, CLASSICAL_COMPUTER, QUANTUM_COMPUTER, \
    WIN_SCORE, WIDTH_UNIT, MEASURE_LEFT, MEASURE_RIGHT, WINDOW_HEIGHT
from utils.colors import BLACK

if not pygame.font:
    print('Warning, fonts disabled')
if not pygame.mixer:
    print('Warning, sound disabled')


def main():
    pygame.init()

    # hardware acceleration to reduce flickering. Works only in full screen
    #flags = DOUBLEBUF | HWSURFACE | FULLSCREEN
    flags = DOUBLEBUF | HWSURFACE
    screen = pygame.display.set_mode(WINDOW_SIZE, flags)

    pygame.display.set_caption('QPong')

    # clock for timing
    clock = pygame.time.Clock()
    old_clock = pygame.time.get_ticks()

    # initialize scene, level and input Classes
    scene = Scene()
    level = Level()
    input = Input()

    # define ball
    ball = Ball()
    balls = pygame.sprite.Group()   # sprite group type is needed for sprite collide function in pygame
    balls.add(ball)

    # Show start screen to select difficulty
    input.running = scene.start(screen, ball)     # start screen returns running flag
    level.setup(scene, ball)

    # Put all moving sprites a group so that they can be drawn together
    moving_sprites = pygame.sprite.Group()
    moving_sprites.add(ball)
    moving_sprites.add(level.paddle[0])
    moving_sprites.add(level.paddle[1])

    # update the screen
    pygame.display.flip()

    # reset the ball
    ball.reset()

    # a valuable to record the time when the paddle is measured
    measure_time = 100000

    # Main Loop
    while input.running:
        # set maximum frame rate
        clock.tick(60)
        # refill whole screen with black color at each frame
        screen.fill(BLACK)

        ball.update()  # update ball position
        scene.dashed_line(screen, ball)  # draw dashed line in the middle of the screen
        scene.score(screen, ball)   # print score

        # level.statevector_grid.display_statevector(scene.qubit_num) # generate statevector grid
        level.statevector[0].draw(screen)  # draw right paddle together with statevector grid
        level.statevector[1].draw(screen)  # draw right paddle together with statevector grid
        level.circuit_grid[0].draw(screen)  # draw circuit grid
        level.circuit_grid[1].draw(screen)  # draw circuit grid
        moving_sprites.draw(screen)  # draw moving sprites

        # Show game over screen if the score reaches WIN_SCORE, reset everything if replay == TRUE
        if ball.score.get_score(CLASSICAL_COMPUTER) >= WIN_SCORE:
            scene.gameover(screen, CLASSICAL_COMPUTER)
            scene.replay(screen, ball.score, level.circuit_grid_model, level.circuit_grid)
            input.update_paddle(level, screen, scene)

        if ball.score.get_score(QUANTUM_COMPUTER) >= WIN_SCORE:
            scene.gameover(screen, QUANTUM_COMPUTER)
            scene.replay(screen, ball.score, level.circuit_grid_model, level.circuit_grid)
            input.update_paddle(level, screen, scene)

        # handle input events
        input.handle_input(level, screen, scene)

        # check ball location and decide what to do
        ball.action()

        if ball.ball_action == MEASURE_LEFT:
            circuit = level.circuit_grid_model[0].compute_circuit()
            pos = level.statevector_grid[0].paddle_after_measurement(circuit, scene.qubit_num, 1)
            level.statevector[0].arrange()

            # paddle after measurement
            level.paddle[0].rect.y = pos
            measure_time = pygame.time.get_ticks()

        if ball.ball_action == MEASURE_RIGHT:
            circuit = level.circuit_grid_model[1].compute_circuit()
            pos = level.statevector_grid[1].paddle_after_measurement(circuit, scene.qubit_num, 1)
            level.statevector[1].arrange()

            # paddle after measurement
            level.paddle[1].rect.y = pos
            measure_time = pygame.time.get_ticks()

        if pygame.sprite.spritecollide(level.paddle[0], balls, False):
            ball.bounce_edge()

        if pygame.sprite.spritecollide(level.paddle[1], balls, False):
            ball.bounce_edge()

        if pygame.time.get_ticks() - measure_time > 400:
            # refresh the screen a moment after measurement to update visual
            input.update_paddle(level, screen, scene)
            # add a buffer time before measure again
            measure_time = pygame.time.get_ticks() + 100000

        # Update the screen
        pygame.display.flip()

    pygame.quit()


if __name__ == '__main__':
    main()
