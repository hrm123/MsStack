<Query Kind="Program" />

void Main()
{
/*
	var rand = new Random();
	var randomValues = Enumerable.Range(1,10).Select(_ => rand.Next(10)-5);
	Console.WriteLine(randomValues);
	var csvString = new Func<IEnumerable<int>, string>(values => { return string.Join(",", values.Select( v => v.ToString()).ToArray()); });
	Console.WriteLine(csvString(randomValues));
	Console.WriteLine(csvString(randomValues.OrderBy(x => x)));
	*/
	
	var pps = new List<Person>{
		new Person { Name="Adam", Age = 20},
		new Person { Name="Adam", Age = 20},
		new Person { Name="Boris", Age = 18},
		new Person { Name="Claire", Age = 36},
		new Person { Name="Adam", Age = 20},
		new Person { Name="Jack", Age = 20}
	};
	
	IEnumerable<IGrouping<string, Person>> byName = pps.GroupBy( p => p.Name);
	Console.WriteLine(byName);
	IEnumerable<IGrouping<Boolean, Person>> byAge = pps.GroupBy( p => p.Age <30);
	Console.WriteLine(byAge);
	
	var byAgeNames = pps.GroupBy( p => p.Age, p => p.Name); // In grouping only keep name
	Console.WriteLine(byAgeNames);
	
	
	
	
}

// Define other methods and classes here

class Person
{
public string Name;
public int Age;
}
