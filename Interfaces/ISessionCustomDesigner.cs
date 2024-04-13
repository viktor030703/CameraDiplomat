using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface ISessionCustomDesigner
	{
		Session DesigneNewEntity(DateTime startData, int totalCount, int marrageCount);
		Session DesigneNewEntity(string id, DateTime startData, int totalCount, int marrageCount);
	}
}
