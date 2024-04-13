using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;

namespace CameraDiplomat.Services
{
	public class SessionCustomDesigner : ISessionCustomDesigner
	{
		private readonly ConfigurationService _configurationService;
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
			session.creatorsLogin = _configurationService.activeUser.login;

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
