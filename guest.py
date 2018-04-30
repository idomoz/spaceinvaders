import socket
from threading import Thread
from queue import Queue
from figures import *
import time


#shots = []
players = {}



def connect(player_address, other_player_address):
    players[client_address[0]] = Spaceship()
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    client_socket.bind(('0.0.0.0', udp_port))
    for func in [recv_channel, send_channel]:
        t = Thread(target=func, args=(client_socket, (client_address[0], 6000)))
        t.daemon = True
        t.start()


def recv_channel(client_socket, client_address):
    while True:
        try:
            data = client_socket.recvfrom(1024)[0].decode()[:-1].split('$')[0]
            data = data.split(',')
            if data[0] == '0':

                players[client_address[0]].x, players[client_address[0]].y = int(data[1]), int(data[2])
            elif data[0] == '1':
                pass

        except ConnectionResetError:
            client_socket.close()
            del players[client_address[0]]
            return


def send_channel(client_socket, client_address):
    pos_x, pos_y = 0, 0
    while True:
        try:
            for player in players.items():
                if player[0] != client_address[0] and (pos_x != player[1].x or pos_y != player[1].y):
                    client_socket.sendto('{},{},{}$'.format(0, player[1].x, player[1].y).encode(), client_address)
                    pos_x, pos_y = player[1].x, player[1].y

        except ConnectionResetError:
            client_socket.close()
            return

        # shots.append(Shot(x=int(data[2]), y=int(data[3])))
        time.sleep(0.02)


server_socket = socket.socket()
server_socket.bind(('0.0.0.0', 6001))
server_socket.listen()
client1_socket, client1_address = server_socket.accept()
client2_socket, client2_address = server_socket.accept()
client1_socket.send(str(6002).encode())
client2_socket.send(str(6003).encode())
client1_socket.close()
client2_socket.close()
connect((client1_address[0], 6002), (client2_address[0], 6003))
connect((client2_address[0], 6003), (client1_address[0], 6002))
