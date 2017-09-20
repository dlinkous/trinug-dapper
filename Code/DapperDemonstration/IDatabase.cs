using System;
using System.Collections.Generic;
using DapperDemonstration.Entities;

namespace DapperDemonstration
{
	internal interface IDatabase
	{
		IEnumerable<CustomerFluff> GetActiveCustomers();
		IEnumerable<CustomerFluff> SearchCustomersByText(string text);
		CustomerFlagQuantities GetCustomerFlagQuantities();
		void CreateCustomer(string nameLast, string nameFirst);
		void MarkCustomerRetired(int customerId);
		void SetCustomerComments(int customerId, string comments);
		void DeleteCustomer(int customerId);
		void DeleteAllCustomers();
	}
}
