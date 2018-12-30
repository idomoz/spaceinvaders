using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication29
{
    class Upgrade
    {
        public Point location;
        public char type;
        
        public Upgrade(char type )
        {
            Random ran=new Random();
            this.location = new Point(ran.Next(2, Console.WindowWidth - 2),1);
            this.type = type;
            
        }
        
        public bool Move()
        {
            if (this.location.Y == Console.WindowHeight - 2)
            {
                Delete();
                return true;
            }
            this.location.Y++;
            if (this.location.Y > 1)
            {
                Console.SetCursorPosition(this.location.X, this.location.Y - 1);
                Console.Write(" ");
            }
            
            Print();
            return false;
        }
        public void Delete()
        {
            Console.SetCursorPosition(this.location.X, this.location.Y);
            Console.Write(" ");
        }
        public void Print()
        {

            Console.SetCursorPosition(this.location.X, this.location.Y);
            Console.ForegroundColor = ConsoleColor.Black;
            switch(this.type)
            {
                case 'L': Console.BackgroundColor = ConsoleColor.Green; break;
                case 'P': Console.BackgroundColor = ConsoleColor.Yellow; break;
            }
            
            Console.Write(type);
            
            Console.ResetColor();
        }
    }
}
