using Microsoft.AspNetCore.Http;
using Ray.Blog.Blobs;
using Ray.Blog.Categories;
using Ray.Blog.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    public class BlobAppService : ApplicationService, IBlobAppService
    {
        private readonly IBlobContainer _blobContainer;

        public BlobAppService(IBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public async Task UploadPics(IRemoteStreamContent file)
        {
            var stream = file.GetStream();
            await _blobContainer.SaveAsync("my-blob-1", stream, true);
        }

        public async Task<byte[]> GetBytesAsync()
        {
            return await _blobContainer.GetAllBytesOrNullAsync("my-blob-1");
        }
    }
}
