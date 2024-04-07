using CameraDiplomat.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Services
{
	public class DbMainController
	{
		private ApplicationContext db = new ApplicationContext();
		public void DeleteDb()
		{
			db.Database.EnsureDeleted();
			db.SaveChanges();
		}
		public void CreateDb()
		{
			db.Database.EnsureCreated();
			db.SaveChanges();
		}

		public bool CheckDbExist()
		{
			bool response = db.Database.CanConnect();
			return response;
		}

	}
}
