using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;

namespace ConsoleAppTest
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Repository repository = new Repository();
			UserService userService = new UserService(repository);

			var list = userService.GetUsers();

			foreach (var item in list)
			{
				Console.WriteLine($"{item.Id} {item.FirstName}  {item.LastName}  {item.Email}");
			}


		}
	}
}
