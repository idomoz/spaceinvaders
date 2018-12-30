using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication29
{
    class Shot
    {
        public Point location;
        public bool reverse,red;
        public Shot(bool red,int XLoc, int YLoc)
        {
            this.location = new Point(XLoc, YLoc);
            this.reverse = false;
            this.red = red;
        }
        public Shot(int XLoc, int YLoc, bool reverse)
        {
            this.location = new Point(XLoc, YLoc);
            this.reverse = reverse;
        }
        public void Move()
        {
            if (reverse)
                this.location.Y = this.location.Y + 1;
            else
                this.location.Y = this.location.Y - 1;
        }
        public void Print()
        {
            if (!reverse)
            {
                if (this.location.Y > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if(this.red)
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(this.location.X, this.location.Y);
                    Console.Write("^");
                    Console.ResetColor();
                }
                if (this.location.Y == Console.WindowHeight - 4)
                    return;
                Console.SetCursorPosition(this.location.X, this.location.Y + 1);
                Console.Write(" ");
            }
            else
            {
                if (this.location.Y < Console.WindowHeight - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(this.location.X, this.location.Y);
                    Console.Write("8");
                    Console.ResetColor();
                }

                Console.SetCursorPosition(this.location.X, this.location.Y - 1);
                Console.Write(" ");
            }
        }

        public void Delete()
        {
            Console.SetCursorPosition(this.location.X, this.location.Y);
            Console.Write(" ");
        }

    }
}
