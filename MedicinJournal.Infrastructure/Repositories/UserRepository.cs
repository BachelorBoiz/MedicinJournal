using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;
using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MedicinJournalDbContext _dbContext;

        public UserRepository(MedicinJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _dbContext.Users.Where(entity => entity.Id == id).Select(entity => new User
            {
                Id = entity.Id,
                Name = entity.Name,
                BirthDate = entity.BirthDate,
                Gender = entity.Gender,
                Height = entity.Height,
                Weight = entity.Weight,
            }).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUser(User user)
        {
            var entity = new UserEntity
            {
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Height = user.Height,
                Weight = user.Weight,
                Name = user.Name
            };

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            user.Id = entity.Id;

            return user;
        }
    }
}
