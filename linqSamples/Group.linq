<Query Kind="Program" />

void Main()
{
	var rand = new Random();
	var People = new List<Person> {
		new Person { Name = "ram", Age =  30, Salary= rand.Next(1,1000)},
		new Person { Name = "ram1", Age =  31, Salary= rand.Next(1,1000)},
		new Person { Name = "ram2", Age =  32, Salary= rand.Next(1,1000)},
		new Person { Name = "ram3", Age =  33, Salary= rand.Next(1,1000)},
		new Person { Name = "ram4", Age =  34, Salary= rand.Next(1,1000)},
		new Person { Name = "ram_1", Age =  30, Salary= rand.Next(1,1000)},
		new Person { Name = "ram1_1", Age =  31, Salary= rand.Next(1,1000)},
		new Person { Name = "ram2_1", Age =  32, Salary= rand.Next(1,1000)},
		new Person { Name = "ram3_1", Age =  33, Salary= rand.Next(1,1000)},
		new Person { Name = "ram_2", Age =  30, Salary= rand.Next(1,1000)},
		new Person { Name = "ram1_2", Age =  31, Salary= rand.Next(1,1000)},
		new Person { Name = "ram2_2", Age =  32, Salary= rand.Next(1,1000)},
		new Person { Name = "ram3_2", Age =  33, Salary= rand.Next(1,1000)}
	};
	
	///Console.WriteLine(People.GroupBy( p => p.Age, p=>p.Name)); // get all names in same age
	//Console.WriteLine(People.GroupBy( p => p.Age <33).Select( g => new { LessThan33 = g.Key , count = g.Count() })); // count of members  less thant and greater than 33 age
	Console.WriteLine(People.GroupBy( p => p.Age <33).Select( g => new { LessThan33 = g.Key , HighestSal =  g.OrderByDescending( g1 => g1.Salary).First() })); // get person with highest salary in ages below 33 and in above 33

}


	
public class Person
{
	public string Name {get;set;}
	public int Age {get;set;}
	public int Salary {get;set;}
}