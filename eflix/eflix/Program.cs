using System;
using Akka.Actor;

namespace eflix
{
    
    class Program
    {
        private static ActorSystem _eflixActorSystem;

        static void Main(string[] args)
        {
            _eflixActorSystem = ActorSystem.Create("eflix");

            Console.ReadLine();

            
        }
    }
}
