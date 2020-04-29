<Query Kind="Program" />

void Main()
{
	var people = new Person[]{
		new Person("Jane", "jane@foo.com"),
		new Person("John", "john@foo.com"),
		new Person("Chris", string.Empty)
	};
	
	var records = new Record[]{
		new Record("jane@foo.com", "JaneAtFoo"),
		new Record("jane@foo.com", "JaneAtHome"),
		new Record("john@foo.com", "Jane1980"),
	};
	
	//Join
	var query = people.Join(records, 
		x => x.Email, // join field
		y => y.Mail, // join field
		(person, record) => new { Name = person.Name, SkypeId = record.SkypeId});
		Console.WriteLine(query);

	//GroupJoin
	var query1 = people.GroupJoin(records, 
		x => x.Email, // join field
		y => y.Mail, // join field
		(person, recs) => new { Name = person.Name, SkypeId = recs.Select( r => r.SkypeId).ToArray()});
		Console.WriteLine(query1);
		
		
		
}

// Define other methods and classes here

public class Person
{
	public string Name, Email;
	
	public Person(string name, string email){
		Name = name;
		Email = email;
	}
}

public class Record
{
	public string Mail, SkypeId;
	
	public Record(string mail, string skypeId){
		Mail = mail;
		SkypeId = skypeId;
	}
}