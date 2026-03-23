using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Security;
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

			try
			{
				string voucherNumber = VoucherNumberHelper.GenerateNumber();
				Voucher voucher = new Voucher() { Number = voucherNumber, InitialAmount = 50};
				Console.WriteLine(voucher);
            }
			catch (ArgumentException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
