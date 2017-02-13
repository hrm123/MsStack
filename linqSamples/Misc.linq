<Query Kind="Program" />

void Main()
{
	/*
	string word1 = "helooooo";
	string word2 = "help";
	Console.WriteLine(word1.Distinct());
	Console.WriteLine(word1.Intersect(word2)); // things common to both collecctions
	Console.WriteLine(word1.Union(word2)); // things unique from both collections
	Console.WriteLine(word1.Except(word2)); // things not present in other collection
	*/
	
	// Quantifiers
	int[] numbers = {4,1,2,3,4,5,3,3};
	/*
	Console.WriteLine("Are all numbers >0 ? " + numbers.All( x => x >0));
	Console.WriteLine("Are all numbers Odd ? " + numbers.All( x => x %2 == 1)); 
	Console.WriteLine("Are any  numbers Odd ? " + numbers.Any( x => x %2 == 1)); 
	Console.WriteLine("Are any  numbers equal 5 ? " + numbers.Contains(5)); 
	Console.WriteLine( numbers.Skip(2).Take(60)); 
	*/
	Console.WriteLine( numbers.SkipWhile( i => i ==4));
	Console.WriteLine( numbers.TakeWhile( i => i >3));
	Console.WriteLine(numbers.SkipWhile(i=>i<0).TakeWhile(i1=> i1>0).Count()); // finds the first contiguous subsequence of positive numbers and tells you how many numbers that subsequence it contains
	
}