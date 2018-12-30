using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication29
{
    class Missile
    {
        private Point locationIn, locationOut,last;
        private double InY,InX,stepX,stepY;
        public bool active = true;
        public Missile(Point locationIn, Point locationOut)
        {
            this.locationIn = locationIn;
            this.InY = locationIn.Y;
            this.InX = locationIn.X;
            this.locationOut = locationOut;
            this.last = new Point();
            Point middlePoint = new Point(Console.WindowWidth / 2, Console.WindowHeight / 2);
            if (this.locationIn.Equals(middlePoint))
                return;
            this.stepX = (middlePoint.X - this.locationIn.X) / Math.Sqrt(Math.Pow(middlePoint.Y - this.locationIn.Y, 2) + Math.Pow(middlePoint.X - this.locationIn.X, 2));
            this.stepY= (middlePoint.Y - this.locationIn.Y) / Math.Sqrt(Math.Pow(middlePoint.Y - this.locationIn.Y, 2) + Math.Pow(middlePoint.X - this.locationIn.X, 2));
        }
        public Point GetLocationIn()
        {
            return this.locationIn;
        }
        public Point GetLocationOut()
        {
            return this.locationOut;
        }
        public void Deactivate()
        {
            this.active = false;
        }
        public void Delete()
        {
            Console.SetCursorPosition(this.locationOut.X, this.locationOut.Y);
            Console.Write(" ");
            Console.SetCursorPosition(this.locationIn.X, this.locationIn.Y);
            Console.Write(" ");
        }
        public bool Move()
        {

            Point middlePoint = new Point(Console.WindowWidth / 2, Console.WindowHeight / 2);
            if (middlePoint.Equals(this.locationIn))
                return false;
            if(Math.Sqrt(Math.Pow(middlePoint.Y - this.locationIn.Y, 2) + Math.Pow(middlePoint.X - this.locationIn.X, 2))<=1.5)
            {
                this.last.X = this.locationOut.X;
                this.last.Y = this.locationOut.Y;
                this.locationOut.X = this.locationIn.X;
                this.locationOut.Y = this.locationIn.Y;
                this.locationIn = middlePoint;
                return true;
            }
            this.last.X = this.locationOut.X;
            this.last.Y = this.locationOut.Y;
            this.locationOut.X = this.locationIn.X;
            this.locationOut.Y = this.locationIn.Y;
            this.InX += this.stepX;
            this.InY += this.stepY;
            this.locationIn.X = (int)InX;
            this.locationIn.Y = (int)InY;
            return true;
        }
        public void Print()
        {
            Console.SetCursorPosition(this.last.X, this.last.Y);
            Console.Write(" ");
            Console.SetCursorPosition(this.locationOut.X, this.locationOut.Y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("*");
            Console.SetCursorPosition(this.locationIn.X, this.locationIn.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("*");
            
            Console.ResetColor();
        }
    }
}
