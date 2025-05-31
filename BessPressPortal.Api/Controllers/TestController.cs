namespace BessPressPortal.Api.Controllers
{
    using BessPressPortal.Api.Data;
    using BessPressPortal.Shared.Models;
    using Microsoft.AspNetCore.Mvc;


    [ApiController] // Indicates that this class is an API controller
    [Route("[controller]")] // Defines the base route for this controller (e.g., /weather)
    public class TestController : ControllerBase // Inherit from ControllerBase for API controllers
    {
        private readonly ITestRepository _testRepository; // Inject the repository
        public TestController(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }
        // Define the HTTP GET endpoint
        [HttpGet] // This method will respond to GET requests to /test
        public async Task<IEnumerable<TestDto>> Get()
        {
            return await _testRepository.GetAllAsync();
        }
    } // End of TestController class

}
