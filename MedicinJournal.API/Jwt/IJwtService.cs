using MedicinJournal.API.Dtos;

namespace MedicinJournal.API.Jwt
{
    public interface IJwtService
    {
        Task<JwtToken> GenerateJwt(string userName, string password);
    }
}
