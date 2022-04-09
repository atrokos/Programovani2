using System;

namespace tictactoe
{
    internal class Program
    {


        class TurnManager // Manages turns between the two players
        {
            Player P1, P2;
            Board board;
            Checker checker;

            public TurnManager(Player p1, Player p2, Board board, Checker checker)
            {
                P1 = p1;
                P2 = p2;
                this.board = board;
                this.checker = checker;
            }
        }

        class Checker // Checks if any of the two players won
        {
            Player P1, P2;
            Board board;

            public Checker(Player p1, Player p2, Board board)
            {
                P1 = p1;
                P2 = p2;
                this.board = board;
            }

            public void CheckBoard()
            {
                for (int i = 0; i < board.GetHeight(); i++)
                {
                    if (board.GetTile(i, 0) == board.GetTile(i, 1) && board.GetTile(i, 1) == board.GetTile(i, 2))
                    {

                    }
                }
            }
        }

        class Player
        {
            string sign; // X or O
            bool isAI; // Whether it is controlled by the AI

            public Player(string sign, bool isAI)
            {
                this.isAI = isAI;
                this.sign = sign;
            }

            public string Sign()
            {
                return sign;
            }

            public void YourTurn()
            {

            } 
        }

        class AI
        {

        }

        class Loader
        {
            string ReadLetter()//Reads one letter or dot, returns it as a string
            {
                char ch;
                do
                {
                    ch = Convert.ToChar((Console.Read()));
                    if (Char.IsLetter(ch) || ch == '.')
                        return Char.ToString(ch);
                } while (true);
            }

            int ReadNumber()
            {
                string line;
                do
                {
                    line = Console.ReadLine();
                    if (line != null)
                        return Convert.ToInt32(line);
                }
                while (true);
            }

            Board CreateBoard()
            {
                string[,] board = new string[3,3];
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        board[i,j] = ReadLetter();
                    }
                }
                return new Board(board);
            }
        }

        class Board
        {
            string[,] board;

            public Board(string[,] board)
            {
                this.board = board;
            }

            public string GetTile(int h, int w)
            {
                return board[h, w];
            }

            public int GetHeight()
            {
                return board.GetLength(0);
            }

            public int GetWidth()
            {
                return board.GetLength(1);
            }

            public bool ChangeTile(string what, int h, int w) //Changes tile at h,w coordinates to what; returns true if possible, false if not
            {
                if (board[h, w] == ".")
                {
                    board[h, w] = what;
                    return true;
                }
                return false;
            }
        }

        static void Main(string[] args)
        {
            
        }
    }
}