<Query Kind="Program" />

void Main()
{
	var rand = new Random();
	var People = new List<Person> {
		new Person { Name = "ram", Age =  30, DeptId= rand.Next(1,4), Salary= 1000},
		new Person { Name = "ram", Age =  30, DeptId= rand.Next(1,4), Salary= 1000},
		new Person { Name = "ram1", Age =  31, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram2", Age =  32, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram3", Age =  33, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram4", Age =  34, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram_1", Age =  30, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram1_1", Age =  31, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram2_1", Age =  32, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram3_1", Age =  33, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram_2", Age =  30, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram1_2", Age =  31, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram2_2", Age =  32, DeptId= rand.Next(1,4), Salary= rand.Next(1,1000)},
		new Person { Name = "ram3_2", Age =  33, DeptId= rand.Next(1,4)},
		new Person { Name = "ram_5", Age =  30, DeptId= 5, Salary= rand.Next(1,1000)},
		new Person { Name = "ram1_5", Age =  31, DeptId= 5, Salary= rand.Next(1,1000)},
		new Person { Name = "ram2_5", Age =  32, DeptId= 5, Salary= rand.Next(1,1000)},
		new Person { Name = "ram3_5", Age =  33, DeptId= 5}
	};
	
	var Depts = new List<Dept> {
		new Dept { Name = "dept1", DeptId= 1},
		new Dept { Name = "dept2", DeptId= 2},
		new Dept { Name = "dept3", DeptId= 3},
		new Dept { Name = "dept4", DeptId= 4}
	};
	
	// get all employees by department (inner join)
	//Console.WriteLine(People.Join(Depts, p=>p.DeptId, d => d.DeptId, (p,d) => new { EmplName = p.Name, EmplDept = p.DeptId, DeptName = d.Name}).GroupBy( empdept => empdept.DeptName ));
	var empDept = (from emp in People
				join depart in Depts on  emp.DeptId  equals depart.DeptId
				select new {
					EmplName  = emp.Name,
					DeptName = depart.Name
				}).ToList();
				
	// Console.WriteLine(empDept);				
				

	// get employee with highest salary in each department
	
	/*
	Console.WriteLine(
		People.Join(Depts, p=>p.DeptId, d => d.DeptId, (p,d) => new { EmplName = p.Name, EmpSal = p.Salary, DeptName = d.Name})
		.GroupBy( empdept => empdept.DeptName )
		.Select(ed => new {DeptName = ed.Key , HighestEmpl = ed.OrderByDescending( g1 => g1.EmpSal).First()})
		);
		*/
	
	
	//left/right outer join between employees and depts
	/*
	Console.WriteLine(
		People.GroupJoin(Depts, p=>p.DeptId, d => d.DeptId, (person,depts) => new { Empl = person, Dept = depts.Select( d1 => d1.Name) })
		);
	*/	
	
		/*
	Console.WriteLine(
		Depts.GroupJoin(People, p=>p.DeptId, d => d.DeptId, 
			(d1,p1) => new { DeptName = d1.Name, Peopls = p1.Select( p12 => p12.Name).ToArray() })
		);	// Get all people in a dept
		*/
	var empIncludingNoDept = (from emp in People
				join depart in Depts on  emp.DeptId  equals depart.DeptId
				into empDepts
				from empdept  in empDepts.DefaultIfEmpty()
				select new {
					EmplName  = emp.Name,
					AssignedDept = empdept == null ? "No Department" : empdept.Name
				}).ToList();
	Console.WriteLine(empIncludingNoDept);
	
	/*
	//cross join
	Console.WriteLine(
		People.SelectMany(p1=>Depts,  (p,d) => new { Empl = p, Dept = d })
		);
		*/
}


	
public class Person
{
	public string Name {get;set;}
	public int Age {get;set;}
	public int DeptId {get;set;}
	public int Salary {get;set;}
}


public class Dept
{
	public string Name {get;set;}
	public int DeptId {get;set;}
}