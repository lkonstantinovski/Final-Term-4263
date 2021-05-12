using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using midTerm.Models.Models.Answers;
using midTerm.Services.Abstractions;

namespace midTerm.Controllers
{
    /// <summary>
    /// Answers API Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _service;
        /// <summary>
        ///  Answers Consturctor API Controller
        /// </summary>
        /// <param name="service">Answers Service</param>
        public AnswersController(IAnswerService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get Item 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Answers/
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
        ///     GET /api/Answers/{id}
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
        /// Get Item by UserId
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Answers/User/{id}
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <returns>An Extended model item</returns>
        /// <response code="200">All went well</response>
        /// <response code="204">Item had no content</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="500">Server side error</response>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var result = await _service.GetByUserId(id);
            return Ok(result);
        }

        /// <summary>
        /// Create an Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Answers
        ///     {
        ///         "UserId": integer,
        ///         "OptionId": integer
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
        public async Task<IActionResult> Post([FromBody] AnswerCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = await _service.Insert(model);
                return answer != null
                    ? (IActionResult)CreatedAtRoute(nameof(GetById), answer, answer.Id)
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
        ///     PUT /api/Answers
        ///     {
        ///         "Id": 1,
        ///         "UserId": integer,
        ///         "OptionId": integer
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <param name="model">model to create</param>
        /// <returns>Answerss base model</returns>
        /// <response code="200">All went well</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="405">Method not allowed</response>
        /// <response code="409">The item was not created</response>
        /// <response code="500">Server side error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AnswersUpdateModel model)
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
        ///     DELETE /api/Answers
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
