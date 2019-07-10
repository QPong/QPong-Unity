import pygame

class Player:
    """Player class
    1. Handle input
    2. Score
    """
    def __init__(self, position='left'):
        self.position = position
        self.score = 0
        self.joystick = pygame.joystick.Joystick()

    # Keep track of scores
    def score_up(self):
        self.score += 1

    def get_score(self):
        return self.score

    def reset_score(self):
        self.score = 0
