import console
import cursor
from threading import Thread
from queue import Queue
from msvcrt import getch
import time
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

# setup connection
if len(argv) == 1:
    hosting = True
else:
    hosting = False
    host_ip = argv[1]
tcp_socket = socket.socket()
if hosting:
    print('waiting for guest...')
    tcp_socket.bind(('0.0.0.0', 6000))
    tcp_socket.listen()
    guest_socket, guest_address = tcp_socket.accept()
    udp_address = (guest_address[0], 6001)
    tcp_socket.close()
    tcp_socket = guest_socket
else:
    tcp_socket.connect((host_ip, 6000))
    udp_address = (host_ip, 6001)
console.clear()
udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
udp_socket.bind(('0.0.0.0', 6001))
udp_queue = Queue()
tcp_queue = Queue()


def tcp_queue_manager():
    while True:
        data = tcp_queue.get()
        try:
            tcp_socket.send(data)
        except ConnectionResetError:
            pass
        tcp_queue.task_done()


def udp_queue_manager():
    while True:
        data = udp_queue.get()
        try:
            udp_socket.sendto(data, udp_address)
        except ConnectionResetError:
            pass
        udp_queue.task_done()


def move_shot(shot):
    shot.move()
    return shot.y != 0


def shots_manager():
    while True:
        global shots
        shots = list(filter(move_shot, shots))
        time.sleep(0.02)


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
            try:
                udp_queue.put('{},{},{}$'.format(0, player.x, player.y).encode())
            except ConnectionResetError:
                pass
        time.sleep(0.02)


def receive_channel():
    while True:
        try:
            data = udp_socket.recvfrom(1024)[0].decode()[:-1].split('$')[0]
            data = data.split(',')
            if data[0] == '0':
                ally.move(int(data[1]), int(data[2]))
            elif data[0] == '1':
                pass
        except ConnectionResetError:
            pass


# start threads
for func in [shots_manager, movement_handler, receive_channel, udp_queue_manager, tcp_queue_manager]:
    t = Thread(target=func)
    t.daemon = True
    t.start()

while True:
    ch = getch()
    if ch == b' ':
        shot = Shot(player)
        shots.append(shot)
        shot.print()

    if ch == b'\x03':
        udp_socket.close()
        tcp_socket.close()
        exit()
    time.sleep(0.02)
