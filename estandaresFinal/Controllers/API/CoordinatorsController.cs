using estandaresFinal.Data;
using estandaresFinal.Data.Entities;
using estandaresFinal.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Controllers.API
{
    [Route("api/[Controller]")]
    public class CoordinatorsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public CoordinatorsController(DataContext dataContext, IUserHelper userHelper)
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
        public IActionResult GetCoordinatorsAsync()
        {
            return Ok(this.dataContext.Coordinators.Include(c => c.User).ToList());
        }

        // Listo
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoordinator([FromRoute] int? id)
        {
            if (id == null || dataContext.Coordinators == null)
            {
                return BadRequest(ModelState);
            }

            var coordinator = await dataContext.Coordinators.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (coordinator == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(this.dataContext.Coordinators.FindAsync(id));
        }

        // Listo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoordinator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coordinator = await dataContext.Coordinators.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (coordinator == null)
            {
                return BadRequest(ModelState);
            }

            dataContext.Coordinators.Remove(coordinator);
            await dataContext.SaveChangesAsync();
            return Ok(coordinator);
        }

        // Listo
        [HttpPost]
        public async Task<IActionResult> PostCoordinator([FromBody] estadaresFinal.Common.Models.Coordinator coordinatorCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await AddUser
            (
                coordinatorCommon.FirstName,
                coordinatorCommon.LastName,
                coordinatorCommon.Enrollment,
                coordinatorCommon.Email,
                coordinatorCommon.Password,
                coordinatorCommon.PhoneNumber,
                coordinatorCommon.Rol = "Coordinator"
             );

            dataContext.Coordinators.Add(new Coordinator { User = user });
            await dataContext.SaveChangesAsync();
            return Ok(user);
        }

        // Listo
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoordinator([FromRoute] int id, [FromBody] estadaresFinal.Common.Models.Coordinator coordinatorCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coordinatorCommon.Id)
            {
                return BadRequest(ModelState);
            }

            var oldCoordinators = await dataContext.Coordinators.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (oldCoordinators == null)
            {
                return BadRequest("No existe el coordinador");
            }

            if (oldCoordinators.User == null)
            {
                return BadRequest("No existe el coordinador");
            }

            oldCoordinators.Id = coordinatorCommon.Id;
            oldCoordinators.User.FirstName = coordinatorCommon.FirstName;
            oldCoordinators.User.LastName = coordinatorCommon.LastName;
            oldCoordinators.User.Enrollment = coordinatorCommon.Enrollment;
            oldCoordinators.User.Email = coordinatorCommon.Email;
            oldCoordinators.User.PhoneNumber = coordinatorCommon.PhoneNumber;
            coordinatorCommon.Rol = "Coordinator";

            dataContext.Update(oldCoordinators);
            await dataContext.SaveChangesAsync();
            return Ok(oldCoordinators);
        }
    }
}
