using CameraDiplomat.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CameraDiplomat.Entities;

namespace CameraDiplomat.Services
{
	public class SessionCustomDesigner
	{
		private ConfigurationService _configurationService;
		public SessionCustomDesigner(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}
		public Session DesigneNewEntity(DateTime startData, int totalCount, int marrageCount)
		{
			Session session = new Session();
			session.Id = Guid.NewGuid().ToString();
			session.Data = startData.ToString() + "/" + DateTime.Now.ToString();
			session.TotalCount = totalCount;
			session.MarriageCount = marrageCount;
			double MarriagePercent = (double)marrageCount / totalCount * 100;
			session.MarriagePercent = MarriagePercent.ToString("F2") + "%";
			if(_configurationService.activeUser!=null)
			{
				session.creatorsLogin = _configurationService.activeUser.login;
			}
			else
			{
				session.creatorsLogin = "*userLogOut*";
			}
			return session;
		}
		public Session DesigneNewEntity(string id, DateTime startData, int totalCount, int marrageCount)
		{
			Session session = new Session();
			session.Id = id;
			session.Data = startData.ToString() + "/" + DateTime.Now.ToString();
			session.TotalCount = totalCount;
			session.MarriageCount = marrageCount;
			double MarriagePercent =  (double)marrageCount/totalCount * 100;
			session.MarriagePercent = MarriagePercent.ToString("F2") + "%";
			if (_configurationService.activeUser != null)
			{
				session.creatorsLogin = _configurationService.activeUser.login;
			}
			else
			{
				session.creatorsLogin = "*userLogOut*";
			}
			return session;
		}


	}
}
