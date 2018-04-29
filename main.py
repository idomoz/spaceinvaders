import console
import cursor
from queue import Queue
from msvcrt import getch
cursor.hide()
console.maximize_console()
console.clear()

from figures import *

def input_handler():
    while True:
        ch = getch()

heat = Heat()
spaceship = Spaceship()
bat = Bat()
power = PowerPack()
life = LifePack()
spaceship.print()
bat.print()
power.print()
life.print()
heat.print_heat(spaceship.heat)

x = input()
bat.print()
x = input()
