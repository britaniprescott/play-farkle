using System;
using System.Collections.Generic;

namespace Farkle
{
    public class FarklePlay
    {
        const int MINIMUM_FIRST_SCORE = 500;
        const int SCORE_TO_WIN = 10000;

        IDictionary<int, int> playerScores = new Dictionary<int, int>();
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

        public void BeginGame(int playerCount)
        {
            //string name;
            for (int i = 1; i < playerCount; ++i)
            {
                //Console.WriteLine("Enter Player {1} Name:", i);
                //name = Console.ReadLine();
                playerScores.Add(i, 0);
            }
        }

        public void EndGame()
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
                }
                else
                {
                    Console.WriteLine("Would you like to continue the previous player's turn?");
                    string continuePreviousPlayerTurn = Console.ReadLine();
                    if (continuePreviousPlayerTurn == "yes")
                    {
                        continuePrevious = true;
                        currentTurnScore = 0;
                    }
                    else
                    {
                        continuePrevious = false;
                    }
                }

                NextPlayerTurn(continuePrevious, playerCount);
            }
        }

        public void TakeTurn(bool continuePrevious)
        {
            bool keepRolling = true;
            string continueTurn;

            while (keepRolling)
            {
                RollDice();

                if (Farkle.FarkleScoring.GetFarkleStatus(rolledDice))
                {
                    Console.WriteLine("YOU FARKLED! NEXT PLAYER'S TURN");
                    //NextPlayerTurn(false);
                }
                else
                {
                    int[] savedDice = DiceToSave();

                    remainingDice = CalculateRemainingDice(savedDice);

                    currentTurnScore = Farkle.FarkleScoring.AttemptToScoreDice(savedDice);

                    if (currentTurnScore > MINIMUM_FIRST_SCORE)
                    {
                        Console.WriteLine("Would you like to continue rolling?");
                        continueTurn = Console.ReadLine();
                        if (continueTurn == "yes")
                        {
                            RollDice();
                        }
                        else
                        {
                            AddToPlayerScore(currentTurnScore);
                            //NextPlayerTurn();
                        }
                    }
                }
            }
        }

        public void NextPlayerTurn(bool continueTurn, int playerCount)
        {
            playerTurn = playerTurn % playerCount + 1;
        }

        public void RollDice()
        {
            Random randomBetween1And6 = new Random();
            int diceRoll;

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

        void AddToPlayerScore(int score)
        {
            if (playerScores[playerTurn] >= 500)
            {
                playerScores[playerTurn] += score;
            }

            if (playerScores[playerTurn] >= SCORE_TO_WIN)
            {
                lastRound = true;
                if (playerTurn == playerScores.Count)
                {
                    Console.WriteLine("Game is Over. Player {1} Wins!", playerTurn);
                    endOfGame = true;
                }
                else
                {
                    Console.WriteLine("Each player after Player {1} has one more chance to score.");
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
                Console.WriteLine("Would you like to save die {1}?", i + 1);
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
