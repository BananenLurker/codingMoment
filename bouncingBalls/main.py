import pygame
import math
from pygame.time import Clock
from random import randrange

# Initialize Pygame
pygame.init()

# Set up the screen
width, height = 1000, 1000
screen = pygame.display.set_mode((width, height))

# Initialize clock for controlling frame rate
clock = Clock()
running = True
hit = pygame.mixer.Sound("hit.wav")

# Define ball properties and speeds
balls = [
    {"rect": pygame.Rect(width/2 + 1, 500, 25, 25), "color": (255, 0, 0), "speed": [0, 5], "radius": 12.5},
]
circle = {
    "center": (width/2, height/2),
    "radius": 300,
    "color": (0, 255, 0)
}

center = (width // 2, height // 2)
radius = 400
thickness = 5
red = (255, 0, 0)
pygame.draw.circle(screen, red, center, radius, thickness)

# Function to check intersection between two circles
def circles_intersect(ball, circle, tolerance=1.0):
    ball_center = ball["rect"].center
    dx = ball_center[0] - circle["center"][0]
    dy = ball_center[1] - circle["center"][1]
    distance = math.sqrt(dx**2 + dy**2)
    return abs(distance - (ball["radius"] + circle["radius"])) <= tolerance

while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

    screen.fill((0, 0, 0))

    for ball in balls:
        rect = ball["rect"]
        speed = ball["speed"]

        # Move the ball
        speed[1] += 0.5
        rect.x += speed[0]
        rect.y += speed[1]

        if circles_intersect(ball, circle):
          speed[0] = -speed[0]
          speed[1] = -speed[1]
          while circles_intersect(ball, circle):
            rect.x += speed[0]
            rect.y += speed[1]

        # Bounce the ball off the walls
        # if rect.left <= 0 or rect.right >= width:
        #     speed[0] = -speed[0]
        # if rect.top <= 0 or rect.bottom >= height:
        #     speed[1] = -speed[1]
            #balls.append({"rect": pygame.Rect(randrange(20, width - 100), 5, 80, 80), "color": (randrange(255), randrange(255), randrange(255)), "speed": [0, randrange(0, 5)]})
            # pygame.mixer.Sound.play(hit)
            # pygame.mixer.music.stop()

        # Draw the ball
        pygame.draw.circle(screen, ball["color"], rect.center, int(ball["radius"]))
        pygame.draw.circle(screen, circle["color"], circle["center"], circle["radius"], thickness)

    pygame.display.flip()
    clock.tick(60)
