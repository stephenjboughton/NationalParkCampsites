using System;
using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
	[TestClass]
	public class ParkDALTests : CampgroundDBTests
	{

		//Tests Get All Parks
		[TestMethod]
		public void GetAllParks_Test()
		{
			//Arrange
			ParkDAL dal = new ParkDAL(ConnectionString);

			//Act
			var parks = dal.GetAllParks();

			//Assert
			Assert.AreEqual(1, parks.Count);
		}
	}
}
