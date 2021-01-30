using System;

namespace Farkle
{
    public class FarkleDiceGame
    {
        private
        const int SINGLE_ONE = 100;
        const int SINGLE_FIVE = 50;

        const int FOUR_OF_A_KIND = 1000;
        const int FIVE_OF_A_KIND = 2000;
        const int SIX_OF_A_KIND  = 3000;

        const int THREE_PAIRS = 1500;
        const int TWO_TRIPLETS = 2500;
        const int FOUR_OF_A_KIND_WITH_A_PAIR = 1500;

        const int ONE_THROUGH_SIX_STRAIGHT = 1500;

        int remainingDice;
        int rollScore;
        int currentTurnScore;

        int[] rolledDice  = new int[6];
        //int[] savedDice   = new int[6];
        //int[] diceToCheck = new int[6];

        bool youFarkled;

        public FarkleDiceGame()
        {
            youFarkled = false;
            remainingDice = 6;
            rollScore = 0;
            currentTurnScore = 0;
        }

        void PrintDiceRoll(int[] dice)
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

        void ClearDice(int[] dice)
        {
            for (int i = 0; i < 6; ++i)
            {
                rolledDice[i] = 0;
                //savedDice[i] = 0;
                dice[i] = 0;
            }
        }

        int CountDice(int[] dice)
        {
            int totalDice = 0;

            for (int i = 0; i < 6; ++i)
            {
                totalDice += dice[i];
            }

            return totalDice;
        }

        int[] OrganizeDice(int[] dice)
        {
            int[] organizedDice = new int[6];

            for (int dieInDice = 0; dieInDice < 6; ++dieInDice)
            {
                for (int possibleDieNumber = 1; possibleDieNumber < 7; ++possibleDieNumber)
                {
                    if (dice[dieInDice] == possibleDieNumber)
                    {
                        ++organizedDice[possibleDieNumber - 1];
                    }
                }
            }
            return organizedDice;
        }

        void AttemptToScoreDice(int[] dice)
        {
            int[] organizedDice = OrganizeDice(dice);
            int numberOfDice = CountDice(dice);

            rollScore = 0;

            if (numberOfDice == 6)
            {
                AttemptToScoreSixDice(organizedDice);
            }
            else if (numberOfDice == 5)
            {
                AttemptToScoreFiveDice(organizedDice);
            }
            else if (numberOfDice == 4)
            {
                AttemptToScoreFourDice(organizedDice);
            }
            else if (numberOfDice == 3)
            {
                AttemptToScoreThreeDice(organizedDice);
            }
            else
            {
                AttemptToScoreSingleDie(organizedDice);
            }

            if (rollScore == 0)
            {
                currentTurnScore = 0;
                youFarkled = true;
            }
            else
            {
                youFarkled = false;
            }
        }

        void AttemptToScoreSingleDie(int[] dice)
        {
            for (int i = 0; i < 6; ++i)
            {
                if (dice[i] == 1)
                {
                    rollScore += dice[i] * SINGLE_ONE;
                    dice[i] = 0;
                }
                else if (dice[i] == 5)
                {
                    rollScore += dice[i] * SINGLE_FIVE;
                    dice[i] = 0;
                }
            }
        }

        void AttemptToScoreThreeDice(int[] dice)
        {
            for (int i = 1; i < 6; ++i)
            {
                if (dice[i] == 3)
                {
                    rollScore += 100 * (i + 1);
                    dice[i] = 0;
                    return;
                }
            }

            AttemptToScoreSingleDie(dice);
        }

        void AttemptToScoreFourDice(int[] dice)
        {
            for (int i = 0; i < 6; ++i)
            {
                if (dice[i] == 4)
                {
                    rollScore += FOUR_OF_A_KIND;
                    dice[i] = 0;
                    return;
                }
            }
            AttemptToScoreThreeDice(dice);
        }

        void AttemptToScoreFiveDice(int[] dice)
            {
                for (int i = 0; i < 6; ++i)
                {
                    if (dice[i] == 5)
                    {
                        rollScore += FIVE_OF_A_KIND;
                        dice[i] = 0;
                        return;
                    }
                }

                AttemptToScoreFourDice(dice);
        }

        void AttemptToScoreSixDice(int[] dice)
        {
            int ones = 0;
            int twos = 0;
            int threes = 0;
            int fours = 0;
            for (int i = 0; i < 6; ++i)
            {
                if (dice[i] == 6)
                {
                    rollScore += SIX_OF_A_KIND;
                    dice[i] = 0;
                    return;
                }
                else if (dice[i] == 4)
                {
                    ++fours;
                }
                else if (dice[i] == 3)
                {
                    ++threes;
                }
                else if (dice[i] == 2)
                {
                    ++twos;
                }
                else if (dice[i] == 1)
                {
                    ++ones;
                }
            }

            if (threes == 2)
            {
                rollScore += THREE_PAIRS;
                ClearDice(dice);
            }
            else if (twos == 3)
            {
                rollScore += TWO_TRIPLETS;
                ClearDice(dice);
            }
            else if ((twos == 1) && (fours == 1))
            {
                rollScore += FOUR_OF_A_KIND_WITH_A_PAIR;
                ClearDice(dice);
            }
            else if (ones == 6)
            {
                rollScore += ONE_THROUGH_SIX_STRAIGHT;
                ClearDice(dice);
            }

            AttemptToScoreFiveDice(dice);
        }

        public

        void RollDice()
        {
            Random randomBetween1And6 = new Random();
            int diceRoll;

            for (int i = 0; i < remainingDice; ++i)
            {
                diceRoll = randomBetween1And6.Next(1, 7);
                rolledDice[i] = diceRoll;
            }

            AttemptToScoreDice(rolledDice);

            PrintDiceRoll(rolledDice);

            if (youFarkled)
            {
                Console.WriteLine("YOU FARKLED! NEXT PLAYER'S TURN");
                // NextPlayerTurn(false);
            }
        }

        void SaveDice(int[] diceToSave)
        {
            int numberOfDiceRolled = CountDice(rolledDice);
            int numberOfDiceSaved = CountDice(diceToSave);

            if (!youFarkled)
            {
                AttemptToScoreDice(diceToSave);
                currentTurnScore += rollScore;

                if ((numberOfDiceRolled - numberOfDiceSaved) == 0)
                {
                    remainingDice = 6;
                }
                else
                {
                    remainingDice = numberOfDiceRolled - numberOfDiceSaved;
                }
            }
        }

        //void Pass()

        // void ContinueRolling(bool continue)

        int[] GetRolledDice()
        {
            return rolledDice;
        }

        bool GetFarkleStatus()
        {
            return youFarkled;
        }

        int GetCurrentTurnScore()
        {
            return currentTurnScore;
        }

        int GetRemainingDiceToRoll()
        {
            return remainingDice;
        }
    }
}
