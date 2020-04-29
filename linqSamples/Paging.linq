<Query Kind="Program" />

void Main()
{
	int pageNum = 10;
	int pageSize = 30;
	
	var csvString = new Func<IEnumerable<int>, string>(values => { return string.Join(",", values.Select( v => v.ToString()).ToArray()); });
	var pagedRecords = Enumerable.Range(0,10000).Skip(pageNum * pageSize).Take(pageSize);
	Console.WriteLine(csvString(pagedRecords));
	
	Random rand = new Random();
	var randName = new Func<int, string>( i =>  { 
		char[] alphs =  {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
		var map = new Dictionary<int ,string>();
		string resp;
		  if (map.TryGetValue(i, out resp))
		  {
	        return resp;
			} else{
				resp = alphs[rand.Next(26)] + i.ToString() ;
				map.Add(i, resp);
				return resp;
			}
		});
	
	var emps = Enumerable.Range(0,100).Select(_ => new Employee{
		EmplId = rand.Next(100),
		EmplName = randName(rand.Next(100))
	}).ToList();
	emps.AddRange(Enumerable.Range(0,100).Select(_ => new Employee{
		EmplId = rand.Next(100),
		EmplName = randName(rand.Next(100))
	}).ToList());
	pageNum = 1;
	pageSize = 100;
	
	string sortField = "Name";
	var pagedEmpls = emps.OrderBy(e => e.EmplName).ThenBy(e=>e.EmplId).Skip((pageNum-1) * pageSize).Take(pageSize);
	var pagedEmpls1 = ( from empl in emps
		orderby empl.EmplName
		orderby empl.EmplId
		select empl).Skip((pageNum-1) * pageSize).Take(pageSize);
		
	Console.WriteLine(pagedEmpls1);
}




// Define other methods and classes here

public class Employee{
	public String EmplName;
	public int EmplId;
}
