using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public class ImageManager : IImageManager
    {
        private const string conatinerName = "userprofiles";
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ImageManager> _logger;
        private readonly IBlobAccess _blobAccess;

        public ImageManager(ApplicationDbContext dbContext, 
                            ILogger<ImageManager> logger,
                            IBlobAccess blobAccess)
        {
            _dbContext = dbContext;
            _logger = logger;
            _blobAccess = blobAccess;
        }

        public async Task<string> AddAsync(int userId,
                                   string fileExtension,
                                   Stream stream,
                                   CancellationToken cancellationToken = default)
        {
            var fileName = $"{Guid.NewGuid():N}{fileExtension}";
            await _blobAccess.UploadAsync(conatinerName, fileName, stream, cancellationToken);
            await _dbContext.ProfileImages
                                .AddAsync(new ProfileImage
                                {
                                    FileName = fileName,
                                    UserId = userId
                                }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return fileName;
        }

        public async Task<IEnumerable<string>> GetAllForUserAsync(int userId, CancellationToken cancellationToken = default)
            => await _dbContext.ProfileImages
                                 .Where(i => i.UserId == userId)
                                 .Select(i => i.FileName)
                                 .ToListAsync(cancellationToken);

        public async Task DeleteByNameAsync(int userId,
                                            string fileName,
                                            CancellationToken cancellationToken = default)
        {
            await _blobAccess.DeleteAsync(conatinerName, fileName, cancellationToken);
            var img = await _dbContext.ProfileImages.SingleOrDefaultAsync(i => i.UserId == userId && i.FileName == fileName,
                                                                          cancellationToken: CancellationToken.None);
            if (img is null)
            {
                _logger.LogWarning("Failed to delete fileName {fileName} for userId {userId}", fileName, userId);
                return;
            }
            _dbContext.ProfileImages.Remove(img);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

    }
}
