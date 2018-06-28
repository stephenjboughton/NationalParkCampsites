using System;
using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
	[TestClass]
	public class CampgroundDALTests : CampgroundDBTests
	{
		//Tests Get All Campgrounds Per PArk
		[TestMethod]
		public void GetAllCampgroundsPerPark_Test()
		{
			//Arrange
			CampgroundDAL dal = new CampgroundDAL(ConnectionString);
			int parkId = 1;

			//Act
			var campgrounds = dal.GetAllCampgroundsPerPark(parkId);

			//Assert
			Assert.AreEqual(1, campgrounds.Count);
		}
	}
}
