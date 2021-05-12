using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using midTerm.Models.Models.Option;
using midTerm.Services.Abstractions;

namespace midTerm.Controllers
{
    /// <summary>
    /// Option API Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionService _service;
        /// <summary>
        ///  Option Consturctor API Controller
        /// </summary>
        /// <param name="service">Option Service</param>
        public OptionsController(IOptionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Item 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Options/
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
            return result != null && result.Any()
                 ? (IActionResult)Ok(result)
                 : NoContent();
        }
        /// <summary>
        /// Get Item by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Options/{id}
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <returns>An Extended model item</returns>
        /// <response code="200">All went well</response>
        /// <response code="204">Item had no content</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="500">Server side error</response>
        [HttpGet("{id}")]
        //added a different type of return result function
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return result != null
                ? (IActionResult)Ok(result)
                : NoContent();
        }
        /// <summary>
        /// Get Item by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Options/Question/{id}
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <returns>An Extended model item</returns>
        /// <response code="200">All went well</response>
        /// <response code="204">Item had no content</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="500">Server side error</response>
        [HttpGet("question/{id}")]
        public async Task<IActionResult> GetByQuestionId(int id)
        {
            var result = await _service.GetByQuestionId(id);
            return Ok(result);
        }

        /// <summary>
        /// Create an Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Options
        ///     {
        ///         "Text":"string",
        ///         "Order": integer,
        ///         "QuestionId": integer
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
        public async Task<IActionResult> Post([FromBody] OptionCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var option = await _service.Insert(model);

                if (option != null)
                {

                    return Created($"/api/Options/{option.Id}", option.Id);
                }
                return Conflict();
            }
            return BadRequest();
        }
        /// <summary>
        /// Update an Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Match
        ///     {
        ///         "Id": 1,
        ///         "Text":"string",
        ///         "Order": integer,
        ///         "QuestionId": integer
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">identifier of the item</param>
        /// <param name="model">model to create</param>
        /// <returns>Option base model</returns>
        /// <response code="200">All went well</response>
        /// <response code="400">The Item is NULL</response>
        /// <response code="405">Method not allowed</response>
        /// <response code="409">The item was not created</response>
        /// <response code="500">Server side error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OptionUpdateModel model)
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
        ///     DELETE /api/Options/1
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
