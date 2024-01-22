using estandaresFinal.Data;
using estandaresFinal.Data.Entities;
using estandaresFinal.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Controllers.API
{
    [Route("api/[Controller]")]
    public class StudentsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public StudentsController(DataContext dataContext, IUserHelper userHelper)
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

        private async Task<Career> AddCareer(string Career)
        {
            var career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == Career);

            return career;
        }

        // Listo
        public IActionResult GetStudentsAsync()
        {
            return Ok(this.dataContext.Students.Include(s => s.Career).Include(s => s.User).ToList());
        }

        // Listo
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int? id)
        {
            if (id == null || dataContext.Students == null)
            {
                return BadRequest(ModelState);
            }

            var stundent = await dataContext.Students.Include(s => s.Career).Include(s => s.User).FirstOrDefaultAsync(m => m.Id == id);

            if (stundent == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(this.dataContext.Students.FindAsync(id));
        }

        // Listo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await dataContext.Students.Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return BadRequest(ModelState);
            }

            dataContext.Students.Remove(student);
            await dataContext.SaveChangesAsync();
            return Ok(student);
        }

        // Listo
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] estadaresFinal.Common.Models.Student studentCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await AddUser
            (
                studentCommon.FirstName,
                studentCommon.LastName,
                studentCommon.Enrollment,
                studentCommon.Email,
                studentCommon.Password,
                studentCommon.PhoneNumber,
                studentCommon.Rol = "Student"
             );

            var career = await AddCareer
            (
                studentCommon.Career
            );

            dataContext.Students.Add(new Student { User = user, Career = career });
            await dataContext.SaveChangesAsync();
            return Ok(user);
        }

        // Listo
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] estadaresFinal.Common.Models.Student studentCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentCommon.Id)
            {
                return BadRequest(ModelState);
            }

            var oldStudent = await dataContext.Students.Include(s => s.Career).Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (oldStudent == null)
            {
                return BadRequest("No existe el estudiente");
            }

            if (oldStudent.User == null)
            {
                return BadRequest("No existe el estudiente");
            }

            oldStudent.Id = studentCommon.Id;
            oldStudent.User.FirstName = studentCommon.FirstName;
            oldStudent.User.LastName = studentCommon.LastName;
            oldStudent.User.Enrollment = studentCommon.Enrollment;
            oldStudent.User.Email = studentCommon.Email;
            oldStudent.User.PhoneNumber = studentCommon.PhoneNumber;
            studentCommon.Rol = "Student";

            dataContext.Update(oldStudent);
            await dataContext.SaveChangesAsync();
            return Ok(oldStudent);
        }
    }
}
