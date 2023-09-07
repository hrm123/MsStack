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

        public static void TestCase()
        {
            PrimeNumbers pn = new PrimeNumbers();

            int[] primes = pn.GetPrimeNumbers(0, 100);
            foreach (var item in primes)
            {
                Console.Write(item + ",");
            }
        }

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
            for(x=start; x <= end; x++)
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
