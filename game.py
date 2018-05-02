import console
import cursor
from threading import Thread
from msvcrt import getch
from sys import argv
import time
import network

# initialize screen
cursor.hide()
console.maximize_console()
console.clear()
header = 23
footer = 43
try:
    scr_width, scr_height = console.get_screen_size()
    win_width, win_height = console.get_console_size()
except:
    scr_width, scr_height = 1680, 1050
    win_width, win_height = 170, 44
scr_height -= header + footer

# initialize game
from figures import *

player = Spaceship()
ally = Spaceship()
shots = []
enemies = []
eggs = []
for i in range(10):
    enemies.append(Chicken())
network.setup_connection(None if len(argv) == 1 else argv[1])


def movement_handler():
    while True:
        try:
            pos_x, pos_y = console.get_cursor_position()
        except:
            pos_x, pos_y = 10, 1
        pos_x -= 5
        pos_y = max(0, pos_y - header - 40)
        pos_x, pos_y = int((pos_x / scr_width) * win_width), int((pos_y / scr_height) * win_height)
        if player.x != pos_x or player.y != pos_y:
            player.move(pos_x, pos_y)
            if network.multiplayer:
                network.udp_queue.put('{},{},{}$'.format(0, player.x, player.y).encode())
        time.sleep(0.02)


def move_shot(shot):
    shot.move()
    return shot.y != 0


def shots_manager():
    while True:
        global shots
        shots = list(filter(move_shot, shots))
        time.sleep(0.02)


def move_enemies():
    while True:
        for enemy in enemies:
            enemy.move()
            if random.randint(0, 500) == 0:
                eggs.append(Egg(enemy))
        time.sleep(0.1)


def move_egg(egg):
    egg.move()
    return egg.y != max_height


def move_eggs():
    global eggs
    while True:
        eggs = list(filter(move_egg, eggs))
        time.sleep(0.1)


def cool_down():
    while True:
        player.heat.change_heat(heat_up=False)
        ally.heat.change_heat(heat_up=False)
        time.sleep(0.07)


def network_manager():
    while True:
        data = network.action_queue.get()
        if data[0] == '0':
            ally.move(int(data[1]), int(data[2]))
        elif data[0] == '1':
            ally_shot = ally.shoot()
            if ally_shot is not None:
                shots.append(ally_shot)
        network.action_queue.task_done()
# start threads


for func in [
    shots_manager,
    movement_handler,
    move_enemies,
    move_eggs,
    cool_down,
    network_manager,
             ]:
    t = Thread(target=func)
    t.daemon = True
    t.start()

while True:
    ch = getch()
    if ch == b' ':
        shot = player.shoot(heat_up=True)
        if shot is not None:
            shots.append(shot)
            if network.multiplayer:
                network.udp_queue.put('{}$'.format(1).encode())
    elif ch == b'\x03':
        network.udp_socket.close()
        network.tcp_socket.close()
        exit()
    time.sleep(0.03)
