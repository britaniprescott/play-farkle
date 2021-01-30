using System;

namespace Farkle
{
    public static class FarkleScoring
    {
        const int SINGLE_ONE = 100;
        const int SINGLE_FIVE = 50;

        const int FOUR_OF_A_KIND = 1000;
        const int FIVE_OF_A_KIND = 2000;
        const int SIX_OF_A_KIND  = 3000;

        const int THREE_PAIRS = 1500;
        const int TWO_TRIPLETS = 2500;
        const int FOUR_OF_A_KIND_WITH_A_PAIR = 1500;

        const int ONE_THROUGH_SIX_STRAIGHT = 1500;

        static void ClearDice(int[] dice)
        {
            for (int i = 0; i < 6; ++i)
            {
                dice[i] = 0;
            }
        }

        public static int CountDice(int[] dice)
        {
            int totalDice = 0;

            for (int i = 0; i < 6; ++i)
            {
                totalDice += dice[i];
            }

            return totalDice;
        }

        static int[] OrganizeDice(int[] dice)
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

        public static int AttemptToScoreDice(int[] dice)
        {
            int[] organizedDice = OrganizeDice(dice);
            int numberOfDice = CountDice(dice);

            if (numberOfDice == 6)
            {
                return AttemptToScoreSixDice(organizedDice);
            }
            else if (numberOfDice == 5)
            {
                return AttemptToScoreFiveDice(organizedDice);
            }
            else if (numberOfDice == 4)
            {
                return AttemptToScoreFourDice(organizedDice);
            }
            else if (numberOfDice == 3)
            {
                return AttemptToScoreThreeDice(organizedDice);
            }
            else
            {
                return AttemptToScoreSingleDie(organizedDice);
            }
        }

        public static int GetCurrent(int[] dice)
        {
             return AttemptToScoreDice(dice);
        }

        public static bool GetFarkleStatus(int[] dice)
        {
            if (AttemptToScoreDice(dice) == 0)
            {
                return true;
            }

            return false;
        }

        static int AttemptToScoreSingleDie(int[] dice)
        {
            int rollScore = 0;

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

            return rollScore;
        }

        static int AttemptToScoreThreeDice(int[] dice)
        {
            int rollScore = 0;

            for (int i = 1; i < 6; ++i)
            {
                if (dice[i] == 3)
                {
                    rollScore += 100 * (i + 1);
                    dice[i] = 0;
                }
            }

            return rollScore + AttemptToScoreSingleDie(dice);
        }

        static int AttemptToScoreFourDice(int[] dice)
        {
            int rollScore = 0;

            for (int i = 0; i < 6; ++i)
            {
                if (dice[i] == 4)
                {
                    rollScore += FOUR_OF_A_KIND;
                    dice[i] = 0;
                }
            }

            return rollScore + AttemptToScoreThreeDice(dice);
        }

        static int AttemptToScoreFiveDice(int[] dice)
        {
            int rollScore = 0;

            for (int i = 0; i < 6; ++i)
            {
                if (dice[i] == 5)
                {
                    rollScore += FIVE_OF_A_KIND;
                    dice[i] = 0;
                }
            }

            return rollScore + AttemptToScoreFourDice(dice);
        }

        static int AttemptToScoreSixDice(int[] dice)
        {
            int rollScore = 0;

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
                    return rollScore;
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

            return AttemptToScoreFiveDice(dice);
        }
    }
}
