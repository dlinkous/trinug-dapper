using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DapperDemonstration.Entities;

namespace DapperDemonstration
{
	internal class ClassicDatabase : IDatabase
	{
		private readonly string connectionString = @"Server=POSEIDON\EXPRESS1;Database=DapperDemonstration;Trusted_Connection=true;";

		public void CreateCustomer(string nameLast, string nameFirst)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"INSERT INTO Customer VALUES (@NameLast, @NameFirst, 0, GETUTCDATE(), 0, 0, '')";
				using (var com = new SqlCommand(sql, con))
				{
					com.Parameters.AddWithValue("NameLast", nameLast);
					com.Parameters.AddWithValue("NameFirst", nameFirst);
					com.ExecuteNonQuery();
				}
			}
		}

		public void DeleteCustomer(int customerId)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"DELETE Customer WHERE CustomerId = @Id";
				using (var com = new SqlCommand(sql, con))
				{
					com.Parameters.AddWithValue("Id", customerId);
					com.ExecuteNonQuery();
				}
			}
		}

		public IEnumerable<CustomerFluff> GetActiveCustomers()
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"SELECT * FROM Customer WHERE IsRetired = 0";
				using (var com = new SqlCommand(sql, con))
				{
					using (var rdr = com.ExecuteReader())
					{
						while (rdr.Read())
						{
							var customerFluff = new CustomerFluff()
							{
								CustomerId = rdr.GetInt32(0),
								NameLast = rdr.GetString(1),
								NameFirst = rdr.GetString(2),
								Comments = rdr.GetString(7)
							};
							yield return customerFluff;
						}
					}
				}
			}
		}

		public CustomerFlagQuantities GetCustomerFlagQuantities()
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"SELECT SUM(CAST(IsRetired AS INT)) AS [QuantityRetired], SUM(CAST(IsLocked AS INT)) AS [QuantityLocked] FROM Customer";
				using (var com = new SqlCommand(sql, con))
				{
					using (var rdr = com.ExecuteReader())
					{
						rdr.Read();
						return new CustomerFlagQuantities()
						{
							QuantityRetired = rdr.GetInt32(0),
							QuantityLocked = rdr.GetInt32(1)
						};
					}
				}
			}
		}

		public void MarkCustomerRetired(int customerId)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"UPDATE Customer SET IsRetired = 1 WHERE CustomerId = @Id";
				using (var com = new SqlCommand(sql, con))
				{
					com.Parameters.AddWithValue("Id", customerId);
					com.ExecuteNonQuery();
				}
			}
		}

		public IEnumerable<CustomerFluff> SearchCustomersByText(string text)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"SELECT * FROM Customer WHERE NameLast LIKE '%' + @Text + '%' OR NameFirst LIKE '%' + @Text + '%' OR Comments LIKE '%' + @Text + '%'";
				using (var com = new SqlCommand(sql, con))
				{
					com.Parameters.AddWithValue("Text", text);
					using (var rdr = com.ExecuteReader())
					{
						while (rdr.Read())
						{
							var customerFluff = new CustomerFluff()
							{
								CustomerId = rdr.GetInt32(0),
								NameLast = rdr.GetString(1),
								NameFirst = rdr.GetString(2),
								Comments = rdr.GetString(7)
							};
							yield return customerFluff;
						}
					}
				}
			}
		}

		public void SetCustomerComments(int customerId, string comments)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"UPDATE Customer SET Comments = @Comments WHERE CustomerId = @Id";
				using (var com = new SqlCommand(sql, con))
				{
					com.Parameters.AddWithValue("Id", customerId);
					com.Parameters.AddWithValue("Comments", comments);
					com.ExecuteNonQuery();
				}
			}
		}

		public void DeleteAllCustomers()
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"DELETE Customer";
				using (var com = new SqlCommand(sql, con))
				{
					com.ExecuteNonQuery();
				}
			}
		}
	}
}
