using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using MovieService.Api.DTO;
using MovieService.Api.Interfaces;
using MovieService.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CachedEntriesController : ODataController
    {
        private readonly IRepository<CachedEntry> _repository;

        public CachedEntriesController(IRepository<CachedEntry> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var cachedEntries = _repository.GetAllAsync();
            return Ok(cachedEntries);
        }

        [HttpGet("({key})")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var cachedEntry = await _repository.GetByIdAsync(key);
            if (cachedEntry == null)
            {
                return NotFound();
            }
            return Ok(cachedEntry);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CachedEntryDto cachedEntryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cachedEntry = new CachedEntry
            {
                Title = cachedEntryDto.Title,
                Year = cachedEntryDto.Year,
                ImdbID = cachedEntryDto.ImdbID,
                // Map other properties as necessary
            };

            await _repository.AddAsync(cachedEntry);
            return CreatedAtAction(nameof(Get), new { key = cachedEntry.Id }, cachedEntry);
        }

        [HttpPut("({key})")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] CachedEntryDto cachedEntryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cachedEntry = await _repository.GetByIdAsync(key);
            if (cachedEntry == null)
            {
                return NotFound();
            }

            cachedEntry.Title = cachedEntryDto.Title;
            cachedEntry.Year = cachedEntryDto.Year;
            cachedEntry.ImdbID = cachedEntryDto.ImdbID;
            // Update other properties as necessary

            await _repository.UpdateAsync(cachedEntry);
            return NoContent();
        }

        [HttpDelete("({key})")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var cachedEntry = await _repository.GetByIdAsync(key);
            if (cachedEntry == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(key);
            return NoContent();
        }
    }
}