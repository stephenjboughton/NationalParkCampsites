using System;
using System.Transactions;
using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
	[TestClass]
	public class SiteDALTests : CampgroundDBTests
	{
		//Tests Get Available Sites for a Campground
		[DataTestMethod]
		[DataRow(1, "06/08/2018", "06/14/2018", 2)]
		[DataRow(1, "06/06/2018", "06/14/2018", 1)]
		[DataRow(1, "06/18/2018", "06/25/2018", 0)]
		[DataRow(1, "06/06/2018", "06/18/2018", 0)]
		[DataRow(1, "05/25/2018", "06/02/2018", 0)]
		public void GetAvailableSites_Test(int campgroundId, string fromDate, string toDate, int expectedCount)
		{
			//Arrange
			SiteDAL dal = new SiteDAL(ConnectionString);
			
			//Act
			var sites = dal.GetAvailableSites(campgroundId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));

			//Assert
			Assert.AreEqual(expectedCount, sites.Count);
		}

		//Tests Get Available Sites for a Campground
		[DataTestMethod]
		[DataRow(1, "06/08/2018", "06/14/2018", 2)]
		[DataRow(1, "06/06/2018", "06/14/2018", 1)]
		[DataRow(1, "06/18/2018", "06/25/2018", 0)]
		[DataRow(1, "06/06/2018", "06/18/2018", 0)]
		[DataRow(1, "05/25/2018", "06/02/2018", 0)]
		public void GetAvailableSitesParkwide_Test(int parkId, string fromDate, string toDate, int expectedCount)
		{
			//Arrange
			SiteDAL dal = new SiteDAL(ConnectionString);

			//Act
			var sites = dal.GetSitesParkwide(parkId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));

			//Assert
			Assert.AreEqual(expectedCount, sites.Count);
		}
	}
}
