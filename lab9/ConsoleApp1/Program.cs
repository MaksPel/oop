

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("\nСумма положительных чисел: " + File.ReadAllLines(@"../../../numbers.txt").SelectMany(line => line.Split(" ")).Select(word => double.Parse(word)).Where(x => x > 0).Sum());
    }
}