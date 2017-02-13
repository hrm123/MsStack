<Query Kind="Program" />

void Main()
{
	var People = new List<Person> {
		new Person { Name = "ram", Age =  30},
		new Person { Name = "ram1", Age =  31},
		new Person { Name = "ram2", Age =  32},
		new Person { Name = "ram3", Age =  33},
		new Person { Name = "ram4", Age =  34} 
	}
	
	Console.WriteLine(People.OrderBy( p => p.Age));

}


	
public class Person
{
	public string Name {get;set;}
	public int Age {get;set;}
}