using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    class PrimeNumbers
    {

        public static void TestCase_GetPrimesInRange()
        {
            PrimeNumbers pn = new PrimeNumbers();

            int[] primes = pn.GetPrimeNumbers(0, 100);
            foreach (var item in primes)
            {
                Console.Write(item + ",");
            }
        }

        public static void TestCase_GetPrimeDivisors()
        {
            PrimeNumbers pn = new PrimeNumbers();
            var answer = pn.GetPrimeDivisors(1212);
            foreach (var item in answer)
            {
                Console.Write(item + ",");
            }
        }

        List<Tuple<int, int>> GetPrimeDivisors(int number)
        {
            int[] primes = GetPrimeNumbers(0, number);
            int[] powers = new int[primes.Length];
            List<Tuple<int, int>> primeNPower = new List<Tuple<int,int>>();
            for (int y = 0; y < primes.Length; y++)
            {
                if (number <= 2)
                {
                    break;
                }
                int currentPrime = primes[y];
                bool isDivisible = true;
                int ctr = 0;
                do
                {
                    isDivisible = number % currentPrime == 0;
                    if (isDivisible)
                    {
                        number = number / currentPrime;
                        ctr++;
                    }

                } while (isDivisible);
                if (ctr>0)
                {
                    primeNPower.Add(new Tuple<int, int>(currentPrime, ctr));
                }
            }
            return primeNPower;
        }

        /*
        /// <summary>
        /// Number can be written as power(p1,y1)*power(p2,y2)...power(pk,yk)
        /// where p,p2..,pk are prime numbers and y1,y2,..yk are their powers. Ex 100 = power(2,2)*power(5,2)
        /// Total number of divisors will be (y1+1)*(y2+1)...(yk+1) (since divisor also includes power of 0)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        int[] GetDivisors(int number)
        {
            
            int numDivisors = 1;
            foreach (var item in powers)
            {
                numDivisors *= (item + 1);
            }
            int[] divisors = new int[numDivisors];


        }
        */

        // get all prime numbers in given range
        int[] GetPrimeNumbers(int start, int end)
        {
            // use Eratosthenes Sieve methodology - start with lowest prime and mark all numbers in given
            // range that are multiples of this as visited. Go to next lowest not visited number (which will be prime number 
            // since by definition prime numbers are those that dont have anything lesser than them divide them exactly (without leaving reminder)
            bool[] arr = Enumerable.Range(0, end+1).Select(i => false).ToArray<bool>();
            int x = 2;
            while (x <= end)
            {
                if (arr[x] != false)
                {
                    throw new ApplicationException("something wrong");
                }
                int z = 2;
                for (int y = x*z; y <= end; y = x * z) // set all multiples of current prime number to true ('visited')
                {
                    z++;
                    arr[y] = true;
                }

                int j = x+1;
                if (j > end)
                {
                    break;
                }
                //get next number in array that is not yet visited
                while (j <= end && arr[j]) { j++;  }

                if (j > end)
                {
                    break;
                }

                x = j;
            }

            List<int> primes = new List<int>();

            //get all the prime numbers (not visited) from array
            for(x=(start==0 ? 2 : start) ; x <= end; x++)
            {
                if (arr[x] == false)
                {
                    primes.Add(x);
                }
            }

            return primes.ToArray();
        }
    }
}
