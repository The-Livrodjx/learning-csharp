using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("superhero")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        public static List<SuperHero> heroes = new List<SuperHero>
        {
            new SuperHero
            {
                Id = 1,
                Name = "Livrodjx",
                FirstName = "Livro",
                LastName = "djx"
            }
        };

        private DataContext _context;

        public SuperHeroController(DataContext dataContext) 
        {
            _context = dataContext;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id) 
        {
            var hero = await _context.SuperHeroes.FindAsync(id);

            if(hero == null)
                return NotFound("Hero not found");
            

            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            var heroExists = await _context.SuperHeroes.FindAsync(hero.Name);
            if (heroExists == null)
            {
                _context.SuperHeroes.Add(hero);
                await _context.SaveChangesAsync();
                return Ok(await _context.SuperHeroes.ToListAsync());
            }   
            else
                return BadRequest("Hero already exists");
            
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(request.Id);
            if (dbHero == null)
                return BadRequest("Hero not found");

            dbHero.Name = request.Name;
            dbHero.FirstName = request.FirstName;
            dbHero.LastName = request.LastName;

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(id);

            if (dbHero == null)
                return NotFound("Hero not found");

            _context.SuperHeroes.Remove(dbHero);

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }   
}
