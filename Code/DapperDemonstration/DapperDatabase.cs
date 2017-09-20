using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DapperDemonstration.Entities;

namespace DapperDemonstration
{
	internal class DapperDatabase : IDatabase
	{
		private readonly string connectionString = @"Server=POSEIDON\EXPRESS1;Database=DapperDemonstration;Trusted_Connection=true;";

		public void CreateCustomer(string nameLast, string nameFirst)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"INSERT INTO Customer VALUES (@NameLast, @NameFirst, 0, GETUTCDATE(), 0, 0, '')";
				con.Execute(sql, new { NameLast = nameLast, NameFirst = nameFirst });
			}
		}

		public void DeleteCustomer(int customerId)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"DELETE Customer WHERE CustomerId = @Id";
				con.Execute(sql, new { Id = customerId });
			}
		}

		public IEnumerable<CustomerFluff> GetActiveCustomers()
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"SELECT * FROM Customer WHERE IsRetired = 0";
				return con.Query<CustomerFluff>(sql);
			}
		}

		public CustomerFlagQuantities GetCustomerFlagQuantities()
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"SELECT SUM(CAST(IsRetired AS INT)) AS [QuantityRetired], SUM(CAST(IsLocked AS INT)) AS [QuantityLocked] FROM Customer";
				return con.Query<CustomerFlagQuantities>(sql).Single();
			}
		}

		public void MarkCustomerRetired(int customerId)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"UPDATE Customer SET IsRetired = 1 WHERE CustomerId = @Id";
				con.Execute(sql, new { Id = customerId });
			}
		}

		public IEnumerable<CustomerFluff> SearchCustomersByText(string text)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"SELECT * FROM Customer WHERE NameLast LIKE '%' + @Text + '%' OR NameFirst LIKE '%' + @Text + '%' OR Comments LIKE '%' + @Text + '%'";
				return con.Query<CustomerFluff>(sql, new { Text = text });
			}
		}

		public void SetCustomerComments(int customerId, string comments)
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"UPDATE Customer SET Comments = @Comments WHERE CustomerId = @Id";
				con.Execute(sql, new { Id = customerId, Comments = comments });
			}
		}

		public void DeleteAllCustomers()
		{
			using (var con = new SqlConnection(connectionString))
			{
				con.Open();
				const string sql = @"DELETE Customer";
				con.Execute(sql);
			}
		}
	}
}
