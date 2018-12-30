using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication29
{
    class Spaceship
    {
        public Point location, lastLocation;
        public int power,missiles,points;
        public int lives;
        public Spaceship()
        {
            this.location=new Point( Console.WindowWidth / 2,Console.WindowHeight-2);
            this.lastLocation = new Point();
            this.power = 1;
            missiles = 5;
            lives=10;
            points = 0;
        }
        public void ResetLoc()
        {
            try
            {
                Console.SetCursorPosition(this.location.X , this.location.Y - 3);
                Console.Write(" ");
                Console.SetCursorPosition(this.location.X - 1, this.location.Y - 2);
                Console.Write("   ");
                Console.SetCursorPosition(this.location.X , this.location.Y - 1);
                Console.Write(" ");
            }
            catch (System.ArgumentOutOfRangeException) { };
            this.lastLocation = this.location;
            this.location = new Point(Console.WindowWidth / 2, Console.WindowHeight - 2);
        }
        public void Hit()
        {
            if (this.lives == 0)
                return;
            this.lives--;
            if(this.power>1)
            this.power--;
        }
        public void Hit(bool power)
        {
            if (this.lives == 0)
                return;
            this.lives--;
            if(power)
            if (this.power > 1)
                this.power--;
        }
       
        
        public Stack<Shot> Fire(int size)
        {
            int redSize = size / 2;
            
            if (redSize > 10)
                redSize = 10;
            size = size / 3;
            if (size == 0)
                size = 1;
            if (size > 5)
                size = 5;
            Stack<Shot> newShots = new Stack<Shot>();
            int tempLoc=this.location.X-size+1;
            
            for (int i = 0; i < size; i++)
            {
                if (tempLoc > 0 && tempLoc < Console.WindowWidth - 1)
                {
                    bool red = false;
                    if (Math.Abs(tempLoc - this.location.X) < redSize/2)
                        red = true;
                    newShots.Push(new Shot(red,tempLoc, this.location.Y - 4));
                }
                tempLoc += 2;
            }
            return newShots;
        }
        public void Print()
        {
            if (this.location.X < 2)
                this.location.X = 2;
            if (this.location.X > Console.WindowWidth-3)
                this.location.X = Console.WindowWidth - 3;
            if (this.location.Y < 5)
                this.location.Y = 5;
            if (this.location.Y>Console.WindowHeight-1)
                this.location.Y = Console.WindowHeight - 1;
            if(this.lastLocation!=this.location)
            try
            {
                Console.SetCursorPosition(this.lastLocation.X, this.lastLocation.Y - 3);
                Console.Write(" ");
                Console.SetCursorPosition(this.lastLocation.X - 1, this.lastLocation.Y - 2);
                Console.Write("   ");
                Console.SetCursorPosition(this.lastLocation.X, this.lastLocation.Y - 1);
                Console.Write(" ");
            }
            catch (System.ArgumentOutOfRangeException) { };
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(this.location.X, this.location.Y - 3);
            Console.Write("^");
            Console.SetCursorPosition(this.location.X - 1, this.location.Y - 2);
            Console.Write("/");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\\");
            Console.SetCursorPosition(this.location.X, this.location.Y - 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\"");
            Console.ResetColor();
            
            
        }
    }
}
