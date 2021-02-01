using System;
using System.Collections.Generic;

namespace Farkle
{
    public static class FarkleScoring
    {
        const int SINGLE_ONE = 100;
        const int SINGLE_FIVE = 50;

        const int FOUR_OF_A_KIND = 1000;
        const int FIVE_OF_A_KIND = 2000;
        const int SIX_OF_A_KIND = 3000;

        const int THREE_PAIRS = 1500;
        const int TWO_TRIPLETS = 2500;
        const int FOUR_OF_A_KIND_WITH_A_PAIR = 1500;

        const int ONE_THROUGH_SIX_STRAIGHT = 1500;

        public static int[] ClearDice(int[] dice)
        {
            int[] clearedDice = dice;
            for (int i = 0; i < 6; ++i)
            {
                clearedDice[i] = 0;
            }
            return clearedDice;
        }

        public static int CountDice(int[] dice)
        {
            int totalDice = 0;

            for (int i = 0; i < 6; ++i)
            {
                if (dice[i] > 0)
                {
                    ++totalDice;
                }
            }

            return totalDice;
        }

        static Dictionary<int, int> OrganizeDice(int[] dice)
        {
            int numberOfDice = CountDice(dice);
            Dictionary<int, int> organizedDice = new Dictionary<int, int>()
            {
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0},
                {5, 0},
                {6, 0},
                {0, 0} // unused dice
            };

            for (int dieInDice = 0; dieInDice < 6; dieInDice++)
            {
                ++organizedDice[dice[dieInDice]];
            }

            return organizedDice;
        }

        public static int AttemptToScoreDice(int[] dice)
        {
            Dictionary<int, int> organizedDice = OrganizeDice(dice);
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
                return AttemptToScoreSingles(organizedDice);
            }
        }

        public static int GetRollScore(int[] dice)
        {
            Console.WriteLine("Score From GetRollScore: ", AttemptToScoreDice(dice));
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

        static int AttemptToScoreSingles(Dictionary<int, int> organizedDice)
        {
            return organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
        }

        static int AttemptToScoreThreeDice(Dictionary<int, int> organizedDice)
        {
            for (int i = 1; i < 7; ++i)
            {
                if (organizedDice[i] == 3)
                {
                    if (i != 1)
                    {
                        return i * 100;
                    }
                    return organizedDice[1] * SINGLE_ONE;
                }
            }

            return organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
        }

        static int AttemptToScoreFourDice(Dictionary<int, int> organizedDice)
        {
            for (int i = 1; i < 7; ++i)
            {
                if (organizedDice[i] == 4)
                {
                    return FOUR_OF_A_KIND;
                }
                if (organizedDice[i] == 3)
                {
                    if ((i != 1) && (i != 5))
                    {
                        return i * 100 + organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
                    }
                    if (i == 5)
                    {
                        return i * 100 + organizedDice[1] * SINGLE_ONE;
                    }
                }
            }

            return organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
        }

        static int AttemptToScoreFiveDice(Dictionary<int, int> organizedDice)
        {
            for (int i = 1; i < 7; ++i)
            {
                if (organizedDice[i] == 5)
                {
                    return FIVE_OF_A_KIND;
                }
                if (organizedDice[i] == 4)
                {
                    return FOUR_OF_A_KIND + organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
                }
                if (organizedDice[i] == 3)
                {
                    if ((i != 1) && (i != 5))
                    {
                        return i * 100 + organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
                    }
                    if (i == 5)
                    {
                        return i * 100 + organizedDice[1] * SINGLE_ONE;
                    }
                }
            }

            return organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
        }

        static int AttemptToScoreSixDice(Dictionary<int, int> organizedDice)
        {
            for (int i = 1; i < 7; ++i)
            {
                if (organizedDice[i] == 6)
                {
                    return SIX_OF_A_KIND;
                }
                if (organizedDice[i] == 5)
                {
                    return FIVE_OF_A_KIND + organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
                }
            }

            int threeOfAKindNumber = 0;
            int ones = 0;
            int twos = 0;
            int threes = 0;
            int fours = 0;

            for (int i = 1; i < 7; ++i)
            {
                if (organizedDice[i] == 4)
                {
                    ++fours;
                }
                else if (organizedDice[i] == 3)
                {
                    threeOfAKindNumber = i;
                    ++threes;
                }
                else if (organizedDice[i] == 2)
                {
                    ++twos;
                }
                else if (organizedDice[i] == 1)
                {
                    ++ones;
                }
            }

            if (twos == 3)
            {
                return THREE_PAIRS;
            }
            if (threes == 2)
            {
                return TWO_TRIPLETS;
            }
            if ((twos == 1) && (fours == 1))
            {
                return FOUR_OF_A_KIND_WITH_A_PAIR;
            }
            if (ones == 6)
            {
                return ONE_THROUGH_SIX_STRAIGHT;
            }

            if (fours == 1)
            {
                return FOUR_OF_A_KIND + organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
            }

            if (threes == 1)
            {
                if ((threeOfAKindNumber != 1) && (threeOfAKindNumber != 5))
                {
                    return threeOfAKindNumber * 100 + organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
                }
                if (threeOfAKindNumber == 1)
                {
                    return 300 + organizedDice[5] * SINGLE_FIVE;
                }
                if (threeOfAKindNumber == 5)
                {
                    return threeOfAKindNumber * 100 + organizedDice[1] * SINGLE_ONE;
                }
            }

            return organizedDice[1] * SINGLE_ONE + organizedDice[5] * SINGLE_FIVE;
        }
    }
}