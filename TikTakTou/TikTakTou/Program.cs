using System;

namespace tictactoe
{
    internal class Program
    {
        class TurnManager // Manages turns between the two players
        {
            Player P1 = null;
            AI P2 = null;
            AI P3 = null;
            Board board;
            bool whichOne; // If true, P1 plays, otherwise P2 plays
            bool continueGame = true;

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

            public TurnManager(AI p2, AI p3, Board board, bool whichone)
            {
                whichOne = whichone;
                P2 = p2;
                P3 = p3;
                this.board = board;
            }

            void AIsTurn(AI ai)
            {
                int[] Move = ai.Play();
                board.ChangeTile(ai.GetPlayer().Sign(), Move[0], Move[1], false);
            }

            public bool Continue()
            {
                return continueGame;
            }

            void PlayersTurn()
            {
                Console.WriteLine("Player's turn:");
                while (true)
                {
                    string[] Move = Console.ReadLine().Split(',');
                    if (board.ChangeTile(P1.Sign(), int.Parse(Move[0]) - 1, int.Parse(Move[1]) - 1, false))
                        break;
                    else
                        Console.WriteLine("There already is a sign. Choose another one.");
                }
            }

            public void ProgressAIGame()
            {
                if (whichOne)
                {
                    AIsTurn(P2);
                    if (board.EvaluateBoard(P2.GetPlayer()) == -10)
                    {
                        continueGame = false;
                    }
                }
                else
                {
                    AIsTurn(P3);
                    if (board.EvaluateBoard(P3.GetPlayer()) == -10)
                    {
                        continueGame = false;
                    }
                }
                whichOne = !whichOne;
            }

            public void ProgressGame()
            {
                if (whichOne)
                {
                    PlayersTurn();
                    if (board.EvaluateBoard(P1) == 10)
                    {
                        Console.WriteLine("Player 2 wins!");
                        continueGame = false;
                    }
                }
                else
                {
                    AIsTurn(P2);
                    if (board.EvaluateBoard(P1) == -10)
                    {
                        Console.WriteLine("Player 1 wins!");
                        continueGame = false;
                    }
                }

                whichOne = !whichOne;
            }
        }

        class Player
        {
            readonly string sign; // x or o
            readonly string opponent; // Opponent's sign
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
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
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
                int[] bestMove = new int[2];
                int bestMoveValue = -100;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board.GetTile(i, j) == ".") // If the tile is empty, put your sign in it
                        {
                            board.ChangeTile(player.Sign(), i, j, false);
                            int currentMoveValue = Minimax(board, false);
                            board.ChangeTile(".", i, j, true); // Remove the sign

                            if (currentMoveValue > bestMoveValue)
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

            int Minimax(Board board, bool Max) // Evaluate board; Max = true for max's turn, false for min's turn
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
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (board.GetTile(i,j) == ".") // If the tile is empty, put your sign in it
                            {
                                board.ChangeTile(player.Sign(), i, j, false);
                                bestMove = Math.Max(bestMove, Minimax(board, !Max));
                                board.ChangeTile(".", i, j, true); // Remove the sign
                            }
                        }
                    }
                    return bestMove;
                }
                else
                {
                    int bestMove = 100;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (board.GetTile(i, j) == ".") // If the tile is empty, put your opponent's sign in it
                            {
                                board.ChangeTile(player.Opponent(), i, j, false);
                                bestMove = Math.Min(bestMove, Minimax(board, !Max));
                                board.ChangeTile(".", i, j, true); // Remove the sign
                            }
                        }
                    }
                    return bestMove;
                }
            }

            public int[] Play()
            {
                return FindBestMove();
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

            public Board CreateBoard()
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
                for (int i = 0; i < 3; i++) // vertical
                {
                    if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    {
                        if (board[i, 0] == player.Sign()) // If there is the player's sign, return negative value
                            return -10;
                        else if (board[i, 0] == player.Opponent())
                            return 10;
                    }
                }

                for (int i = 0; i < 3; i++) // horizontal
                {
                    if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    {
                        if (board[0, i] == player.Sign())
                            return -10;
                        else if (board[0, i] == player.Opponent())
                            return 10;
                    }
                }

                if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) // diagonal \
                {
                    if (board[0, 0] == player.Sign())
                        return -10;
                    else if (board[0, 0] == player.Opponent())
                        return 10;
                }
                else if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) // diagonal /
                {
                    if (board[0, 2] == player.Sign())
                        return -10;
                    else if (board[0, 2] == player.Opponent())
                        return 10;
                }
                return 0;
            }

        }

        static void Main(string[] args)
        {
            string input = Console.ReadLine(), opponentsign = "o";
            bool interactive = false;
            if (! int.TryParse(input, out int numberofplays))
            {
                interactive = true;
                if (input == "o")
                {
                    opponentsign = "x";
                }
            }

            if (interactive)
            {
                Console.WriteLine("The goal of reverse Tic-Tac-Toe is to NOT form a Tic-Tac-Toe.\nTo place your sign, write the coordinates like this: 'x,y', where 'x' is a column number and y is a row number.\nBoth are in range [1,3].");
                Player player1 = new(input, false);
                Player player2 = new(opponentsign, true);
                Board gameBoard = new Board();
                AI opponent = new(player2, gameBoard);
                TurnManager manager = new(player1, opponent, gameBoard);
                Printer printer = new(gameBoard);
                while (gameBoard.AnyMovesLeft() && manager.Continue())
                {
                    printer.PrintBoard();
                    Console.WriteLine();
                    manager.ProgressGame();
                }
                if (gameBoard.EvaluateBoard(player1) == 0)
                {
                    Console.WriteLine("It's a draw!");
                }
            }
            else // Analytical mode
            {
                string[] results = new string[numberofplays];
                int resultPointer = 0;
                Loader loader = new();
                for (int n = 0; n < numberofplays; n++)
                {
                    Board gameBoard = loader.CreateBoard();
                    int noOfX = 0, noOfO = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (gameBoard.GetTile(i, j) == "x")
                                noOfX++;
                            else if (gameBoard.GetTile(i, j) == "o")
                                noOfO++;
                        }
                    }

                    //Decide if x or o starts
                    bool whichone = false;
                    if (noOfX == noOfO)
                    {
                        whichone = true;
                    }

                    // Initialize all needed objects
                    Player player1 = new("x", true);
                    Player player2 = new("o", true);
                    AI ai1 = new(player1, gameBoard);
                    AI ai2 = new(player2, gameBoard);
                    TurnManager manager = new(ai1, ai2, gameBoard, whichone);

                    // Simulate the game
                    while (gameBoard.AnyMovesLeft() && manager.Continue())
                    {
                        manager.ProgressAIGame();
                    }

                    //Evaluate the outcome and save it to results
                    if (gameBoard.EvaluateBoard(player1) == 0)
                    {
                        results[resultPointer] = "N";
                        resultPointer++;
                    }
                    else if (gameBoard.EvaluateBoard(player1) == 10)
                    {
                        if (player1.Sign() == "x")
                        {
                            results[resultPointer] = "X";
                        }
                        else
                        {
                            results[resultPointer] = "O";
                        }
                        resultPointer++;
                    }
                    else if ((gameBoard.EvaluateBoard(player2) == 10))
                    {
                        if (player2.Sign() == "x")
                        {
                            results[resultPointer] = "X";
                        }
                        else
                        {
                            results[resultPointer] = "O";
                        }
                        resultPointer++;
                    }
                }

                for (int i = 0; i < results.Length; i++) // Print results
                {
                    Console.Write(results[i]);
                }
            }
        }
    }
}