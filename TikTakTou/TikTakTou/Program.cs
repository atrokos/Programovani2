using System;

namespace tictactoe
{
    internal class Program
    {


        class TurnManager // Manages turns between the two players
        {
            Player P1, P2;
            Board board;

            public TurnManager(Player p1, Player p2, Board board)
            {
                P1 = p1;
                P2 = p2;
                this.board = board;
            }
        }

        class Player
        {
            string sign; // X or O
            string opponent; // Opponent's sign
            bool isAI; // Whether it is controlled by the AI

            public Player(string sign, bool isAI)
            {
                this.isAI = isAI;
                this.sign = sign;
                if (sign == "x")
                    this.opponent = "o";
                else
                    this.opponent = "x";
            }

            public string Sign()
            {
                return sign;
            }

            public string Opponent()
            {
                return opponent;
            }
        }

        class AI
        {
            Player player;
            Board board;

            int startMinimax()
            {
                Board evalboard = new Board(board.GetWholeBoard());

            }

            int minimax(Board evalboard, bool Max) // Evaluate evalboard; Max = true for max's turn, false for min's turn
            {
                int currentValue = evalboard.EvaluateBoard(player.Sign());
                if (currentValue == 10)
                    return currentValue;

                if (currentValue == -10)
                    return currentValue;

                if (!evalboard.AnyMovesLeft())
                    return 0;


                if (Max)
                {
                    int bestMove = -100;
                    for (int i = 0; i < evalboard.GetHeight(); i++)
                    {
                        for (int j = 0; j < evalboard.GetWidth(); j++)
                        {
                            if (evalboard.GetTile(i,j) == ".") // If the tile is empty, put your sign in it
                            {
                                evalboard.ChangeTile(player.Sign(), i, j);
                                bestMove = Math.Max(bestMove, minimax(evalboard, !Max));
                                evalboard.ChangeTile(".", i, j); // Remove the sign
                            }
                        }
                    }
                }
                else
                {
                    int bestMove = 100;
                    for (int i = 0; i < evalboard.GetHeight(); i++)
                    {
                        for (int j = 0; j < evalboard.GetWidth(); j++)
                        {
                            if (evalboard.GetTile(i, j) == ".") // If the tile is empty, put your opponent's sign in it
                            {
                                evalboard.ChangeTile(player.Opponent(), i, j);
                                bestMove = Math.Min(bestMove, minimax(evalboard, !Max));
                                evalboard.ChangeTile(".", i, j); // Remove the sign
                            }
                        }
                    }
                }
                return 0;
            }
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
            int NumberOfMoves;

            public Board(string[,] board) // If created in analytical mode, board is preset
            {
                this.board = board;
                NumberOfMoves = 9;
                for (int i = 0; i < board.GetLength(0); i++) // Updates the number of moves left
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] != ".")
                            NumberOfMoves--;
                    }
                }
            }

            public Board() // If created for interactive play, the board is empty
            {
                board = new string[3,3] { { ".", ".", "." }, { ".", ".", "." }, { ".", ".", "." } };
                NumberOfMoves = 9;
            }

            public bool AnyMovesLeft()
            {
                if (NumberOfMoves == 0)
                    return false;
                return true;
            }

            public string[,] GetWholeBoard()
            {
                return board;
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

            public int EvaluateBoard(string sign) // Evaluates board for ... tictactoe
            {
                for (int i = 0; i < board.GetLength(0); i++) // vertical
                {
                    if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    {
                        if (board[i, 0] == sign)
                            return -10;
                        else
                            return 10;
                    }
                }

                for (int i = 0; i < board.GetLength(1); i++) // horizontal
                {
                    if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    {
                        if (board[0, i] == sign)
                            return -10;
                        else
                            return 10;
                    }
                }

                if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) // diagonal \
                {
                    if (board[0, 0] == sign)
                        return -10;
                    else
                        return 10;
                }
                else if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) // diagonal /
                {
                    if (board[0, 2] == sign)
                        return -10;
                    else
                        return 10;
                }
                return 0;
            }

        }

        static void Main(string[] args)
        {
            
        }
    }
}