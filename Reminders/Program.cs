

if(args.Length > 0)
{
    if (args[0] == "LA")
    {
        Console.WriteLine("Look away from monitor outside");
    }
    else
    {
        //default
        Console.WriteLine("Stand and Stretch slowly");
    }

}
else
{
    //default
    Console.WriteLine("Stand and Stretch slowly");
}
Console.ReadKey();
