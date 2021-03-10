using System;

namespace ProbabilityTDD
{
    public class Probability
    {

        //note: P(A & B) = P(A) x P(B)
        public double CalculateProbabilityOfBoth(double probabilityA, double probabilityB)
        {
            if (probabilityA < 0 || probabilityB < 0 || probabilityB > 1 || probabilityA > 1) throw new ArgumentException();
            return probabilityA * probabilityB;
        }

        //note: P(A | B) = P(A) + P(B) – P(A & B)
        public double CalculateProbabilityOfEither(double probabilityA, double probabilityB)
        {
            if (probabilityA < 0 || probabilityB < 0 || probabilityB > 1 || probabilityA > 1) throw new ArgumentException();
            return probabilityA + probabilityB - CalculateProbabilityOfBoth(probabilityA, probabilityB);
        }

        //note: P(!A) = 1 – P(A)
        public double CalculateProbabilityOfInverse(double probability)
        {

            if (probability < 0 || probability > 1) throw new ArgumentException(); 
            return 1 - probability;
        }
    }
}
