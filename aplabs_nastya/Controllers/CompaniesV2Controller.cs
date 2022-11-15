using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aplabs_nastya.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/{v:apiversion}/companies")]
    [ApiController]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        public CompaniesV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS");
            return Ok();
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await
           _repository.Company.GetAllCompaniesAsync(trackChanges:
            false);
            return Ok(companies);
        }
    }
}
