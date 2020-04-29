<Query Kind="Statements" />



string word1 = "helloooo";
string word2 = "help";

//set operations in Linq
Console.WriteLine(word1.Distinct());
Console.WriteLine(word1.Intersect(word2));
Console.WriteLine(word1.Union(word2));
Console.WriteLine(word1.Except(word2));

//quantifier operations - whether one or more elemnetns in array satisfy given predicate

int [] nums = { 1,2,3,4,5};
Console.WriteLine("Are all nums > 0? " + nums.All(n => n > 0));
Console.WriteLine("Is any nums < 0? " + nums.Any(n => n < 0));

//skip and take
int [] numbs = { 1,2,3,4,5,6,7,8,9};
Console.WriteLine(numbs.Skip(2).Take(6));
Console.WriteLine(numbs.SkipWhile(i => i<3).TakeWhile(j => j < 6));


