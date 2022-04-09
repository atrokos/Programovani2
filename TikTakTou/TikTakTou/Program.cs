using System;

namespace tictactoe
{
    internal class Program
    {
        class TurnManager // Manages turns between the two players
        {
            Player P1;
            AI P2;
            Board board;
            bool whichOne; // If true, P1 plays, otherwise P2 plays

            public TurnManager(Player p1, AI p2, Board board)
            {
                P1 = p1;
                P2 = p2;
                this.board = board;
                if (P1.Sign() == "x")
                    whichOne = true;
                else
                    whichOne = false;
            }

            void AITurn()
            {
                int[] Move = P2.Play();
                board.ChangeTile(P2.GetPlayer().Sign(), Move[0], Move[1], false);
            }

            void PlayerTurn()
            {
                Console.WriteLine("Player's turn:");
                while (true)
                {
                    string[] Move = Console.ReadLine().Split(',');
                    if (board.ChangeTile(P1.Sign(), Convert.ToInt32(Move[0]) - 1, Convert.ToInt32(Move[1]) - 1, false))
                        break;
                    else
                        Console.WriteLine("There already is a sign. Choose another one.");
                }
            }

            public void ProgressGame()
            {
                if (whichOne)
                    PlayerTurn();
                else
                    AITurn();
                whichOne = !whichOne;
            }
        }

        class Player
        {
            string sign; // x or o
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

            public bool AreYouAI()
            {
                return isAI;
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

        class Printer
        {
            Board board;

            public Printer(Board board)
            {
                this.board = board;
            }

            public void PrintBoard()
            {
                for (int i = 0; i < board.GetHeight(); i++)
                {
                    for (int j = 0; j < board.GetWidth(); j++)
                    {
                        Console.Write(board.GetTile(i, j));
                    }
                    Console.WriteLine();
                }
            }
        }

        class AI
        {
            readonly Player player;
            Board board;

            public AI(Player player, Board board)
            {
                this.player = player;
                this.board = board;
            }

            int[] FindBestMove()
            {
                int[] bestMove = new int[2] { -1, -1 };
                int bestMoveValue = -1;
                int currentMoveValue = -1;
                for (int i = 0; i < board.GetHeight(); i++)
                {
                    for (int j = 0; j < board.GetWidth(); j++)
                    {
                        if (board.GetTile(i, j) == ".") // If the tile is empty, put your sign in it
                        {
                            board.ChangeTile(player.Sign(), i, j, false);
                            currentMoveValue = Minimax(board, false, 0);
                            board.ChangeTile(".", i, j, true); // Remove the sign

                            if (currentMoveValue < bestMoveValue || bestMoveValue == -1)
                            {
                                bestMoveValue = currentMoveValue;
                                bestMove[0] = i;
                                bestMove[1] = j;
                            }
                        }
                    }
                }
                return bestMove;
            }

            int Minimax(Board board, bool Max, int depth) // Evaluate evalboard; Max = true for max's turn, false for min's turn
            {
                int currentValue = board.EvaluateBoard(player);
                if (currentValue == 10)
                    return currentValue;

                if (currentValue == -10)
                    return currentValue;

                if (!board.AnyMovesLeft())
                    return 0;

                if (Max)
                {
                    int bestMove = -100;
                    for (int i = 0; i < board.GetHeight(); i++)
                    {
                        for (int j = 0; j < board.GetWidth(); j++)
                        {
                            if (board.GetTile(i,j) == ".") // If the tile is empty, put your sign in it
                            {
                                board.ChangeTile(player.Opponent(), i, j, false);
                                bestMove = Math.Max(bestMove, Minimax(board, !Max, depth+1));
                                board.ChangeTile(".", i, j, true); // Remove the sign
                            }
                        }
                    }
                    return bestMove;
                }
                else
                {
                    int bestMove = 100;
                    for (int i = 0; i < board.GetHeight(); i++)
                    {
                        for (int j = 0; j < board.GetWidth(); j++)
                        {
                            if (board.GetTile(i, j) == ".") // If the tile is empty, put your opponent's sign in it
                            {
                                board.ChangeTile(player.Sign(), i, j, false);
                                bestMove = Math.Min(bestMove, Minimax(board, !Max, depth + 1));
                                board.ChangeTile(".", i, j, true); // Remove the sign
                            }
                        }
                    }
                    return bestMove;
                }
            }

            public int[] Play()
            {
                int[] Move = FindBestMove();
                return Move;
            }

            public Player GetPlayer()
            {
                return player;
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

            public Board(string[,] board) // If created in analytical mode, the board is created manually
            {
                this.board = board;
            }

            public Board() // If created for interactive play, the board is empty
            {
                board = new string[3,3] { { ".", ".", "." }, { ".", ".", "." }, { ".", ".", "." } };
            }

            public bool AnyMovesLeft()
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i,j] == ".")
                            return true;
                    }
                }
                return false;
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

            public bool ChangeTile(string what, int h, int w, bool overwrite) //Changes tile at h,w coordinates to what; returns true if possible, false if not
            {
                if (overwrite)
                {
                    board[h, w] = what;
                    return true;
                }
                else
                {
                    if (board[h, w] == ".")
                    {
                        board[h, w] = what;
                        return true;
                    }
                    return false;
                }
            }

            public int EvaluateBoard(Player player) // Evaluates board for ... tictactoe
            {
                for (int i = 0; i < board.GetLength(0); i++) // vertical
                {
                    if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    {
                        if (board[i, 0] == player.Sign()) // If there is the player's sign, return negative value
                            return 10;
                        else if (board[i, 0] == player.Opponent())
                            return -10;
                    }
                }

                for (int i = 0; i < board.GetLength(1); i++) // horizontal
                {
                    if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    {
                        if (board[0, i] == player.Sign())
                            return 10;
                        else if (board[0, i] == player.Opponent())
                            return -10;
                    }
                }

                if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) // diagonal \
                {
                    if (board[0, 0] == player.Sign())
                        return 10;
                    else if (board[0, 0] == player.Opponent())
                        return -10;
                }
                else if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) // diagonal /
                {
                    if (board[0, 2] == player.Sign())
                        return 10;
                    else if (board[0, 2] == player.Opponent())
                        return -10;
                }
                return 0;
            }

        }

        static void Main(string[] args)
        {
            
        }
    }
}