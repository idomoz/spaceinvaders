using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication29
{
    class Bat
    {
        public Point location;
        public bool small;
        public int lives;
        public bool red = false;
        public Bat(int x, int y, int level)
        {
            this.lives = level / 5;
            this.location = new Point(x, y);
            this.small = false;
            Print();
            Random ran = new Random();

        }
        public Bat(int x, int y, int level, int makeRed)
        {
            this.lives = level / 4;
            this.location = new Point(x, y);
            this.small = false;
            Print();
            Random ran = new Random();
            if (makeRed == 0)
            {
                red = true;
                if (lives == 0)
                    lives++;
                lives *= 10;
            }
        }
        public bool Move()
        {
            if (this.location.Y == Console.WindowHeight - 4)
            {
                Delete();
                return true;
            }
            this.location.Y++;
            if (this.location.Y > 1)
            {
                try
                {
                    Console.SetCursorPosition(this.location.X, this.location.Y - 1);
                    Console.Write(" ");
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Random ran = new Random();
                    this.location.X = ran.Next(2, Console.WindowWidth - 2);
                };
            }
            Print();
            return false;

        }
        public void Print()
        {
            if (this.red)
                Console.ForegroundColor = ConsoleColor.Red;
            try
            {
                Console.SetCursorPosition(this.location.X, this.location.Y);
                if (this.small)
                {
                    Console.Write("v");
                    this.small = false;
                    Console.ResetColor();
                    return;
                }
                Console.Write("V");
                this.small = true;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                Random ran = new Random();
                this.location.X = ran.Next(2, Console.WindowWidth - 2);
            };
            Console.ResetColor();
        }
        public void Delete()
        {
            try
            {
                Console.SetCursorPosition(this.location.X, this.location.Y);
                Console.Write(" ");
            }
            catch (System.ArgumentOutOfRangeException) { };
        }
    }
}
