namespace EfCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (ImpactDbContext impact = new ImpactDbContext())
            {
                Console.WriteLine(impact.Roles.Find(1).RoleName); 

            } 
        }
    }
}