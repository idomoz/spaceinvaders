using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using WMPLib;

namespace ConsoleApplication29
{
    class Program
    {
        static void PrintMissiles(int missiles)
        {
            Console.SetCursorPosition(50, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Missiles [Enter]: " + missiles);
            Console.ResetColor();
            Console.Write(" ");
        }
        static void LevelPrint(int level)
        {
            Console.SetCursorPosition(Console.WindowWidth - 16, 0);
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Level: " + level + " ");
            Console.ResetColor();
        }
        static void PrintPoints(int points)

        {
            Console.SetCursorPosition(73, 0);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" Score: " + points);
            Console.ResetColor();
            Console.Write(" ");
            Console.SetCursorPosition(Console.WindowWidth - 35, 0);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" [P]ause ");
            Console.ResetColor();
        }
        static void SpaceNRes()
        {
            Console.Write(" ");
            Console.ResetColor();
            Console.Write(" ");
        }
        static void HeatPrint(int heat, WindowsMediaPlayer player7)
        {
            Console.SetCursorPosition(18, 0);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Heat:");
            Console.ResetColor();
            Console.Write(" ");
            if (heat > 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                SpaceNRes();
            }
            if (heat > 3)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                SpaceNRes();
            }
            if (heat > 7)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                SpaceNRes();
            }
            if (heat > 10)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                SpaceNRes();
            }
            if (heat > 14)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                SpaceNRes();
            }
            if (heat > 17)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                SpaceNRes();
            }
            if (heat > 21)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                SpaceNRes();
            }
            if (heat > 24)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                SpaceNRes();
            }
            if (heat > 28)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                SpaceNRes();
            }
            if (heat > 31)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                SpaceNRes();
            }
            if (heat > 35)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                SpaceNRes();
            }
            if (heat > 38)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                SpaceNRes();
            }
            if (heat == 40)
            {
                player7.controls.stop();
                player7.controls.play();
            }
            Console.Write(" ");
        }
        //    static void MoveShip(ref bool right, ref Spaceship ship)
        //  {
        //     if (right)
        //      {
        //         bool flag = ship.MoveRight();
        //         if (!flag)
        //         {
        //             right = !right;
        //             ship.MoveLeft();
        //          }
        //       }
        //    else
        //     {
        //         bool flag = ship.MoveLeft();
        //         if (!flag)
        //          {
        //          right = !right;
        //           ship.MoveRight();
        //         }
        //       }
        //    }
        static void UpgardeFall(ref List<Upgrade> ups, ref Spaceship ship)
        {
            bool flag;
            List<int> toRemove = new List<int>();
            for (int up = 0; up < ups.Count; up++)
            {
                flag = ups[up].Move();
                if (flag)
                    toRemove.Add(up);
            }
            toRemove.Sort();
            for (int remove = toRemove.Count - 1; remove >= 0; remove--)
                ups.RemoveAt(toRemove[remove]);
        }
        static void BatsFall(ref WindowsMediaPlayer player8, ref List<Bat> bats, ref Random ran, ref Spaceship ship)
        {
            int len = (int)(bats.Count * 0.75);
            if (bats.Count < 12)
                len = bats.Count;
            int[] chosens = new int[len];
            for (int chosen = 0; chosen < chosens.Length; chosen++)
                chosens[chosen] = -1;
            int num;
            for (int chosen = 0; chosen < chosens.Length; chosen++)
            {
                bool flag = true;
                do
                {
                    flag = true;
                    num = ran.Next(bats.Count);
                    for (int check = 0; check < chosens.Length; check++)
                    {
                        if (check != chosen && num == chosens[check])
                        {
                            flag = false;
                            continue;
                        }
                    }
                } while (!flag);
                chosens[chosen] = num;
            }
            List<int> toRemove = new List<int>();
            for (int movebat = 0; movebat < chosens.Length; movebat++)
            {
                bool hit = false;
                hit = bats[chosens[movebat]].Move();
                if (hit)
                {
                    bats[chosens[movebat]].Delete();
                    toRemove.Add(chosens[movebat]);
                    ship.Hit(false);
                    if (player8.controls.currentPosition > 0.1)
                        player8.controls.stop();
                    player8.controls.play();
                    Console.SetCursorPosition(7, 0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("Lives: " + ship.lives);
                    Console.ResetColor();
                    Console.Write(" ");
                }
            }
            toRemove.Sort();
            for (int remove = toRemove.Count - 1; remove >= 0; remove--)
                bats.RemoveAt(toRemove[remove]);
        }
        static void ShotsMove(ref WindowsMediaPlayer player1, ref Boss boss, ref WindowsMediaPlayer player3, ref Spaceship ship, ref Stack<Shot> shots, ref Stack<Shot> tempShots, ref List<Bat> bats)
        {
            while (shots.Count > 0)
            {
                tempShots.Push(shots.Pop());
                Shot s = tempShots.Peek();

                s.Move();
                s.Print();
                if (s.location.Y == 0 || s.location.Y == Console.WindowHeight - 1)
                {
                    s.Print();
                    tempShots.Pop();
                    continue;
                }
                if (s.reverse)
                {
                    if (Math.Abs(ship.location.X - s.location.X) < 2 && Math.Abs(ship.location.Y - s.location.Y) < 2)
                    {
                        ShipHitNoBoss(ref ship, ref player1);
                        s.Print();
                        s.Delete();
                        try { tempShots.Pop(); } catch (System.InvalidOperationException) { };
                    }

                }
                else
                {
                    if (boss != null && Math.Abs(boss.location.X - s.location.X) < 18 && (s.location.Y - boss.location.Y < 2 && s.location.Y - boss.location.Y > -8))
                    {
                        s.Print();
                        s.Delete();
                        try { tempShots.Pop(); } catch (System.InvalidOperationException) { };
                        int shipPower = ship.power / 3;
                        if (shipPower == 0)
                            shipPower = 1;
                        bool flag = boss.Hit(shipPower);
                        if (flag)
                        {
                            try
                            {
                                if (player3.controls.currentPosition > 0.05)
                                    player3.controls.stop();
                                player3.controls.play();
                            }
                            catch (System.Runtime.InteropServices.COMException) { };
                            ship.points += 20000;
                            ship.missiles += 2;
                            PrintMissiles(ship.missiles);
                            PrintPoints(ship.points);
                            Console.SetCursorPosition(2, 2);
                            Console.Write("             ");
                        }
                    }
                    for (int bat = 0; bat < bats.Count; bat++)
                        if (Math.Abs(bats[bat].location.X - s.location.X) < 3 && Math.Abs(bats[bat].location.Y - s.location.Y) < 2)
                        {
                            if (bats[bat].lives == 0)
                            {
                                s.Print();
                                s.Delete();
                                try
                                {
                                    tempShots.Pop();
                                }
                                catch (System.InvalidOperationException) { };
                                try
                                {
                                    if (player3.controls.currentPosition > 0.05)
                                        player3.controls.stop();
                                    player3.controls.play();
                                }
                                catch (System.Runtime.InteropServices.COMException) { };
                                ship.points += 100;
                                if (ship.points % 10000 == 0)
                                {
                                    ship.missiles++;
                                    PrintMissiles(ship.missiles);
                                }
                                if (bats[bat].red)
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                        ship.points += 100;
                                        if (ship.points % 10000 == 0)
                                        {
                                            ship.missiles++;
                                            PrintMissiles(ship.missiles);
                                        }
                                    }

                                }

                                PrintPoints(ship.points);
                                bats[bat].Delete();
                                bats.RemoveAt(bat);
                                break;
                            }
                            else
                            {
                                if (ship.power < 3)
                                    bats[bat].lives -= 1;
                                else
                                    bats[bat].lives -= ship.power / 3;
                                if (bats[bat].lives < 0)
                                    bats[bat].lives = 0;
                                s.Print();
                                s.Delete();
                                try { tempShots.Pop(); } catch (System.InvalidOperationException) { };
                            }
                        }
                }
            }
            while (tempShots.Count > 0)
                shots.Push(tempShots.Pop());
        }
        static void NextLevel(ref Boss boss, ref WindowsMediaPlayer player9, ref WindowsMediaPlayer player7, ref WindowsMediaPlayer player5, ref WindowsMediaPlayer player4, ref WindowsMediaPlayer player3, ref WindowsMediaPlayer player2, ref WindowsMediaPlayer player1, ref Missile missile, ref Stack<Shot> tempShots, ref ConsoleKeyInfo k, ref bool screenChanged, ref int screenHeight, ref int screenWidth, ref List<Upgrade> ups, ref int NumBats, ref Spaceship ship, ref List<Bat> bats, ref Stack<Shot> shots, ref int currentLap, ref int level, ref int laps, ref Random ran, ref int heat, ref bool heated)
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2);
            Console.Write("Level Completed!");
            PrintMissiles(ship.missiles);
            PrintPoints(ship.points);
            HeatPrint(heat, player7);
            NumBats++;
            bats = new List<Bat>();
            boss = null;
            currentLap = 0;
            if (level % 2 == 0)
                laps++;
            level++;
            Console.Write(" ");
            LevelPrint(level);
            for (int i = 0; i < 150000; i++)
            {
                if (i % 1000 == 0 && missile != null)
                {
                    bool flag = missile.Move();
                    if (!flag)
                    {
                        missile.Deactivate();
                        player9.controls.stop();
                        player1.controls.stop();
                        player1.controls.play();
                        missile.Delete();
                        for (int bat = 0; bat < bats.Count; bat++)
                            bats[bat].Delete();
                        bats = new List<Bat>();
                        missile = null;
                    }
                    else
                        missile.Print();
                }
                if (i % 300 == 0 && (Console.WindowHeight != screenHeight || Console.WindowWidth != screenWidth))
                {

                    screenChanged = true;
                    screenWidth = Console.WindowWidth;
                    screenHeight = Console.WindowHeight;
                    Console.Clear();
                    PrintMissiles(ship.missiles);
                    PrintPoints(ship.points);
                    HeatPrint(heat, player7);
                    LevelPrint(level);
                    Console.SetCursorPosition(7, 0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Lives: " + ship.lives);
                    Console.ResetColor();
                    ship.ResetLoc();

                    for (int bat = 0; bat < bats.Count; bat++)
                    {
                        if (bats[bat].location.X > Console.WindowWidth - 3)
                            bats[bat].location.X = ran.Next(2, Console.WindowWidth - 2);
                    }
                    ship.Print();
                }
                if ((i % 6000 == 0 && !heated) || (i % 4000 == 0 && heated))
                    if (heat > 0)
                    {
                        heat--;
                        Console.SetCursorPosition(18, 0);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        HeatPrint(heat, player7);
                    }
                    else
                        heated = false;

                if (i % 10000 == 0)
                {
                    Console.SetCursorPosition(7, 0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Lives: " + ship.lives);
                    Console.ResetColor();
                }
                if (i % 100 == 0)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2);
                    Console.Write("Level Completed!");
                    int x = Cursor.Position.X, y = Cursor.Position.Y;

                    if (x >= Console.WindowLeft + 25 && x <= (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth))) + Console.WindowLeft - 25 && y >= Console.WindowTop + 25 && y <= (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 25)) + Console.WindowTop - 25)
                    {

                        ship.lastLocation.X = ship.location.X;
                        ship.lastLocation.Y = ship.location.Y;
                        ship.location.X = (int)(((double)(x - Console.WindowLeft) / (((double)Console.WindowWidth / (double)Console.LargestWindowWidth) * (double)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width))) * Console.WindowWidth);
                        ship.location.Y = (int)(((double)(y - Console.WindowTop) / (((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (double)((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)))) * Console.WindowHeight) + 2;
                        ship.Print();
                    }
                    else
                        if (x < Console.WindowLeft + 25 || x > (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth))) + Console.WindowLeft - 25)
                    {
                        ship.lastLocation.Y = ship.location.Y;
                        ship.location.Y = (int)(((double)(y - Console.WindowTop) / (((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (double)((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 25)))) * Console.WindowHeight) + 2;
                        ship.Print();
                    }
                    else
                            if (y < Console.WindowTop + 25 || y > (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)) + Console.WindowTop - 25)
                    {

                        ship.lastLocation.X = ship.location.X;
                        ship.location.X = (int)(((double)(x - Console.WindowLeft) / (((double)Console.WindowWidth / (double)Console.LargestWindowWidth) * (double)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width))) * Console.WindowWidth);
                        ship.Print();
                    }

                }

                if (Console.KeyAvailable)//input handler
                {
                    int back = InputHandel(ref player9, ref player7, ref player2, ref missile, ref ups, ref k, ref heated, ref shots, ref ship, ref heat);
                    if (back == 1)
                    {

                        NextLevel(ref boss, ref player9, ref player7, ref player5, ref player4, ref player3, ref player2, ref player1, ref missile, ref tempShots, ref k, ref screenChanged, ref screenHeight, ref screenWidth, ref ups, ref NumBats, ref ship, ref bats, ref shots, ref currentLap, ref level, ref laps, ref ran, ref heat, ref heated);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2);
                        Console.Write("                ");
                        foreach (Bat bat in bats)
                            bat.Delete();
                        bats = new List<Bat>();
                        if (level % 10 == 0)
                        {
                            boss = new Boss(100 * level);
                            boss.Print();
                        }
                        else
                        {
                            for (int j = 0; j < NumBats; j++)
                                bats.Add(new Bat(ran.Next(2, Console.WindowWidth - 2), ran.Next(3, 12), level, ran.Next(30 - batChance(level))));
                        }
                        return;
                    }

                }
                if (i % 90000 == 0 || screenChanged)
                {
                    for (int up = 0; up < ups.Count; up++)
                        ups[up].Print();
                }
                if (i % 5000 == 0 || screenChanged)//bats flying
                {

                    List<int> toRemove = new List<int>();
                    for (int up = 0; up < ups.Count; up++)
                    {
                        if (Math.Abs(ups[up].location.X - ship.location.X) < 3 && Math.Abs(ups[up].location.Y - (ship.location.Y - 2)) < 3)
                        {
                            if (ups[up].type == 'L')
                            {
                                ship.lives++;
                                if (player5.controls.currentPosition > 0.1)
                                    player5.controls.stop();
                                player5.controls.play();
                                Console.SetCursorPosition(7, 0);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.Write("Lives: " + ship.lives);
                                Console.ResetColor();
                            }
                            if (ups[up].type == 'P')
                            {
                                
                                    ship.power++;
                                if (player4.controls.currentPosition > 0.1)
                                    player4.controls.stop();
                                player4.controls.play();
                            }
                            ups[up].Print();
                            ups[up].Delete();
                            toRemove.Add(up);
                        }
                    }
                    toRemove.Sort();
                    for (int remove = toRemove.Count - 1; remove >= 0; remove--)
                        ups.RemoveAt(toRemove[remove]);

                }

                if (i % 30000 == 0)
                    UpgardeFall(ref ups, ref ship);
                if (i % 1300 == 0)//shots movement
                    ShotsMove(ref player1, ref boss, ref player3, ref ship, ref shots, ref tempShots, ref bats);
            }
            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2);
            Console.Write("                ");
            if (level % 10 == 0)
            {
                boss = new Boss(100 * level);
                boss.Print();
            }
            else
            {
                for (int j = 0; j < NumBats; j++)
                    bats.Add(new Bat(ran.Next(2, Console.WindowWidth - 2), ran.Next(3, 12), level, ran.Next(30 - batChance(level))));
            }
        }
        static void GameOver(ref Boss boss, ref WindowsMediaPlayer player7, ref WindowsMediaPlayer player6, ref List<Upgrade> ups, ref int laps, ref int currentLap, ref List<Bat> bats, ref Stack<Shot> shots, ref int NumBats, ref Random ran, ref Spaceship ship, ref int level, ref int heat, ref bool heated)
        {
            player6.controls.stop();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
            Console.Write("GAME OVER");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 + 2);
            Console.Write("Score: ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(ship.points);
            Console.ResetColor();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2 + 4);
            Console.Write("Press [SPACEBAR] to play again...");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Spacebar);
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            boss = null;
            laps = 1;
            currentLap = 0;
            Console.Clear();
            
            bats = new List<Bat>();
            shots = new Stack<Shot>();
            ups = new List<Upgrade>();
            NumBats = 3;
            for (int j = 0; j < NumBats; j++)
                bats.Add(new Bat(ran.Next(2, Console.WindowWidth - 2), ran.Next(3, 12), level, ran.Next(30 - batChance(level))));
            ship = new Spaceship();
            ship.Print();
            PrintMissiles(ship.missiles);
            PrintPoints(ship.points);
            level = 1;
            LevelPrint(level);
            
            heat = 0;
            heated = false;
            HeatPrint(heat, player7);
            player6.controls.play();
        }
        static int InputHandel(ref WindowsMediaPlayer player9, ref WindowsMediaPlayer player7, ref WindowsMediaPlayer player2, ref Missile missile, ref List<Upgrade> ups, ref ConsoleKeyInfo k, ref bool heated, ref Stack<Shot> shots, ref Spaceship ship, ref int heat)
        {

            k = Console.ReadKey(true);

            if (k.Key == ConsoleKey.F5)
                ups.Add(new Upgrade('L'));
            if (k.Key == ConsoleKey.F6)
                ups.Add(new Upgrade('P'));
            if (k.Key == ConsoleKey.F7)
                return 1;
            if (k.Key == ConsoleKey.Enter && (missile == null || !missile.active) && ship.missiles > 0)
            {
                if (player9.controls.currentPosition > 0.2)
                    player9.controls.stop();
                player9.controls.play();
                missile = new Missile(new Point(ship.location.X, ship.location.Y - 2), new Point(ship.location.X, ship.location.Y - 2));
                missile.Print();
                ship.missiles--;
                PrintMissiles(ship.missiles);
            }
            if (k.Key == ConsoleKey.P)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 6);
                Console.Write("Game paused");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2 - 4);
                Console.Write("Press [SPACEBAR] to continue...");
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(true);
                } while (key.Key != ConsoleKey.Spacebar);
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 6);
                Console.Write("           ");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2 - 4);
                Console.Write("                               ");

            }
            if (k.Key == ConsoleKey.Spacebar)
                if (!heated)
                {
                    try
                    {
                        if (player2.controls.currentPosition > 0.03)
                            player2.controls.stop();
                        player2.controls.play();
                    }
                    catch (System.Runtime.InteropServices.COMException) { };
                    Stack<Shot> newShots = ship.Fire(ship.power);
                    if (shots.Count == 0 || Math.Abs(newShots.Peek().location.Y - shots.Peek().location.Y) > 1)
                    {
                        int count = newShots.Count;
                        for (int i = 0; i < count; i++)
                            shots.Push(newShots.Pop());
                        if (heat < 40)
                        {
                            heat++;
                            if (heat == 40)
                                heated = true;
                            HeatPrint(heat, player7);
                        }
                        else
                            heated = true;
                    }
                }
            return 0;
        }
        static int batChance(int level)
        {
            if (level / 3 <= 25)
                return level / 3;
            else
                return 25;
        }
        static void Main(string[] args)
        {
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            WMPLib.WindowsMediaPlayer player1 = new WindowsMediaPlayer(), player2 = new WindowsMediaPlayer(), player3 = new WindowsMediaPlayer(), player4 = new WindowsMediaPlayer(), player5 = new WindowsMediaPlayer(), player6 = new WindowsMediaPlayer(), player7 = new WindowsMediaPlayer(), player8 = new WindowsMediaPlayer(), player9 = new WindowsMediaPlayer();

            player1.URL = @"EXPLOSIONS.mp3";
            player1.controls.stop();
            player2.URL = @"shot-lazer.mp3";
            player2.controls.stop();
            player3.URL = @"bat-die.mp3";
            player3.controls.stop();
            player4.URL = @"powerup.mp3";
            player4.controls.stop();
            player5.URL = @"heal.mp3";
            player5.controls.stop();
            player6.URL = @"NMD.mp3";
            player6.controls.play();
            player7.URL = @"heat.mp3";
            player7.controls.stop();
            player8.URL = @"bat-attack.mp3";
            player8.controls.stop();
            player9.URL = @"missile-launch.mp3";
            player9.controls.stop();
            Boss boss = null;
            Missile missile = null;
            Spaceship ship = new Spaceship();
            Stack<Shot> shots = new Stack<Shot>(), tempShots = new Stack<Shot>();
            List<Upgrade> ups = new List<Upgrade>();
            ConsoleKeyInfo k = new ConsoleKeyInfo('c', ConsoleKey.C, false, false, false);
            int NumBats = 3, level = 1, heat = 0, laps = 1, currentLap = 0, screenWidth = Console.WindowWidth, screenHeight = Console.WindowHeight;
            bool heated = false, screenChanged = false;
            HeatPrint(heat, player7);
            PrintMissiles(ship.missiles);
            PrintPoints(ship.points);
            Random ran = new Random();
            List<Bat> bats = new List<Bat>();
            ship.Print();
            for (int i = 0; i < NumBats; i++)
                bats.Add(new Bat(ran.Next(2, Console.WindowWidth - 2), ran.Next(3, 12), level, ran.Next(30 - batChance(level))));
            LevelPrint(level);
            while (true)
                for (long i = 0; i < 1000000000000000; i++)
                {

                    if (i % 12000 == 0 && boss != null)
                    {
                        int next = 3 + level / 5;
                        if (!(ran.Next(next) == 1))
                        {
                            for (int ww = 0; ww < level / 10; ww++)
                            {
                                shots.Push(new Shot(ran.Next(boss.location.X - 18, boss.location.X + 17), boss.location.Y + 1, true));
                                try
                                {
                                    if (player2.controls.currentPosition > 0.03)
                                        player2.controls.stop();
                                    player2.controls.play();
                                }
                                catch (System.Runtime.InteropServices.COMException) { };
                            }
                        }
                    }
                    if (i % 140000 == 0)
                    {
                        try
                        {
                            if (player6.playState == WMPPlayState.wmppsStopped)
                                player6.controls.play();
                        }
                        catch (System.Runtime.InteropServices.COMException) { };
                    }
                    if (i % 1000 == 0 && missile != null)
                    {
                        bool flag = missile.Move();
                        if (!flag)
                        {
                            missile.Deactivate();
                            player9.controls.stop();
                            player1.controls.stop();
                            player1.controls.play();
                            if (boss != null)
                                boss.Hit(100);
                            missile.Delete();
                            for (int bat = 0; bat < bats.Count; bat++)
                            {
                                try
                                {
                                    if (player3.controls.currentPosition > 0.1)
                                        player3.controls.stop();
                                    player3.controls.play();
                                }
                                catch (System.Runtime.InteropServices.COMException) { };
                                bats[bat].Delete();
                            }
                            bats = new List<Bat>();
                            missile = null;
                        }
                        else
                            missile.Print();
                    }
                    if (i % 300 == 0 && (Console.WindowHeight != screenHeight || Console.WindowWidth != screenWidth))
                    {
                        screenChanged = true;
                        screenWidth = Console.WindowWidth;
                        screenHeight = Console.WindowHeight;
                        Console.Clear();
                        PrintMissiles(ship.missiles);
                        PrintPoints(ship.points);
                        HeatPrint(heat, player7);
                        LevelPrint(level);
                        Console.SetCursorPosition(7, 0);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Lives: " + ship.lives);
                        Console.ResetColor();
                        ship.ResetLoc();

                        for (int bat = 0; bat < bats.Count; bat++)
                        {
                            if (bats[bat].location.X > Console.WindowWidth - 3)
                                bats[bat].location.X = ran.Next(2, Console.WindowWidth - 2);
                        }
                        ship.Print();
                    }
                    int moreBat = 50000 - level * 100;
                    if (moreBat < 1000)
                        moreBat = 1000;
                    if (boss == null && i % (moreBat) == 0 && currentLap < laps)
                    {
                        for (int more = 0; more < ran.Next(1, level); more++)
                        {


                            Bat newBat = new Bat(ran.Next(2, Console.WindowWidth - 2), ran.Next(3, 12), level, ran.Next(30 - (batChance(level))));
                            bats.Add(newBat);
                            newBat.Print();
                        }
                    }

                    if ((i % 6000 == 0 && !heated) || (i % 4000 == 0 && heated))
                        if (heat > 0)
                        {
                            heat--;
                            Console.SetCursorPosition(18, 0);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            HeatPrint(heat, player7);
                        }
                        else
                            heated = false;

                    if (i % 10000 == 0)
                    {
                        Console.SetCursorPosition(7, 0);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Lives: " + ship.lives);
                        Console.ResetColor();
                    }

                    if ((bats.Count == 0 && ship.lives > 0 && currentLap == laps && boss == null) || (boss != null && boss.IsDead()))//winning or losing
                        NextLevel(ref boss, ref player9, ref player7, ref player5, ref player4, ref player3, ref player2, ref player1, ref missile, ref tempShots, ref k, ref screenChanged, ref screenHeight, ref screenWidth, ref ups, ref NumBats, ref ship, ref bats, ref shots, ref currentLap, ref level, ref laps, ref ran, ref heat, ref heated);
                    else
                        if (ship.lives == 0)
                        GameOver(ref boss, ref player7, ref player6, ref ups, ref laps, ref currentLap, ref bats, ref shots, ref NumBats, ref ran, ref ship, ref level, ref heat, ref heated);
                    if (i % 1000 == 0 && boss != null)
                    {
                        boss.Move(ship.location);
                    }
                    //if (i % 4000 == 0)// ship movement
                    //   MoveShip(ref right, ref ship);
                    if (i % 100 == 0)
                    {
                        int x = Cursor.Position.X, y = Cursor.Position.Y;

                        if (x >= Console.WindowLeft + 25 && x <= (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth))) + Console.WindowLeft - 25 && y >= Console.WindowTop + 25 && y <= (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 25)) + Console.WindowTop - 25)
                        {

                            ship.lastLocation.X = ship.location.X;
                            ship.lastLocation.Y = ship.location.Y;
                            ship.location.X = (int)(((double)(x - Console.WindowLeft) / (((double)Console.WindowWidth / (double)Console.LargestWindowWidth) * (double)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width))) * Console.WindowWidth);
                            ship.location.Y = (int)(((double)(y - Console.WindowTop) / (((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (double)((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)))) * Console.WindowHeight) + 2;
                            ship.Print();
                        }
                        else
                            if (x < Console.WindowLeft + 25 || x > (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth))) + Console.WindowLeft - 25)
                        {
                            ship.lastLocation.Y = ship.location.Y;
                            ship.location.Y = (int)(((double)(y - Console.WindowTop) / (((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (double)((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 25)))) * Console.WindowHeight) + 2;
                            ship.Print();
                        }
                        else
                                if (y < Console.WindowTop + 25 || y > (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)) + Console.WindowTop - 25)
                        {

                            ship.lastLocation.X = ship.location.X;
                            ship.location.X = (int)(((double)(x - Console.WindowLeft) / (((double)Console.WindowWidth / (double)Console.LargestWindowWidth) * (double)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width))) * Console.WindowWidth);
                            ship.Print();
                        }

                    }

                    if (Console.KeyAvailable)//input handler
                    {
                        int back = InputHandel(ref player9, ref player7, ref player2, ref missile, ref ups, ref k, ref heated, ref shots, ref ship, ref heat);
                        if (back == 1)
                        {
                            for (int bat = 0; bat < bats.Count; bat++)
                                bats[bat].Delete();
                            NextLevel(ref boss, ref player9, ref player7, ref player5, ref player4, ref player3, ref player2, ref player1, ref missile, ref tempShots, ref k, ref screenChanged, ref screenHeight, ref screenWidth, ref ups, ref NumBats, ref ship, ref bats, ref shots, ref currentLap, ref level, ref laps, ref ran, ref heat, ref heated);

                        }
                    }
                    if (i % 90000 == 0 || screenChanged)
                    {
                        for (int up = 0; up < ups.Count; up++)
                            ups[up].Print();
                    }

                    if (i % 1000 == 0 && boss != null)
                    {
                        switch (ship.location.Y - 2)
                        {
                            case 5:
                                if (Math.Abs(boss.location.X - ship.location.X) < 3 && Math.Abs(boss.location.Y - ship.location.Y - 14) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 6:
                                if (Math.Abs(boss.location.X - ship.location.X) < 6 && Math.Abs(boss.location.Y - ship.location.Y - 13) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 7:
                                if (Math.Abs(boss.location.X - ship.location.X) < 7 && Math.Abs(boss.location.Y - ship.location.Y - 12) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 8:
                                if (Math.Abs(boss.location.X - ship.location.X) < 8 && Math.Abs(boss.location.Y - ship.location.Y - 11) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 9:
                                if (Math.Abs(boss.location.X - ship.location.X) < 9 && Math.Abs(boss.location.Y - ship.location.Y - 10) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 10:
                                if (Math.Abs(boss.location.X - ship.location.X) < 10 && Math.Abs(boss.location.Y - ship.location.Y - 9) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 11:
                                if (Math.Abs(boss.location.X - ship.location.X) < 11 && Math.Abs(boss.location.Y - ship.location.Y - 8) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 12:
                                if (Math.Abs(boss.location.X - ship.location.X) < 12 && Math.Abs(boss.location.Y - ship.location.Y - 7) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 13:
                                if (Math.Abs(boss.location.X - ship.location.X) < 13 && Math.Abs(boss.location.Y - ship.location.Y - 6) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 14:
                                if (Math.Abs(boss.location.X - ship.location.X) < 14 && Math.Abs(boss.location.Y - ship.location.Y - 5) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 15:
                                if (Math.Abs(boss.location.X - ship.location.X) < 18 && Math.Abs(boss.location.Y - ship.location.Y - 4) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 16:
                                if (Math.Abs(boss.location.X - ship.location.X) < 19 && Math.Abs(boss.location.Y - ship.location.Y - 3) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 17:
                                if (Math.Abs(boss.location.X - ship.location.X) < 18 && Math.Abs(boss.location.Y - ship.location.Y - 2) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 18:
                                if (Math.Abs(boss.location.X - ship.location.X) < 17 && Math.Abs(boss.location.Y - ship.location.Y - 1) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 19:
                                if (Math.Abs(boss.location.X - ship.location.X) < 13 && Math.Abs(boss.location.Y - ship.location.Y) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 20:
                                if (Math.Abs(boss.location.X - ship.location.X) < 10 && Math.Abs(boss.location.Y - ship.location.Y + 1) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;
                            case 21:
                                if (Math.Abs(boss.location.X - ship.location.X) < 10 && Math.Abs(boss.location.Y - ship.location.Y + 2) < 3)
                                    ShipHit(ref boss, ref ship, ref player1);
                                break;


                        }
                    }
                    if (i % 5000 == 0 || screenChanged)//bats flying
                    {
                        List<int> toRemove = new List<int>();
                        for (int bat = 0; bat < bats.Count; bat++)
                        {
                            if (bats[bat].red && ran.Next(20) == 0)
                            {
                                shots.Push(new Shot(bats[bat].location.X, bats[bat].location.Y + 1, true));
                                try
                                {
                                    if (player2.controls.currentPosition > 0.03)
                                        player2.controls.stop();
                                    player2.controls.play();
                                }
                                catch (System.Runtime.InteropServices.COMException) { };
                            }
                            if (Math.Abs(bats[bat].location.X - ship.location.X) < 4 && Math.Abs(bats[bat].location.Y - ship.location.Y + 2) < 3)
                            {
                                ship.Hit();
                                player1.controls.stop();
                                player1.controls.play();
                                bats[bat].Delete();
                                toRemove.Add(bat);
                                Console.SetCursorPosition(7, 0);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.Write("Lives: " + ship.lives);
                                Console.ResetColor();
                                Console.Write(" ");
                                Cursor.Position = new Point((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth)) / 2) + Console.WindowLeft, (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)) + Console.WindowTop - 90);

                                ship.ResetLoc();
                                ship.Print();

                            }
                            else

                                bats[bat].Print();
                        }
                        toRemove.Sort();
                        for (int remove = toRemove.Count - 1; remove >= 0; remove--)
                        {

                            bats.RemoveAt(toRemove[remove]);

                        }
                        
                        

                    }
                    if(i%500==0||screenChanged)
                    {
                        List<int> toRemove = new List<int>();
                        for (int up = 0; up < ups.Count; up++)
                        {
                            ups[up].Print();
                            if (Math.Abs(ups[up].location.X - ship.location.X) < 3 && Math.Abs(ups[up].location.Y - (ship.location.Y - 2)) < 3)
                            {
                                if (ups[up].type == 'L')
                                {
                                    ship.lives++;
                                    if (player5.controls.currentPosition > 0.1)
                                        player5.controls.stop();
                                    player5.controls.play();
                                    Console.SetCursorPosition(7, 0);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                    Console.Write("Lives: " + ship.lives);
                                    Console.ResetColor();
                                }
                                if (ups[up].type == 'P')
                                {

                                    ship.power++;
                                    if (player4.controls.currentPosition > 0.1)
                                        player4.controls.stop();
                                    player4.controls.play();
                                }
                                ups[up].Print();
                                ups[up].Delete();
                                toRemove.Add(up);
                            }
                        }
                        toRemove.Sort();
                        for (int remove = toRemove.Count - 1; remove >= 0; remove--)
                            ups.RemoveAt(toRemove[remove]);
                    }
                    if (i % 30000 == 0)
                        UpgardeFall(ref ups, ref ship);

                    int batMove = 10010 - level * 100;
                    if (batMove < 1000)
                        batMove = 1000;
                    if (i % batMove == 0)//batsFall
                        BatsFall(ref player8, ref bats, ref ran, ref ship);

                    if (i % 1300 == 0)//shots movement
                        ShotsMove(ref player1, ref boss, ref player3, ref ship, ref shots, ref tempShots, ref bats);

                    if (i % 350000 == 0)
                        if (currentLap < laps)
                        {
                            currentLap++;


                            int upgrade = ran.Next(2);
                            if (upgrade == 0)
                                ups.Add(new Upgrade('P'));
                            else
                                ups.Add(new Upgrade('L'));

                        }
                    screenChanged = false;


                }
        }
        static void ShipHit(ref Boss boss, ref Spaceship ship, ref WindowsMediaPlayer player1)
        {
            ship.Hit();
            player1.controls.stop();
            player1.controls.play();
            Console.SetCursorPosition(7, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Lives: " + ship.lives);
            Console.ResetColor();
            Console.Write(" ");
            Cursor.Position = new Point((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth)) / 2) + Console.WindowLeft, (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)) + Console.WindowTop - 90);
            ship.ResetLoc();
            ship.Print();
            boss.Hit(10);
        }
        static void ShipHitNoBoss(ref Spaceship ship, ref WindowsMediaPlayer player1)
        {
            ship.Hit();
            player1.controls.stop();
            player1.controls.play();
            Console.SetCursorPosition(7, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Lives: " + ship.lives);
            Console.ResetColor();
            Console.Write(" ");
            Cursor.Position = new Point((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * (((double)Console.WindowWidth / (double)Console.LargestWindowWidth)) / 2) + Console.WindowLeft, (int)(((double)Console.WindowHeight / (double)Console.LargestWindowHeight) * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50)) + Console.WindowTop - 90);
            ship.ResetLoc();
            ship.Print();

        }
    }

}
