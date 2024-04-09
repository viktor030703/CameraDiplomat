namespace CameraDiplomat.Interfaces
{
	public interface IDbMainController
	{
		void DeleteDb();
		void CreateDb();
		bool CheckDbExist();
	}
}
