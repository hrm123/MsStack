<Query Kind="Statements" />

var l = new List<string>{"hgt","iu","iuy"};
// Console.WriteLine(l);

// Console.WriteLine(Enumerable.Range('a','z'-'a'));


var nums = Enumerable.Range(1,4);
var squares = nums.Select(x => x*x);
// Console.WriteLine(squares);

string sentence = "this is a nice sentence";
var wordLengths = sentence.Split().Select(w => w.Length);
var wordLengths1 = sentence.Split().Select(w => new { Word = w, Size = w.Length });
// Console.WriteLine(wordLengths1);

var sequences = new [] { "red,green,blue","orange","white,pink"};
var allWords = sequences.SelectMany(s => s.Split(',')); // SelectMany flattens a collection
// Console.WriteLine(allWords);

//cross product of colelctions
string[] objects =  { "house", "car", "bicycle"};
string[] colors = { "red", "green", "gray"};
var pairs = colors.SelectMany( _ => objects, (c, o) => $"{c} {o}");
Console.WriteLine(pairs);
