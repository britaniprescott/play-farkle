using System;
using System.Collections.Generic;

namespace Farkle
{
    public class FarklePlay
    {
        const int MINIMUM_FIRST_SCORE = 500;
        const int SCORE_TO_WIN = 10000;

        Dictionary<int, int> playerScores = new Dictionary<int, int>();
        int playerTurn;
        int remainingDice;
        int currentTurnScore;
        int[] rolledDice = new int[6];

        bool lastRound;
        bool endOfGame;

        public FarklePlay()
        {
            lastRound = false;
            endOfGame = false;
            playerTurn = 1;
            remainingDice = 6;
        }

        private void BeginGame(int playerCount)
        {
            //string name;
            for (int i = 1; i < playerCount + 1; ++i)
            {
                //Console.WriteLine("Enter Player {0} Name:", i);
                //name = Console.ReadLine();
                playerScores.Add(i, 0);
            }

            foreach (KeyValuePair<int, int> kvp in playerScores)
            {
                Console.WriteLine(kvp);
            }
        }

        private void EndGame()
        {
            Console.WriteLine("End Game");
        }

        public void Play()
        {
            bool continuePrevious = false;

            Console.WriteLine("Start New Farkle Game: How many players?");
            int playerCount = Int32.Parse(Console.ReadLine());
            BeginGame(playerCount);

            while (!endOfGame)
            {
                TakeTurn(continuePrevious);

                if (currentTurnScore == 0)
                {
                    continuePrevious = false;
                    currentTurnScore = 0;
                }
                else
                {
                    Console.WriteLine("Would you like to continue the previous player's turn?");
                    string continuePreviousPlayerTurn = Console.ReadLine();
                    if (continuePreviousPlayerTurn == "yes")
                    {
                        continuePrevious = true;
                    }
                    else
                    {
                        continuePrevious = false;
                    }
                }

                NextPlayerTurn(continuePrevious, playerCount);

                for (int i = 1; i < playerCount + 1; ++i)
                {
                    Console.WriteLine("Player {0} Score: " + playerScores[i], i);
                }
            }
        }

        private void TakeTurn(bool continuePrevious)
        {
            Console.WriteLine("Player {0} Turn:", playerTurn);

            //bool keepRolling = true;
            string continueTurn;

            while (true)
            {
                RollDice();

                if (Farkle.FarkleScoring.GetFarkleStatus(rolledDice))
                {
                    Console.WriteLine("YOU FARKLED! NEXT PLAYER'S TURN");
                    currentTurnScore = 0;
                    remainingDice = 6;
                    break;//NextPlayerTurn(false);
                }
                else
                {
                    int[] savedDice = DiceToSave();

                    remainingDice = CalculateRemainingDice(savedDice);

                    Console.WriteLine("Remaining Dice: " + remainingDice);

                    currentTurnScore += Farkle.FarkleScoring.GetRollScore(savedDice);

                    Console.WriteLine("Current Turn Score: ", currentTurnScore);

                    if (playerScores[playerTurn] == 0)
                    {
                        if (currentTurnScore > MINIMUM_FIRST_SCORE)
                        {
                            Console.WriteLine("You are currently at {0} points for this turn. Would you like to continue rolling?", currentTurnScore);
                            continueTurn = Console.ReadLine();
                            if (continueTurn == "yes")
                            {
                                RollDice();
                            }
                            else
                            {
                                AddToPlayerScore(currentTurnScore);
                                break;//NextPlayerTurn();
                            }
                        }
                        else
                        {
                            Console.WriteLine("You are currently at {0} points for this turn. You must score at least 500 on your first turn. Continue rolling.", currentTurnScore);

                        }
                    }
                    else if (currentTurnScore > 0)
                    {
                        Console.WriteLine("You are currently at {0} points for this turn. Would you like to continue rolling?", currentTurnScore);
                        continueTurn = Console.ReadLine();
                        if (continueTurn == "yes")
                        {
                            RollDice();
                        }
                        else
                        {
                            AddToPlayerScore(currentTurnScore);
                            break;//NextPlayerTurn();
                        }
                    }
                }
            }
        }

        private void NextPlayerTurn(bool continueTurn, int playerCount)
        {
            playerTurn = playerTurn % playerCount + 1;
        }

        private void RollDice()
        {
            rolledDice = Farkle.FarkleScoring.ClearDice(rolledDice);
            Random randomBetween1And6 = new Random();
            int diceRoll;

            Console.WriteLine("Remaining Dice in RollDice(): " + remainingDice);

            for (int i = 0; i < remainingDice; ++i)
            {
                diceRoll = randomBetween1And6.Next(1, 7);
                rolledDice[i] = diceRoll;
            }

            PrintDiceRoll(rolledDice);

            /*if (Farkle.FarkleScoring.GetFarkleStatus(rolledDice))
            {
                Console.WriteLine("YOU FARKLED! NEXT PLAYER'S TURN");
                // NextPlayerTurn(false);
            }*/
        }

        private void AddToPlayerScore(int score)
        {
            if ((currentTurnScore >= MINIMUM_FIRST_SCORE) || (playerScores[playerTurn] > 0))
            {
                playerScores[playerTurn] += score;
            }

            if (playerScores[playerTurn] >= SCORE_TO_WIN)
            {
                lastRound = true;
                if (playerTurn == playerScores.Count)
                {
                    Console.WriteLine("Game is Over. Player {0} Wins!", playerTurn);
                    endOfGame = true;
                }
                else
                {
                    Console.WriteLine("Each player after Player {0} has one more chance to score.");
                    //NextPlayerTurn();
                }
            }
            else
            {
                //NextPlayerTurn();
            }
        }

        static void PrintDiceRoll(int[] dice)
        {
            string diceRoll = "";
            for (int i = 0; i < 6; ++i)
            {
                diceRoll = diceRoll + (dice[i]);
                if (i != 5)
                {
                    diceRoll = diceRoll + " ";
                }
            }

            Console.WriteLine(diceRoll);
        }

        public int[] DiceToSave()
        {
            int[] diceToSave = new int[6];
            int numberOfDiceRolled = Farkle.FarkleScoring.CountDice(rolledDice);

            for (int i = 0; i < numberOfDiceRolled; ++i)
            {
                Console.WriteLine("Would you like to save die {0}?", i + 1);
                if (Console.ReadLine() == "yes")
                {
                    diceToSave[i] = rolledDice[i];
                }
            }

            return diceToSave;
        }

        public int CalculateRemainingDice(int[] diceSaved)
        {
            if (Farkle.FarkleScoring.GetFarkleStatus(rolledDice))
            {
                return 6;
            }
            else
            {
                int numberOfDiceRolled = Farkle.FarkleScoring.CountDice(rolledDice);
                int numberOfDiceSaved = Farkle.FarkleScoring.CountDice(diceSaved);

                if ((numberOfDiceRolled - numberOfDiceSaved) == 0)
                {
                    return 6;
                }
                else
                {
                    return numberOfDiceRolled - numberOfDiceSaved;
                }
            }
        }
    }
}