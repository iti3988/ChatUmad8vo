using estandaresFinal.Data.Entities;
using estandaresFinal.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Data
{
    public class Seeder
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public Seeder(DataContext dataContext, IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await dataContext.Database.EnsureCreatedAsync();
            await userHelper.CheckRoleAsync("Admin");
            await userHelper.CheckRoleAsync("Capturer");
            await userHelper.CheckRoleAsync("Coordinator");
            await userHelper.CheckRoleAsync("Student");
            await userHelper.CheckRoleAsync("Teacher");

            await CheckCareersAsync();
            await CheckChatTypesAsync();
            await CheckAdminsAsync();
            await CheckCapturersAsync();
            await CheckCoordinatorsAsync();
            await CheckStudentsAsync();
            await CheckTeachersAsync();
            await CheckChatsAsync();
            await CheckChatMembersAsync();
            await CheckMessagesAsync();
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
                    throw new InvalidOperationException("Usuario no pude ser creado en el seeder");
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
        private async Task CheckCareersAsync()
        {
            if (!dataContext.Careers.Any())
            {
                dataContext.Careers.Add(new Entities.Career { Name = "Ingeniería en software", Acronym = "ISF" });
                dataContext.Careers.Add(new Entities.Career { Name = "Ingeniería industrial y rentabilidad de negocios", Acronym = "IIRN" });
                dataContext.Careers.Add(new Entities.Career { Name = "Ingeniería mecatrónica", Acronym = "IM" });
                dataContext.Careers.Add(new Entities.Career { Name = "Ingeniería en diseño industrial", Acronym = "IDI" });
                dataContext.Careers.Add(new Entities.Career { Name = "Diseño y negocios de la moda", Acronym = "DNM" });
                dataContext.Careers.Add(new Entities.Career { Name = "Diseño digital", Acronym = "DD" });
                dataContext.Careers.Add(new Entities.Career { Name = "Lenguas extranjeras", Acronym = "LE" });
                dataContext.Careers.Add(new Entities.Career { Name = "Arquitectura e interiorismo", Acronym = "AI" });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckChatTypesAsync()
        {
            if (!dataContext.ChatTypes.Any())
            {
                dataContext.ChatTypes.Add(new Entities.ChatType { Name = "Avisos generales" });
                dataContext.ChatTypes.Add(new Entities.ChatType { Name = "Grupal" });
                dataContext.ChatTypes.Add(new Entities.ChatType { Name = "Privado" });

                await dataContext.SaveChangesAsync();

            }
        }
        private async Task CheckAdminsAsync()
        {
            if (!dataContext.Admins.Any())
            {
                var user = await AddUser("Carlos", "Zapata", "12345", "carlos.admin@umad.edu.mx", "123456", "2222222222", "Admin");
                dataContext.Admins.Add(new Admin { User = user });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckCapturersAsync()
        {
            if (!dataContext.Capturers.Any())
            {
                var user = await AddUser("Diego", "Pedraza", "12345", "diego.pedraza@umad.edu.mx", "123456", "2222222222", "Capturer");
                dataContext.Capturers.Add(new Capturer { User = user });
                user = await AddUser("Aarmando", "Gonzalez", "12345", "armando.gonzalez@umad.edu.mx", "123456", "2222222222", "Capturer");
                dataContext.Capturers.Add(new Capturer { User = user });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckCoordinatorsAsync()
        {
            if (!dataContext.Coordinators.Any())
            {
                var user = await AddUser("Carlos", "Zapata", "12345", "carlos.zapata@umad.edu.mx", "123456", "2222222222", "Coordinator");
                dataContext.Coordinators.Add(new Coordinator { User = user });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckStudentsAsync()
        {
            if (!dataContext.Students.Any())
            {
                var career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería en software");
                var user = await AddUser("Emigdio", "Espinoza", "12345", "emigdio.espinoza@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería en software");
                user = await AddUser("Juan", "Perez", "12345", "juan.perez@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería en software");
                user = await AddUser("Diego", "Pedraza", "12345", "diego.pedraza@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería en software");
                user = await AddUser("Nadia", "Ramos", "12345", "nadia.ramos@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería industrial y rentabilidad de negocios");
                user = await AddUser("Manuel", "Perez", "12345", "manuel.perez@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería industrial y rentabilidad de negocios");
                user = await AddUser("Jorge", "Ramos", "12345", "jorge.ramos@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería mecatrónica");
                user = await AddUser("Manuel", "Triunfo", "12345", "manuel.triunfo@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería mecatrónica");
                user = await AddUser("Gal", "Gadot", "12345", "gal.gadot@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });
                career = await dataContext.Careers.FirstOrDefaultAsync(c => c.Name == "Ingeniería en diseño industrial");
                user = await AddUser("Mariana", "Lunares", "12345", "mariana.Lunares@umad.edu.mx", "123456", "2222222222", "Student");
                dataContext.Students.Add(new Student { User = user, Career = career });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckTeachersAsync()
        {
            if (!dataContext.Teachers.Any())
            {
                var user = await AddUser("Eduardo", "Fong", "12345", "eduardo.fong@umad.edu.mx", "123456", "2222222222", "Teacher");
                dataContext.Teachers.Add(new Teacher { User = user });
                user = await AddUser("Patricia", "Hernandez", "12345", "patricia.hernandez@umad.edu.mx", "123456", "2222222222", "Teacher");
                dataContext.Teachers.Add(new Teacher { User = user });
                user = await AddUser("Nicolas", "Arrioja", "12345", "nicolas.arrioja@umad.edu.mx", "123456", "2222222222", "Teacher");
                dataContext.Teachers.Add(new Teacher { User = user });
                user = await AddUser("Jorge", "Castelan", "12345", "jorge.castelan@umad.edu.mx", "123456", "2222222222", "Teacher");
                dataContext.Teachers.Add(new Teacher { User = user });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckChatsAsync()
        {
            if (!dataContext.Chats.Any())
            {
                var chatType = await dataContext.ChatTypes.FirstOrDefaultAsync(c => c.Name == "Privado");
                dataContext.Chats.Add(new Chat { Date = DateTime.Now, ChatType = chatType });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckChatMembersAsync()
        {
            if (!dataContext.ChatMembers.Any())
            {
                var chat = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == 1);
                var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == "carlos.zapata@umad.edu.mx");
                dataContext.ChatMembers.Add(new ChatMember { Chat = chat, User = user });
                chat = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == 1);
                user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == "emigdio.espinoza@umad.edu.mx");
                dataContext.ChatMembers.Add(new ChatMember { Chat = chat, User = user });

                await dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckMessagesAsync()
        {
            if (!dataContext.Messages.Any())
            {
                var chat = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == 1);
                var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == "carlos.zapata@umad.edu.mx");
                dataContext.Messages.Add(new Message { Chat = chat, User = user, Date = DateTime.Now, MessageDescription = "Feria mañana" });
                chat = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == 1);
                user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == "emigdio.espinoza@umad.edu.mx");
                dataContext.Messages.Add(new Message { Chat = chat, User = user, Date = DateTime.Now, MessageDescription = "Enterado, saludos cordiales" });
                chat = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == 1);
                user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == "carlos.zapata@umad.edu.mx");
                dataContext.Messages.Add(new Message { Chat = chat, User = user, Date = DateTime.Now, MessageDescription = ":)" });

                await dataContext.SaveChangesAsync();
            }
        }
    }
}