namespace UserStorage.IdentifiersGeneration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PrimeNumbersGenerator : MarshalByRefObject, IIdentifiersGenerator
    {
        private const int MaxNumOfNumbers = 10000;
        private IEnumerator<int> numbers;
        
        public PrimeNumbersGenerator(int lastGeneratedNumber = 1)
        {
            this.numbers = this.GetNewPrimeId(lastGeneratedNumber); 
        } 

        public void ResetGenerator()
        {
            this.numbers = this.GetNewPrimeId(1);
        }

        public int GenerateNewNumber()
        {
            bool isPossibleToGetNext = this.numbers.MoveNext();
            if (!isPossibleToGetNext)
            {
                this.ResetGenerator();
                this.numbers.MoveNext();
            }

            return this.numbers.Current;
        }

        private IEnumerator<int> GetNewPrimeId(int lastGeneratedNumber)
        {
            int limit = this.ApproximateNthPrime(MaxNumOfNumbers);
            BitArray bits = this.SieveOfEratosthenes(limit);
            List<int> primes = new List<int>();
            for (int i = lastGeneratedNumber + 1, numOfFound = 0; i < limit && numOfFound < MaxNumOfNumbers; i++)
            {
                if (bits[i])
                {
                    yield return i;
                    numOfFound++;
                }
            }
        }

        private BitArray SieveOfEratosthenes(int limit)
        {
            BitArray bits = new BitArray(limit + 1, true);
            bits[0] = false;
            bits[1] = false;
            for (int i = 0; i * i <= limit; i++)
            {
                if (bits[i])
                {
                    for (int j = i * i; j <= limit; j += i)
                    {
                        bits[j] = false;
                    }
                }
            }

            return bits;
        }
        
        private int ApproximateNthPrime(int nn)
        {
            double n = (double)nn;
            double p;
            if (nn >= 7022)
            {
                p = (n * Math.Log(n)) + (n * (Math.Log(Math.Log(n)) - 0.9385));
            }
            else if (nn >= 6)
            {
                p = (n * Math.Log(n)) + (n * Math.Log(Math.Log(n)));
            }
            else if (nn > 0)
            {
                p = new int[] { 2, 3, 5, 7, 11 }[nn - 1];
            }
            else
            {
                p = 0;
            }

            return (int)p;
        }
    }
}
