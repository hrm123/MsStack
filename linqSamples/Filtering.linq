<Query Kind="Program" />

void Main()
{
	
	
	Console.WriteLine(Exercise.MyFilter( Enumerable.Range(1, 10)));
	Console.WriteLine(Exercise.MyFilter1( Enumerable.Range(1, 10)));
	
}


	
public class Exercise
{
	private static Func<int,bool> myfilter = (val) =>
	{
		return val%2 == 0;
	};
	
	public static IEnumerable<int> MyFilter(IEnumerable<int> input)
	{
		return input.Where( i => { return i%2 == 0; }).Select( i1 => i1*i1).Where(  i2 => i2 <=50);
	}
	
	public static IEnumerable<int> MyFilter1(IEnumerable<int> input)
	{
		return input.Where( i => { return myfilter(i); }).Select( i1 => i1*i1).Where(  i2 => i2 <=50);
	}
	
	
}
