import console
import cursor
from threading import Thread
from queue import Queue
from msvcrt import getch
import time
import socket

cursor.hide()
console.maximize_console()
console.clear()
shots = []
try:
    scr_width, scr_height = console.get_screen_size()
    win_width, win_height = console.get_console_size()
except:
    scr_width, scr_height = 1700,800
    win_width, win_height =170,44
from figures import *

spaceship = Spaceship()
other = Spaceship()
client_socket = socket.socket()
client_socket.connect(('10.0.0.8', 6666))
client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
client_socket.bind(('0.0.0.0', 6666))

def move_shot(shot):
    shot.move()
    return shot.y != 0


def shots_manager():
    while True:
        global shots
        shots = list(filter(move_shot, shots))
        time.sleep(0.01)

def mouse_handler():
    while True:
        try:
            pos_x, pos_y = console.get_cursor_position()
        except:
            pos_x, pos_y = 10,10
        spaceship.move(int((pos_x / scr_width) * win_width), int((pos_y / scr_height) * win_height))
        client_socket.sendto((str(spaceship.x) + ',' + str(spaceship.y)).encode(), ('10.0.0.8', 6666))
        data, addr = client_socket.recvfrom(1024)
        data = data.decode().split(',')
        other.move(int(data[0]), int(data[1]))
        time.sleep(0.033)


for func in [shots_manager, mouse_handler]:
    t = Thread(target=func)
    t.daemon = True
    t.start()

while True:
    ch = getch()
    if ch == b' ':
        shot = Shot(spaceship)
        shots.append(shot)
        shot.print()
    time.sleep(0.033)
