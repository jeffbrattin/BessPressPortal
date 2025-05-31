namespace BessPressPortal.Api.Data
{

    using BessPressPortal.Shared.Models;
    public interface ITestRepository
    {

        Task<IEnumerable<TestDto>> GetAllAsync();


    }
}
