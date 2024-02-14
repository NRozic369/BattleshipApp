using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary;
using BattleshipLibrary.Models;

namespace Battleship
{
     class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                // Display grid from activePlayer on where they fired
                DisplayShotGrid(activePlayer);

                // Ask activePlayer for a shot
                // Determine if it is a valid shot
                // Determine shot results
                RecordPlayerShot(activePlayer, opponent);

                // Determine if the game is over
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                // If over, set activePlayer as winner
                // else, swap positions (activePlayer is opponent)
                if (doesGameContinue == true)
                {
                    // Temp variable
                    //PlayerInfoModel tempHolder = opponent;
                    //opponent = activePlayer;
                    //activePlayer = tempHolder;

                    // Tuple
                    (activePlayer, opponent) = (opponent, activePlayer);

                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UsersName} for winning!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            // Asks for a shot (ask for "B2")
            // Determine what row and column that is - split it apart
            // Determine if that is a valid shot
            // Go back to beginning if not a valid shot
            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot(activePlayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    isValidShot = false;
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Please try again.");
                }
            } while (isValidShot == false);


            // Determine shot results
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            // Record results
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

            //Display results
            DisplayShotResults(row, column, isAHit, activePlayer);
        }

        private static void DisplayShotResults(string row, int column, bool isAHit, PlayerInfoModel activePlayer)
        {
            Console.Clear();
            if (isAHit)
            {
                Console.WriteLine($"{ activePlayer.UsersName }, {row}{column} is a hit!");
                Console.WriteLine("____________________________________________________");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"{activePlayer.UsersName}, {row}{column} is a miss.");
                Console.WriteLine("____________________________________________________");
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"{ player.UsersName }, please enter you shot selection: ");
            string output = Console.ReadLine();

            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;
            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }

            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship");
            Console.WriteLine("Created by Nika Rozic");
            Console.WriteLine();
        }
        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for { playerTitle }");

            // Ask user for a name
            output.UsersName = AskForUsersName();
            // Load up the shot grid
            GameLogic.InitializeGrid(output);

            // Ask user for their 5 ship placements
            PlaceShips(output);

            // Clear
            Console.Clear();

            return output;
        }

        private static string AskForUsersName()
        {
            Console.Write("What is your name: ");
            string output = Console.ReadLine();
            Console.WriteLine();

            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            BattleshipInfo(model);
            do
            {
                Console.Write($"Where do you want to place ship number { model.ShipLocations.Count + 1 }: ");
                string location = Console.ReadLine();

                bool isValidLocation = false;
                try
                {
                    isValidLocation = GameLogic.PlaceShip(model, location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: " + ex.Message);
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                    Console.WriteLine();
                }
            } while (model.ShipLocations.Count < 5);
        }

        private static void BattleshipInfo(PlayerInfoModel model)
        {
            Console.WriteLine($"{model.UsersName}, possible ship locations are A through E for rows and 1 through 5 for columns!");
            Console.WriteLine("Info: in this game, ships are a size of one grid spot and each player has 5 ships.");
            Console.WriteLine();
        }
    }
}
