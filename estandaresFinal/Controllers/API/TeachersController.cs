using estandaresFinal.Data;
using estandaresFinal.Data.Entities;
using estandaresFinal.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Controllers.API
{
    [Route("api/[Controller]")]
    public class TeachersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public TeachersController(DataContext dataContext, IUserHelper userHelper)
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
        public IActionResult GetTeachersAsync()
        {
            return Ok(this.dataContext.Teachers.Include(t => t.User).ToList());
        }

        // Listo
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeachers([FromRoute] int? id)
        {
            if (id == null || dataContext.Teachers == null)
            {
                return BadRequest(ModelState);
            }

            var teacher = await dataContext.Teachers.Include(t => t.User).FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(this.dataContext.Teachers.FindAsync(id));
        }

        // Listo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacher = await dataContext.Teachers.Include(t => t.User).FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return BadRequest(ModelState);
            }

            dataContext.Teachers.Remove(teacher);
            await dataContext.SaveChangesAsync();
            return Ok(teacher);
        }

        // Listo
        [HttpPost]
        public async Task<IActionResult> PostTeacher([FromBody] estadaresFinal.Common.Models.Teacher teacherCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await AddUser
            (
                teacherCommon.FirstName,
                teacherCommon.LastName,
                teacherCommon.Enrollment,
                teacherCommon.Email,
                teacherCommon.Password,
                teacherCommon.PhoneNumber,
                teacherCommon.Rol = "Teacher"
             );

            dataContext.Teachers.Add(new Teacher { User = user });
            await dataContext.SaveChangesAsync();
            return Ok(user);
        }

        // Listo
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher([FromRoute] int id, [FromBody] estadaresFinal.Common.Models.Teacher teacherCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacherCommon.Id)
            {
                return BadRequest(ModelState);
            }

            var oldTeacher = await dataContext.Teachers.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (oldTeacher == null)
            {
                return BadRequest("No existe el coordinador");
            }

            if (oldTeacher.User == null)
            {
                return BadRequest("No existe el coordinador");
            }

            oldTeacher.Id = teacherCommon.Id;
            oldTeacher.User.FirstName = teacherCommon.FirstName;
            oldTeacher.User.LastName = teacherCommon.LastName;
            oldTeacher.User.Enrollment = teacherCommon.Enrollment;
            oldTeacher.User.Email = teacherCommon.Email;
            oldTeacher.User.PhoneNumber = teacherCommon.PhoneNumber;
            teacherCommon.Rol = "Coordinator";

            dataContext.Update(oldTeacher);
            await dataContext.SaveChangesAsync();
            return Ok(oldTeacher);
        }
    }
}
