using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreDemos
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ManualResetEvent evenMre = new ManualResetEvent(false);
            ManualResetEvent oddMre = new ManualResetEvent(false);
            int ctrEven = 0;
            int ctrOdd = 1;
            Object blockLock = new object();


            Console.WriteLine("Program prints with 2 threads each prints even and odd numbers one after other !");
            Task evenThread = new Task(() => {
                while (true)
                {
                    lock (blockLock)
                    {
                        Console.Write(ctrEven + ",");
                        ctrEven = ctrEven + 2;
                        Console.Out.Flush();
                        evenMre.Set();
                        evenMre.Reset();
                    }
                    
                    oddMre.WaitOne();
                }
            });

            Task oddThread = new Task(() => {
                evenMre.WaitOne(); //wait until t1 is finished
                while (true)
                {
                    // b.SignalAndWait();
                    lock (blockLock)
                    {
                        Console.Write(ctrOdd + ",");
                        ctrOdd = ctrOdd + 2;
                        Console.Out.Flush();
                        oddMre.Set();
                        oddMre.Reset();

                    }
                    
                    evenMre.WaitOne(); //wait until t1 is finished
                }
            });

            oddThread.Start();
            evenThread.Start();

            Console.Read();
            evenMre.Reset();
            oddMre.Reset();
            Console.WriteLine("Click to exit");
        }
    }

    public class ExThread
    {

        // Static method for thread a 
        public static void printEven()
        {
            int ctr = 0;
            Console.WriteLine(ctr + ",");
            ctr = ctr + 2;
        }

        // static method for thread b 
        public static void printOdd()
        {
            int ctr = 1;
            Console.WriteLine(ctr + ",");
            ctr = ctr + 2;
        }
    }
}
