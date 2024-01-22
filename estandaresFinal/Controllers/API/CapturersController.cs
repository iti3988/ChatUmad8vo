using estandaresFinal.Data;
using estandaresFinal.Data.Entities;
using estandaresFinal.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Controllers.API
{
    [Route("api/[Controller]")]
    public class CapturersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public CapturersController(DataContext dataContext, IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        private async Task<User> AddUser(string firstName, string lastName, string enrollment, string email, string password, string phoneNumber, string rol)
        {
            var user = await userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Enrollment = enrollment,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    UserName = email
                };
                var result = await userHelper.AddUserAsync(user, password);
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Usuario no pude ser creado");
                }
                await userHelper.AddUserToRoleAsync(user, rol);
                return user;
            }
            var isInRol = await userHelper.IsUserInRoleAsync(user, rol);
            if (!isInRol)
            {
                await userHelper.AddUserToRoleAsync(user, rol);
            }
            return user;
        }
        
        // Listo
        public IActionResult GetCapturersAsync()
        {
            return Ok(this.dataContext.Capturers.Include(c => c.User).ToList());
        }

        // Listo
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCapturer([FromRoute] int? id)
        {
            if (id == null || dataContext.Capturers == null)
            {
                return BadRequest(ModelState);
            }

            var coordinator = await dataContext.Capturers.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (coordinator == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(this.dataContext.Capturers.FindAsync(id));
        }

        // Listo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCapturer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capturer = await dataContext.Capturers.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (capturer == null)
            {
                return BadRequest(ModelState);
            }

            dataContext.Capturers.Remove(capturer);
            await dataContext.SaveChangesAsync();
            return Ok(capturer);
        }

        // Listo
        [HttpPost]
        public async Task<IActionResult> PostCapturer([FromBody] estadaresFinal.Common.Models.Capturer capturerCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await AddUser
            (
                capturerCommon.FirstName,
                capturerCommon.LastName,
                capturerCommon.Enrollment,
                capturerCommon.Email,
                capturerCommon.Password,
                capturerCommon.PhoneNumber,
                capturerCommon.Rol = "Capturer"
             );

            dataContext.Capturers.Add(new Capturer { User = user });
            await dataContext.SaveChangesAsync();
            return Ok(user);
        }

        // Listo
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCapturer([FromRoute] int id, [FromBody] estadaresFinal.Common.Models.Capturer capturerCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != capturerCommon.Id)
            {
                return BadRequest(ModelState);
            }

            var oldCapturer = await dataContext.Capturers.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (oldCapturer == null)
            {
                return BadRequest("No existe el coordinador");
            }

            if (oldCapturer.User == null)
            {
                return BadRequest("No existe el coordinador");
            }

            oldCapturer.Id = capturerCommon.Id;
            oldCapturer.User.FirstName = capturerCommon.FirstName;
            oldCapturer.User.LastName = capturerCommon.LastName;
            oldCapturer.User.Enrollment = capturerCommon.Enrollment;
            oldCapturer.User.Email = capturerCommon.Email;
            oldCapturer.User.PhoneNumber = capturerCommon.PhoneNumber;
            capturerCommon.Rol = "Coordinator";

            dataContext.Update(oldCapturer);
            await dataContext.SaveChangesAsync();
            return Ok(oldCapturer);
        }
    }
}
