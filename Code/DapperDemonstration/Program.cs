using System;
using System.Diagnostics;
using System.Linq;

namespace DapperDemonstration
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var classic = new ClassicDatabase();
			var dapper = new DapperDatabase();
			for (var i = 0; i < 7; i++)
			{
				Console.WriteLine($"Batch {i + 1}:");
				ExecuteDatabase(classic);
				ExecuteDatabase(dapper);
			}
			Console.WriteLine("Finished!");
			Console.ReadKey();
		}

		private static void ExecuteDatabase(IDatabase database)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			database.DeleteAllCustomers();
			for (var i = 0; i < 300; i++)
			{
				database.CreateCustomer($"Name {i}", $"Name {i}");
			}
			var active = database.GetActiveCustomers().ToList();
			foreach (var customer in active)
			{
				if (customer.CustomerId % 2 == 0)
				{
					database.MarkCustomerRetired(customer.CustomerId);
				}
				if (customer.CustomerId % 3 == 0)
				{
					database.SetCustomerComments(customer.CustomerId, "COMMENTS");
				}
			}
			var quantityWithComments = database.SearchCustomersByText("COMMENTS").Count();
			DoStuff(quantityWithComments);
			var flagQuantities = database.GetCustomerFlagQuantities();
			DoStuff(flagQuantities.QuantityRetired);
			database.DeleteCustomer(active.First().CustomerId);
			stopwatch.Stop();
			Console.WriteLine($"Time ({database.GetType().Name}): {stopwatch.ElapsedMilliseconds}");
		}

		private static void DoStuff(int value)
		{
			// Empty
		}
	}
}
