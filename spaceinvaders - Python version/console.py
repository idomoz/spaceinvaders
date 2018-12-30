from ctypes import *
from ctypes import wintypes
from colorama import Fore, Back, Style, init
import os
from threading import Thread
from queue import Queue
import struct
import msvcrt
import subprocess

print_queue = Queue()
SW_MAXIMIZE = 3
windll.kernel32.GetConsoleWindow.restype = wintypes.HWND
windll.kernel32.GetLargestConsoleWindowSize.restype = wintypes._COORD
windll.kernel32.GetLargestConsoleWindowSize.argtypes = (wintypes.HANDLE,)
windll.user32.ShowWindow.argtypes = (wintypes.HWND, c_int)
STD_OUTPUT_HANDLE = -11
init()
os.system('cls')


class COORD(Structure):
    _fields_ = [("x", c_short), ("y", c_short)]


class POINT(Structure):
    _fields_ = [("x", c_long), ("y", c_long)]


def print_at(st, x, y, fore_color, back_color):
    h = windll.kernel32.GetStdHandle(STD_OUTPUT_HANDLE)
    non_default_color = fore_color != 'WHITE' or back_color != 'BLACK'
    color_tag = ''
    if non_default_color:
        color_tag = getattr(Fore, fore_color) + getattr(Back, back_color)
    windll.kernel32.SetConsoleCursorPosition(h, COORD(x, y))
    c = (color_tag + st + (Style.RESET_ALL if non_default_color else '')).encode("windows-1252")
    windll.kernel32.WriteConsoleA(h, c_char_p(c), len(c), None, None)


def get_cursor_position():
    pt = POINT()
    windll.user32.GetCursorPos(byref(pt))
    return pt.x, pt.y


def get_screen_size():
    return windll.user32.GetSystemMetrics(0), windll.user32.GetSystemMetrics(1)


def get_console_size():
    h = windll.kernel32.GetStdHandle(-12)
    csbi = create_string_buffer(22)
    res = windll.kernel32.GetConsoleScreenBufferInfo(h, csbi)
    if res:
        bufx, bufy, curx, cury, wattr, left, top, right, bottom, maxx, maxy = struct.unpack("hhhhHhhhhhh", csbi.raw)
        width = right - left + 1
        height = bottom - top + 1
        return width, height


def maximize_console():
    fd = os.open('CONOUT$', os.O_RDWR)
    try:
        h_con = msvcrt.get_osfhandle(fd)
        max_size = windll.kernel32.GetLargestConsoleWindowSize(h_con)
        if max_size.X == 0 and max_size.Y == 0:
            raise WinError(get_last_error())
    finally:
        os.close(fd)
    cols = max_size.X
    h_wnd = windll.kernel32.GetConsoleWindow()
    if cols and h_wnd:
        lines = max_size.Y
        subprocess.check_call('mode.com con cols={} lines={}'.format(
                                cols, lines))
        windll.user32.ShowWindow(h_wnd, SW_MAXIMIZE)


def add_print(st, x, y, fore_color='WHITE', back_color='BLACK'):
    print_queue.put((st, x, y, fore_color, back_color))


def print_manager():
    while True:
        st, x, y, fore_color, back_color = print_queue.get()
        print_at(st, x, y, fore_color, back_color)
        print_queue.task_done()


t = Thread(target=print_manager)
t.daemon = True
t.start()

