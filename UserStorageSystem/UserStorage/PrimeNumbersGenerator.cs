using System;
using System.Collections;
using System.Collections.Generic;

namespace UserStorage
{
    // TODO: try to make it more useful, fast and remove constant limit :(
    public class PrimeNumbersGenerator : IIdentifiersGenerator
    {
        private const int maxNumOfNumbers = 1000;
        private IEnumerator<int> numbers;
        
        public PrimeNumbersGenerator()
        {
            numbers = GetNewPrimeId(); 
        } 

        public void ResetGenerator()
        {
            numbers = GetNewPrimeId();
        }

        public int GenerateNewNumber()
        {
            bool isPossibleToGetNext = numbers.MoveNext();
            if (!isPossibleToGetNext)
            {
                ResetGenerator();
                numbers.MoveNext();
            }
            return numbers.Current;
        }

        private IEnumerator<int> GetNewPrimeId()
        {
            int limit = ApproximateNthPrime(maxNumOfNumbers);
            BitArray bits = SieveOfEratosthenes(limit);
            List<int> primes = new List<int>();
            for (int i = 1, numOfFound = 0; i < limit && numOfFound < maxNumOfNumbers; i++)
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
                p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
            }
            else if (nn >= 6)
            {
                p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
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
