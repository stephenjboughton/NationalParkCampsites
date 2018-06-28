using System;
using System.Transactions;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace Capstone.Tests
{
	[TestClass]
	public class CampgroundDBTests
	{
		public const string ConnectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Campground;Integrated Security=True";

		private TransactionScope transaction;


		[TestInitialize]
		public void SetupData()
		{
			transaction = new TransactionScope();

			string sql = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "database.sql"));

			using (SqlConnection conn = new SqlConnection(ConnectionString))
			{
				conn.Open();

				SqlCommand cmd = new SqlCommand(sql, conn);

				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Rollback Transaction
		/// </summary>
		[TestCleanup]
		public void CleanUpData()
		{
			transaction.Dispose();
		}
	}
}
