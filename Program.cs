using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships
{
    class Program
    {
        const string TestGame = @"battleship_assets\testships.txt";

        public struct TShip
        {
            public string Name;
            public int Size;
            public bool Sunk;
        }

        private static void GetPosition(ref int Row, ref int Column)
        {
            bool valid_col = false, valid_row = false;

            Console.WriteLine();

            while (valid_col == false)
            {
                Console.Write("Please enter column: ");
                try
                {
                    Column = int.Parse(Console.ReadLine()!);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Please enter a number between 0 and 9.");
                    Console.Write("Please enter column: ");
                    Column = int.Parse(Console.ReadLine()!);
                }


                if (Column >= 0 && Column <= 9)
                {
                    valid_col = true;
                }
            }

            while (valid_row == false)
            {
                Console.Write("Please enter row: ");
                try
                {
                    Row = int.Parse(Console.ReadLine()!);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Please enter a number between 0 and 9.");
                    Console.Write("Please enter row: ");
                    Row = int.Parse(Console.ReadLine()!);
                }


                if (Row >= 0 && Row <= 9)
                {
                    valid_row = true;
                }
            }


            Console.WriteLine();
        }

        private static void MakeMove(ref char[,] Board, ref TShip[] Ships)
        {
            int Row = 0;
            int Column = 0;

            GetPosition(ref Row, ref Column);

            if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h')
            {
                Console.WriteLine("Sorry, you have already shot at the square (" + Column + "," + Row + "). Please try again.");
            }
            else if (Board[Row, Column] == '-')
            {
                Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a miss.");
                Board[Row, Column] = 'm';
            }
            else
            {
                Console.WriteLine("Hit at (" + Column + "," + Row + ").");
                Board[Row, Column] = 'h';
            }
        }

        private static void InitialiseBoard(ref char[,] Board)
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = '-';
                }
            }
        }

        private static void LoadGame(string TestGame, ref char[,] Board)
        {
            string Line = "";
            StreamReader BoardFile = new StreamReader(TestGame);

            for (int Row = 0; Row < 10; Row++)
            {
                Line = BoardFile.ReadLine()!;
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = Line[Column]!;
                }
            }

            BoardFile.Close();
        }

        private static void RandomiseShips(ref char[,] Board, TShip[] Ships)
        {
            Random RandomNumber = new Random();
            bool Valid;
            char Orientation = ' ';
            int Row = 0;
            int Column = 0;
            int HorV = 0;

            for (int index = 0; index < Ships.Length; index++)
            {
                Valid = false;

                while (Valid == false)
                {
                    Row = RandomNumber.Next(0, 10);
                    Column = RandomNumber.Next(0, 10);
                    HorV = RandomNumber.Next(0, 2);

                    if (HorV == 0)
                    {
                        Orientation = 'v';
                    }
                    else
                    {
                        Orientation = 'h';
                    }

                    Valid = ValidPlacement(Board, Ships[index], Row, Column, Orientation);
                }

                Console.WriteLine("Computer placing the " + Ships[index].Name);

                PlaceShip(ref Board, Ships[index], Row, Column, Orientation);
            }
        }

        private static void PlaceShip(ref char[,] Board, TShip Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row + Scan, Column] = Ship.Name[0];
                }
            }
            else if (Orientation == 'h')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row, Column + Scan] = Ship.Name[0];
                }
            }
        }

        private static bool ValidPlacement(char[,] Board, TShip Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v' && Row + Ship.Size > 10)
            {
                return false;
            }
            else if (Orientation == 'h' && Column + Ship.Size > 10)
            {
                return false;
            }
            else
            {
                if (Orientation == 'v')
                {
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        if (Board[Row + Scan, Column] != '-')
                        {
                            return false;
                        }
                    }
                }
                else if (Orientation == 'h')
                {
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        if (Board[Row, Column + Scan] != '-')
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool CheckIfWon(char[,] Board)
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //TODO IsShipsSunk
        //public static bool IsShipsSunk()
        //{

        //}

        private static void OutputBoard(char[,] Board)
        {
            Console.WriteLine();
            Console.WriteLine("The board looks like this: ");
            Console.WriteLine();
            Console.Write(" ");

            for (int Column = 0; Column < 10; Column++)
            {
                Console.Write(" " + Column + "  ");
            }

            Console.WriteLine();

            for (int Row = 0; Row < 10; Row++)
            {
                Console.Write(Row + " ");

                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == '-')
                    {
                        Console.Write(" ");
                    }
                    else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(Board[Row, Column]);
                    }
                    if (Column != 9)
                    {
                        Console.Write(" | ");
                    }
                }

                Console.WriteLine();
            }
        }

        private static void OutputMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Load test game");
            Console.WriteLine("9. Quit");
            Console.WriteLine();
        }

        private static int GetMenuChoice()
        {
            int Choice = 0;

            Console.Write("Please enter your choice: ");
            Choice = int.Parse(Console.ReadLine()!);
            Console.WriteLine();

            return Choice;
        }

        private static void PlayGame(ref char[,] Board, ref TShip[] Ships)
        {
            bool GameWon = false;

            while (GameWon == false)
            {
                OutputBoard(Board);
                MakeMove(ref Board, ref Ships);
                GameWon = CheckIfWon(Board);

                if (GameWon == true)
                {
                    Console.WriteLine("All ships sunk!");
                    Console.WriteLine();
                }
            }
        }

        private static void InitialiseShips(ref TShip[] Ships)
        {
            Ships[0].Name = "Aircraft Carrier";
            Ships[0].Size = 5;
            Ships[0].Sunk = false;

            Ships[1].Name = "Battleship";
            Ships[1].Size = 4;
            Ships[1].Sunk = false;

            Ships[2].Name = "Submarine";
            Ships[2].Size = 3;
            Ships[2].Sunk = false;

            Ships[3].Name = "Destroyer";
            Ships[3].Size = 3;
            Ships[3].Sunk = false;

            Ships[4].Name = "Patrol Boat";
            Ships[4].Size = 2;
            Ships[4].Sunk = false;
        }

        static void Main(string[] args)
        {
            TShip[] Ships = new TShip[5];
            char[,] Board = new char[10, 10];
            int MenuOption = 0;

            while (MenuOption != 9)
            {
                InitialiseBoard(ref Board);
                InitialiseShips(ref Ships);

                OutputMenu();
                MenuOption = GetMenuChoice();

                if (MenuOption == 1)
                {
                    RandomiseShips(ref Board, Ships);
                    PlayGame(ref Board, ref Ships);
                }

                if (MenuOption == 2)
                {
                    LoadGame(TestGame, ref Board);
                    PlayGame(ref Board, ref Ships);
                }
            }
        }
    }
}
