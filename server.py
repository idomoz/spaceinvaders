import socket
from threading import Thread
from queue import Queue
from figures import *
import time

clients = {}
shots = []
players = {}
server_socket = socket.socket()
server_socket.bind(('0.0.0.0', 6666))
server_socket.listen()


def connect(_, client_address):
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    clients[client_address[0]] = client_socket
    print(client_address[0])
    players[client_address[0]] = Spaceship()
    t = Thread(target=send, args=(client_socket, client_address))
    t.daemon = True
    t.start()
    client_socket.bind(('0.0.0.0', 6666))
    t = Thread(target=recvi, args=(client_socket, client_address))
    t.daemon = True
    t.start()



def recvi(client_socket, *args):
    while True:
        data, addr = client_socket.recvfrom(1024)
        data = data.decode().split(',')
        players[addr[0]].x, players[addr[0]].y = data[0], data[1]
        #shots.append(Shot(x=int(data[2]), y=int(data[3])))


def send(client_socket, client_address):
    while True:
        msg = ''
        for player in players.items():
            if player[0] != client_address[0]:
                msg += str(player[1].x) + ',' + str(player[1].y)
        client_socket.sendto(msg.encode(), client_address)


while True:
    client_socket, client_address = server_socket.accept()
    connect(client_socket, client_address)