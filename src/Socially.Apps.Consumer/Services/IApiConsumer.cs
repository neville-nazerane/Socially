using Socially.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface IApiConsumer
    {
        Task<string> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task SignupAsync(SignUpModel model, CancellationToken cancellationToken = default);
    }
}