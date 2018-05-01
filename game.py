import console
import cursor
from threading import Thread
from queue import Queue
from msvcrt import getch
import time
import random
import socket
from sys import argv

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

# setup connection
if len(argv) == 1:
    hosting = True
else:
    hosting = False
    host_ip = argv[1]
multiplayer = False
udp_address = None
tcp_socket = None


def accept_guest():
    host_tcp_socket = socket.socket()
    host_tcp_socket.bind(('0.0.0.0', 6000))
    host_tcp_socket.listen()
    global udp_address, tcp_socket, multiplayer
    while True:
        if not multiplayer:
            tcp_socket, guest_address = host_tcp_socket.accept()
            udp_address = (guest_address[0], 6001)
            multiplayer = True
        else:
            time.sleep(1)


if hosting:
    t = Thread(target=accept_guest)
    t.daemon = True
    t.start()
else:
    tcp_socket = socket.socket()
    while not multiplayer:
        tcp_socket.connect((host_ip, 6000))
        udp_address = (host_ip, 6001)
        multiplayer = True
console.clear()
udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
udp_socket.bind(('0.0.0.0', 6001))
udp_queue = Queue()
tcp_queue = Queue()


def tcp_queue_manager():
    global multiplayer
    while True:
        data = tcp_queue.get()
        try:
            tcp_socket.send(data)
        except ConnectionResetError:
            multiplayer = False
        tcp_queue.task_done()


def udp_queue_manager():
    while True:
        data = udp_queue.get()
        try:
            udp_socket.sendto(data, udp_address)
        except ConnectionResetError:
            print('err send')
        udp_queue.task_done()


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
            if multiplayer:
                udp_queue.put('{},{},{}$'.format(0, player.x, player.y).encode())
        time.sleep(0.02)


def receive_udp():
    while True:
        try:
            data_list = udp_socket.recvfrom(1024)[0].decode()[:-1].split('$')
            for i, data in enumerate(data_list):
                data = data.split(',')
                if data[0] == '0':
                    ally.move(int(data[1]), int(data[2]))
                elif data[0] == '1':
                    add_shot(ally)
        except ConnectionResetError:
            pass


def receive_tcp():
    global multiplayer
    while True:
        if multiplayer:
            try:
                data_list = tcp_socket.recv(1024)[0].decode()[:-1].split('$')
                for i, data in enumerate(data_list):
                    if data[0] == '0':
                        pass
                    elif data[0] == '1':
                        pass
            except ConnectionResetError:
                multiplayer = False
        else:
            time.sleep(1)


def add_shot(spaceship):
    shot = Shot(spaceship)
    shots.append(shot)
    shot.print()


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


# start threads
for func in [shots_manager, movement_handler, receive_udp, receive_tcp, udp_queue_manager, tcp_queue_manager,
             move_enemies, move_eggs]:
    t = Thread(target=func)
    t.daemon = True
    t.start()

while True:
    ch = getch()
    if ch == b' ':
        add_shot(player)
        if multiplayer:
            udp_queue.put('{}$'.format(1).encode())
    if ch == b'\x03':
        udp_socket.close()
        tcp_socket.close()
        exit()
    time.sleep(0.04)
