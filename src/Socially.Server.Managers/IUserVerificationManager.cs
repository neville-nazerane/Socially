using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IUserVerificationManager
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
    }
}