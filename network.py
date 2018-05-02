import socket
from queue import Queue
import time
from threading import Thread

udp_queue = Queue()
tcp_queue = Queue()
action_queue = Queue()
multiplayer = False
tcp_socket = None
udp_socket = None
host_tcp_socket = None
hosting = None
udp_address = None


def setup_connection(ip):
    global udp_queue, tcp_queue, action_queue, multiplayer, tcp_socket, udp_socket, hosting, udp_address

    if ip is None:
        hosting = True
    else:
        hosting = False
        host_ip = ip
    tcp_socket = None

    def accept_guest():
        global host_tcp_socket, udp_address, tcp_socket, multiplayer
        host_tcp_socket = socket.socket()
        host_tcp_socket.bind(('0.0.0.0', 6000))
        host_tcp_socket.listen()
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
    udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    udp_socket.bind(('0.0.0.0', 6001))

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
                pass
            udp_queue.task_done()

    def receive_udp():
        while True:
            try:
                data_list = udp_socket.recvfrom(1024)[0].decode()[:-1].split('$')
                for data in data_list:
                    action_queue.put(data.split(','))
            except ConnectionResetError:
                pass

    def receive_tcp():
        global multiplayer
        while True:
            if multiplayer:
                try:
                    data_list = tcp_socket.recv(1024)[0].decode()[:-1].split('$')
                    for data in data_list:
                        action_queue.put(data.split(','))
                except ConnectionResetError:
                    multiplayer = False
            else:
                time.sleep(1)

    # start threads
    for func in [
        receive_udp,
        receive_tcp,
        udp_queue_manager,
        tcp_queue_manager,
    ]:
        t = Thread(target=func)
        t.daemon = True
        t.start()


def close_connection():
    global tcp_socket, udp_socket, host_tcp_socket, hosting
    tcp_socket.close()
    udp_socket.close()
    if hosting:
        host_tcp_socket.close()
