namespace Barker.Stewart.Bpdts.Test.LocationApi.Bpdts
{
    using System.Threading.Tasks;

    public interface IBpdtsClient
    {
        Task<string> GetUsersInCity(string city);

        Task<string> GetAllUsers();
    }
}
