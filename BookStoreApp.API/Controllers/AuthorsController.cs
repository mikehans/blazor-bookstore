using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using AutoMapper;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookstoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AuthorsController(BookstoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDTO>>> GetAuthors()
        {
            try
            {
                var authors = _mapper.Map<IEnumerable<AuthorReadOnlyDTO>>(await _context.Authors.ToListAsync());
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on GET in {nameof(GetAuthors)}");
                return StatusCode(500,"Something bad happened.");
            }

        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDTO>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    return NotFound();
                }

                var mappedAuthor = _mapper.Map<AuthorReadOnlyDTO>(author);

                return Ok(mappedAuthor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on GET in {nameof(GetAuthor)}");
                return StatusCode(500, "Something bad happened");
            }
     
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO authorDTO)
        {
            try
            {
                if (id != authorDTO.Id)
                {
                    return BadRequest();
                }
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _mapper.Map(authorDTO, author);
                _context.Entry(author).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Concurrency error updating record");
                        return BadRequest("A problem occurred updating the record.");
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating data");
                return StatusCode(500, "Something bad happened");
            }
            
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDTO>> PostAuthor(AuthorCreateDTO authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on POST");
                return StatusCode(500, "Something bad happened");
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on DELETE for id {id}");
                return StatusCode(500, "Something bad happened");
            }
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
