using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication29
{
    class Boss
    {
        private int lives, startLives, direction,small=0;
        public Point location, lastLocation;
        Random ran;
        
        public Boss(int lives)
        {
            ran = new Random();
            this.direction = 0;


            this.lives = lives;
            this.startLives = lives;
            this.location = new Point(Console.WindowWidth / 2, 21);
            this.lastLocation = this.location;
        }
        public void PrintLives(int power)
        {
            Console.SetCursorPosition(2, 2);
            Console.Write("Boss's live: ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(new string(' ', (int)(((double)this.lives / (double)this.startLives) * (Console.WindowWidth - 17))));
            Console.ResetColor();
            Console.Write(new string(' ', power));
        }
        public bool Hit(int power)
        {

            this.lives -= power;

            if (this.lives <= 0)
            {
                this.lives = 0;
                delete();
                PrintLives(power);
                return true;
            }
            PrintLives(power);
            return false;
        }
        public void delete()
        {
            Console.SetCursorPosition(this.location.X - 2, this.location.Y - 16);
            Console.Write(new string(' ', 5));
            Console.SetCursorPosition(this.location.X - 5, this.location.Y - 15);
            Console.Write(new string(' ', 11));
            Console.SetCursorPosition(this.location.X - 6, this.location.Y - 14);
            Console.Write(new string(' ', 13));
            Console.SetCursorPosition(this.location.X - 7, this.location.Y - 13);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(this.location.X - 8, this.location.Y - 12);
            Console.Write(new string(' ', 17));
            Console.SetCursorPosition(this.location.X - 9, this.location.Y - 11);
            Console.Write(new string(' ', 19));
            Console.SetCursorPosition(this.location.X - 10, this.location.Y - 10);
            Console.Write(new string(' ', 21));
            Console.SetCursorPosition(this.location.X - 11, this.location.Y - 9);
            Console.Write(new string(' ', 23));
            Console.SetCursorPosition(this.location.X - 12, this.location.Y - 8);
            Console.Write(new string(' ', 25));
            Console.SetCursorPosition(this.location.X - 13, this.location.Y - 7);
            Console.Write(new string(' ', 27));
            Console.SetCursorPosition(this.location.X - 17, this.location.Y - 6);
            Console.Write(new string(' ', 35));
            Console.SetCursorPosition(this.location.X - 18, this.location.Y - 5);
            Console.Write(new string(' ', 37));
            Console.SetCursorPosition(this.location.X - 17, this.location.Y - 4);
            Console.Write(new string(' ', 35));
            Console.SetCursorPosition(this.location.X - 16, this.location.Y - 3);
            Console.Write(new string(' ', 33));
            Console.SetCursorPosition(this.location.X - 12, this.location.Y - 2);
            Console.Write(new string(' ', 25));
            Console.SetCursorPosition(this.location.X - 9, this.location.Y - 1);
            Console.Write(new string(' ', 19));
            Console.SetCursorPosition(this.location.X - 9, this.location.Y);
            Console.Write(new string(' ', 19));
        }
        public void Move(Point shipLoc)
        {
            PrintLives(1);



            if (shipLoc.X - this.location.X > 0)
                direction = 1;
            else
            if (shipLoc.X - this.location.X < 0)
                direction = 0;
            else
            {
                PrintHalf();
                return;
            }


            if (this.direction == 1)
            {
                if (this.location.X + 19 < Console.WindowWidth - 2)
                {
                    this.lastLocation = this.location;
                    this.location = new Point(this.location.X + 1, this.location.Y);
                }
                else
                {
                    PrintHalf();
                    return;
                }

            }
            else
            {
                if (this.location.X - 19 > 1)
                {

                    this.lastLocation = this.location;
                    this.location = new Point(this.location.X - 1, this.location.Y);
                }
                else
                {
                    PrintHalf();
                    return;
                }
            }
            Print();


        }
        public bool IsDead()
        {
            if (this.lives == 0)
                return true;
            return false;
        }
        public void PrintHalf()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(this.location.X - 6, this.location.Y - 3);
            switch (small)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    Console.Write("o O o O o O o"); break;
                case 5 :
                case 6:
                case 7:
                case 8:
                    Console.Write("o o O o O o O"); break;
                case 9 :
                case 10:
                case 11:
                case 12:
                    Console.Write("o o o O o O o"); break;
                case 13:
                case 14:
                case 15:
                case 16:
                    Console.Write("o o o o O o O"); break;
                case 17:
                case 18:
                case 19:
                case 20:
                    Console.Write("o o o o o O o"); break;
                case 21:
                case 22:
                case 23:
                case 24:
                    Console.Write("o o o o o o O"); break;
                case 25:
                    case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                    Console.Write("o o o o o o o"); break;
                case 35:
                case 36:
                case 37:
                case 38:
                    Console.Write("O o o o o o o"); break;
                case 39:
                case 40:
                case 41:
                case 42:
                    Console.Write("o O o o o o o"); break;
                case 43:
                case 44:
                case 45:
                case 46:
                    Console.Write("O o O o o o o"); break;
                case 47:
                case 48:
                case 49:
                case 50:
                    Console.Write("o O o O o o o"); break;
                case 51:
                case 52:
                case 53:
                case 54:
                    Console.Write("O o O o O o o"); break;
            }
            if (small < 54)
                small++;
            else
                small = 1;
            Console.ResetColor();
        }
        public void Print()
        {

            Console.SetCursorPosition(this.lastLocation.X - 2, this.lastLocation.Y - 16);
            Console.Write(new string(' ', 5));
            Console.SetCursorPosition(this.lastLocation.X - 5, this.lastLocation.Y - 15);
            Console.Write(new string(' ', 11));
            Console.SetCursorPosition(this.lastLocation.X - 6, this.lastLocation.Y - 14);
            Console.Write(new string(' ', 13));
            Console.SetCursorPosition(this.lastLocation.X - 7, this.lastLocation.Y - 13);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(this.lastLocation.X - 8, this.lastLocation.Y - 12);
            Console.Write(new string(' ', 17));
            Console.SetCursorPosition(this.lastLocation.X - 9, this.lastLocation.Y - 11);
            Console.Write(new string(' ', 19));
            Console.SetCursorPosition(this.lastLocation.X - 10, this.lastLocation.Y - 10);
            Console.Write(new string(' ', 21));
            Console.SetCursorPosition(this.lastLocation.X - 11, this.lastLocation.Y - 9);
            Console.Write(new string(' ', 23));
            Console.SetCursorPosition(this.lastLocation.X - 12, this.lastLocation.Y - 8);
            Console.Write(new string(' ', 25));
            Console.SetCursorPosition(this.lastLocation.X - 13, this.lastLocation.Y - 7);
            Console.Write(new string(' ', 27));
            Console.SetCursorPosition(this.lastLocation.X - 17, this.lastLocation.Y - 6);
            Console.Write(new string(' ', 35));
            Console.SetCursorPosition(this.lastLocation.X - 18, this.lastLocation.Y - 5);
            Console.Write(new string(' ', 37));
            Console.SetCursorPosition(this.lastLocation.X - 17, this.lastLocation.Y - 4);
            Console.Write(new string(' ', 35));
            Console.SetCursorPosition(this.lastLocation.X - 16, this.lastLocation.Y - 3);
            Console.Write(new string(' ', 33));
            Console.SetCursorPosition(this.lastLocation.X - 12, this.lastLocation.Y - 2);
            Console.Write(new string(' ', 25));
            Console.SetCursorPosition(this.lastLocation.X - 9, this.lastLocation.Y - 1);
            Console.Write(new string(' ', 19));
            Console.SetCursorPosition(this.lastLocation.X - 9, this.lastLocation.Y);
            Console.Write(new string(' ', 19));



            Console.SetCursorPosition(this.location.X - 2, this.location.Y - 16);
            Console.Write("_____");
            Console.SetCursorPosition(this.location.X - 5, this.location.Y - 15);
            Console.Write(",-\"     \"-.");
            Console.SetCursorPosition(this.location.X - 6, this.location.Y - 14);
            Console.Write("/ o       o \\");
            Console.SetCursorPosition(this.location.X - 7, this.location.Y - 13);
            Console.Write("/   \\     /   \\");
            Console.SetCursorPosition(this.location.X - 8, this.location.Y - 12);
            Console.Write("/     )-\"-(     \\");
            Console.SetCursorPosition(this.location.X - 9, this.location.Y - 11);
            Console.Write("/     ( 6 6 )     \\");
            Console.SetCursorPosition(this.location.X - 10, this.location.Y - 10);
            Console.Write("/       \\ \" /       \\");
            Console.SetCursorPosition(this.location.X - 11, this.location.Y - 9);
            Console.Write("/         )=(         \\");
            Console.SetCursorPosition(this.location.X - 12, this.location.Y - 8);
            Console.Write("/   o  .--\" - \"--.  o   \\");
            Console.SetCursorPosition(this.location.X - 13, this.location.Y - 7);
            Console.Write("/    I  /  -   -  \\  I    \\");
            Console.SetCursorPosition(this.location.X - 17, this.location.Y - 6);
            Console.Write(".--(    (_}y/\\       /\\y{_)    )--.");
            Console.SetCursorPosition(this.location.X - 18, this.location.Y - 5);
            Console.Write("(    \".___l\\/__\\_____/__\\/l___,\"    )");
            Console.SetCursorPosition(this.location.X - 17, this.location.Y - 4);
            Console.Write("\\                                 /");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(this.location.X - 6, this.location.Y - 3);
            switch (small)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    Console.Write("o O o O o O o"); break;
                case 5:
                case 6:
                case 7:
                case 8:
                    Console.Write("o o O o O o O"); break;
                case 9:
                case 10:
                case 11:
                case 12:
                    Console.Write("o o o O o O o"); break;
                case 13:
                case 14:
                case 15:
                case 16:
                    Console.Write("o o o o O o O"); break;
                case 17:
                case 18:
                case 19:
                case 20:
                    Console.Write("o o o o o O o"); break;
                case 21:
                case 22:
                case 23:
                case 24:
                    Console.Write("o o o o o o O"); break;
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                    Console.Write("o o o o o o o"); break;
                case 35:
                case 36:
                case 37:
                case 38:
                    Console.Write("O o o o o o o"); break;
                case 39:
                case 40:
                case 41:
                case 42:
                    Console.Write("o O o o o o o"); break;
                case 43:
                case 44:
                case 45:
                case 46:
                    Console.Write("O o O o o o o"); break;
                case 47:
                case 48:
                case 49:
                case 50:
                    Console.Write("o O o O o o o"); break;
                case 51:
                case 52:
                case 53:
                case 54:
                    Console.Write("O o O o O o o"); break;
                    
            }
            if (small < 54)
                small++;
            else
                small = 1;
            Console.ResetColor();
            Console.SetCursorPosition(this.location.X - 16, this.location.Y - 3);
            Console.Write("\" -._");
            Console.SetCursorPosition(this.location.X +13, this.location.Y - 3);
            Console.Write("_,-\""); 
            Console.SetCursorPosition(this.location.X - 12, this.location.Y - 2);
            Console.Write("`--Y--.___________.--Y--'");
            Console.SetCursorPosition(this.location.X - 9, this.location.Y - 1);
            Console.Write("|==.___________.==|");
            Console.SetCursorPosition(this.location.X - 9, this.location.Y);
            Console.Write("`==.___________.=='");
        }
    }
}
