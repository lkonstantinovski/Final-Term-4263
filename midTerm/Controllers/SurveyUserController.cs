using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using midTerm.Models.Models.SurveyUser;
using midTerm.Services.Abstractions;

namespace midTerm.Controllers
{
    /// <summary>
    /// SurveyUser API Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyUserController
        : ControllerBase
    {
        private readonly ISurveyUserService _service;
        /// <summary>
        ///  SurveyUser Consturctor API Controller
        /// </summary>
        /// <param name="service">SurveyUser Service</param>
        public SurveyUserController(ISurveyUserService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get Item 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/SurveyUser/
        ///     
        /// </remarks>
        /// <returns>An Base model item</returns>
        /// <response code="200">All went well</response>
        /// <response code="204">Item had no content</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.Get();
            return Ok(result);
        }
        /// <summary>
        /// Get Item by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/SurveyUser/{id}
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <returns>An Extended model item</returns>
        /// <response code="200">All went well</response>
        /// <response code="204">Item had no content</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="500">Server side error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }
        /// <summary>
        /// Create an Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Match
        ///     {
        ///         "FirstName":"string",
        ///         "LastName":"string",
        ///         "DoB":"2021-05-11T13:48:49.380Z",
        ///         "Gender": gender,
        ///         "Country":"string"
        ///     }
        ///     
        /// </remarks>
        /// <param name="model">model to create</param>
        /// <returns>identifier of the created item</returns>
        /// <response code="201">Path of the created item</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="405">Method not allowed</response>
        /// <response code="409">The item was not created</response>
        /// <response code="500">Server side error</response>
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] SurveyUserCreate model)
        {
            if (ModelState.IsValid)
            {
                var user = await _service.Insert(model);
                return user != null
                    ? (IActionResult)CreatedAtRoute(nameof(GetById), user, user.Id)
                    : Conflict();
            }
            return BadRequest();
        }
        /// <summary>
        /// Update an Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/SurveyUser
        ///     {
        ///         "FirstName":"string",
        ///         "LastName":"string",
        ///         "DoB":"2021-05-11T13:48:49.380Z",
        ///         "Gender": bool,
        ///         "Country":"string"
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <param name="model">model to create</param>
        /// <returns>SurveyUser base model</returns>
        /// <response code="200">All went well</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="405">Method not allowed</response>
        /// <response code="409">The item was not created</response>
        /// <response code="500">Server side error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SurveyUserUpdate model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;
                var result = await _service.Update(model);

                return result != null
                    ? (IActionResult)Ok(result)
                    : NoContent();
            }
            return BadRequest();
        }
        /// <summary>
        /// Delete an Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/SurveyUser
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <returns>bool</returns>
        /// <response code="200">All went well</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="405">Method not allowed</response>
        /// <response code="500">Server side error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _service.Delete(id));
            }
            return BadRequest();
        }
    }
}
