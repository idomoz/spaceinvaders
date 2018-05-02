import console
import random

try:
    max_width, max_height = console.get_console_size()
except:
    max_width, max_height = 170, 44

color_map = {
    'B': 'BLACK',
    'L': 'BLUE',
    'W': 'WHITE',
    'C': 'CYAN',
    'M': 'MAGENTA',
    'G': 'GREEN',
    'R': 'RED',
    'Y': 'YELLOW',
    'b': 'LIGHTBLACK_EX',
    'l': 'LIGHTBLUE_EX',
    'w': 'LIGHTWHITE_EX',
    'c': 'LIGHTCYAN_EX',
    'm': 'LIGHTMAGENTA_EX',
    'g': 'LIGHTGREEN_EX',
    'r': 'LIGHTRED_EX',
    'y': 'LIGHTYELLOW_EX'
}


class Figure:
    def __init__(self):
        self.rows = None
        self.right_size = None
        self.calc_img()

    def calc_img(self):
        self.rows = self.img.split('\n')
        self.right_size = max(len(row) - row.count('$') * 2 for row in self.rows)

    def print(self, erase=False, reload=False):
        if reload:
            self.calc_img()
        x = self.x
        y = self.y
        del_y = 0
        img_right = x + self.right_size
        img_bottom = y + len(self.rows) - 1
        if img_right > max_width:
            x -= img_right - max_width
        if img_bottom > max_height:
            y -= img_bottom - max_height
        for row in self.rows:
            del_x = 0
            words = row.split('$')
            for i, word in enumerate(words):
                if i % 2:
                    fore_color = color_map[word[0]] if not erase else 'WHITE'
                    back_color = color_map[word[1]] if not erase else 'BLACK'
                    console.add_print(word[2:] if not erase else ' ' * (len(word) - 2), x + del_x, y + del_y,
                                      fore_color, back_color)
                    del_x += len(word) - 2
                elif word == ' ' * len(word):
                    del_x += len(word)
                else:
                    console.add_print(word if not erase else ' ' * len(word), x + del_x, y + del_y)
                    del_x += len(word)
            del_y += 1


class Heat(Figure):
    img = ''
    full_img = '''
$Bg $ $Bg $ $BG $ $Bw $ $Bw $ $By $ $BY $ $Br $ $Br $ $BR $
'''[1:-1]

    def __init__(self):
        super().__init__()
        self.x, self.y = 5, 0
        self.heat = 0.0
        self.heated = False

    def change_heat(self, heat_up=True, heat_up_value=0.1):
        old_heat = self.heat
        self.heat = min(self.heat + heat_up_value, 10.9) if heat_up else max(self.heat - 0.1, 0)
        if self.heat == 0:
            self.heated = False
        if self.heat == 10.9:
            self.heated = True
        if int(old_heat) != int(self.heat):
            if not heat_up:
                self.erase()
            self.print()

    def print(self):
        self.img = self.full_img[:int(self.heat) * 6]
        super().print(reload=True)

    def erase(self):
        self.img = self.full_img[:60]
        super().print(erase=True, reload=True)


class Spaceship(Figure):
    img = '''
 $GB^$
$GB/$$Rl*$$GB\$
 $LB"$
    '''[1:-1]
    shot_heat_map = {
        'A': 0.1,
        'B': 0.2,
    }

    def __init__(self):
        super().__init__()
        self.x, self.y = 5, 5
        self.lives = 5
        self.power = 1
        self.shot_type = 'A'
        self.missiles = 0
        self.score = 0
        self.heat = Heat()
        self.heated = False

    def move(self, x, y):
        self.print(erase=True)
        self.x = x
        self.y = max(1, y)
        self.print()

    def shoot(self, heat_up=False):
        if not self.heat.heated:
            shot = Shot(self)
            shot.print()
            if heat_up:
                self.heat.change_heat(heat_up_value=self.shot_heat_map[self.shot_type])
            return shot


class LifePack(Figure):
    img = '$wgL$'

    def __init__(self):
        super().__init__()
        self.x = random.randint(0, max_width)
        self.y = 1


class PowerPack(Figure):
    img = '$wmP$'

    def __init__(self):
        super().__init__()
        self.x = random.randint(0, max_width)
        self.y = 1


class Bat(Figure):
    img = ''

    def __init__(self):
        super().__init__()
        self.x = random.randint(0, max_width)
        self.y = 1
        self.full_img = '$YBv$$YBV$'
        self.phase = 0

    def print(self):
        self.img = self.full_img[self.phase: 5 + self.phase]
        self.phase = 5 - self.phase
        super().print(reload=True)


class Shot(Figure):
    img = '$wm\'$'

    def __init__(self, spaceship):
        super().__init__()
        self.player_id = id(spaceship)
        self.x = min(spaceship.x + 1, max_width - 2)
        self.y = min(max_height - 4, max(spaceship.y - 1, 1))

    def move(self):
        self.print(erase=True)
        self.y = max(0, self.y - 2)
        if self.y:
            self.print()


class Chicken(Figure):
    img = ''
    right_up = '''
(\  }\  
(  \_('> 
(__(=_)  
   -"= 
'''[1:-1]
    right_down = '''
    }\  
 ____('> 
(  /=_)  
(/ -"= 
'''[1:-1]
    left_up = '''
  /{  /)
<')_/  )
 (_=)__)
  ="-   
'''[1:-1]
    left_down = '''
  /{  
<')____
 (_=\  )
  ="- \)  
'''[1:-1]

    def __init__(self):
        super().__init__()
        self.direction = 'right'
        self.x = random.randint(0, 30) * 8
        self.y = random.randint(1, 5) * 4
        self.phase = 'up'
        self.img = getattr(self, '{}_{}'.format(self.direction, self.phase))
        self.print(reload=True)

    def move(self):
        self.print(erase=True)
        self.x += 1 if self.direction == 'right' else -1
        if self.x >= max_width - 9:
            self.direction = 'left'
            self.x = max_width - 9
        if self.x == 0:
            self.direction = 'right'
        self.img = getattr(self, '{}_{}'.format(self.direction, self.phase))
        self.phase = 'down' if self.phase == 'up' else 'up'
        self.print(reload=True)


class Egg(Figure):
    img = '0'

    def __init__(self, chicken):
        super().__init__()
        self.x = chicken.x + 2
        self.y = chicken.y + 8
        self.print()

    def move(self):
        self.print(erase=True)
        self.y += 1
        if self.y < max_height:
            self.print()
