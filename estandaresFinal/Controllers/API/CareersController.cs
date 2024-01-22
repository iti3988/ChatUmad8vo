using estadaresFinal.Common.Models;
using estandaresFinal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Controllers.API
{
    [Route("api/[Controller]")]
    public class CareersController : Controller
    {
        private readonly DataContext dataContext;
        public CareersController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IActionResult GetCareers()
        {
            return Ok(this.dataContext.Careers.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCareer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var career = await dataContext.Careers
                .Where(c => c.Id == id)
                .SelectMany(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            //var career = await dataContext.Students
            //    .Include(s => s.Career.Where(c => c.Id == id)));

            //var career = await dataContext.Careers
            //            .Include(c => c.Students
            //            .Where(s => s.Id == c.Id)
            //            .FirstOrDefaultAsync(c => c.Id == id));

            if (career == null)
            {
                return BadRequest("No existe la carrera");
            }
            return Ok(career);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCareer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var career = await this.dataContext.Careers.FirstOrDefaultAsync(s => s.Id == id);
            if (career == null)
            {
                return BadRequest(ModelState);
            }
            dataContext.Careers.Remove(career);
            await this.dataContext.SaveChangesAsync();
            return Ok(career);
        }

        [HttpPost]
        public async Task<IActionResult> PostCareer([FromBody] estadaresFinal.Common.Models.Career modelCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var career = new Career
            {
                Id = modelCommon.Id,
                Name = modelCommon.Name,
                Acronym = modelCommon.Acronym
            };
            dataContext.Add(career);
            await this.dataContext.SaveChangesAsync();
            return Ok(career);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCareer([FromRoute] int id, [FromBody] estadaresFinal.Common.Models.Career modelCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != modelCommon.Id)
            {
                return BadRequest(ModelState);
            }
            var oldCareer = await dataContext.Careers.FindAsync(id);
            if (oldCareer == null)
            {
                return BadRequest("No existe la carrera que quieres modificar");
            }
            oldCareer.Id = modelCommon.Id;
            oldCareer.Name = modelCommon.Name;
            oldCareer.Acronym = modelCommon.Acronym;
            dataContext.Update(oldCareer);
            await this.dataContext.SaveChangesAsync();
            return Ok(oldCareer);
        }
    }
}
