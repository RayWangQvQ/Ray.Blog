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
using Ray.Blog.BlobContainers;
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
        private readonly IBlobContainer<PostImgContainer> _postImgBlobContainer;

        public BlobAppService(IBlobContainer blobContainer,IBlobContainer<PostImgContainer> postImgBlobContainer)
        {
            _blobContainer = blobContainer;
            _postImgBlobContainer = postImgBlobContainer;
        }

        public async Task<string> UploadPics(IRemoteStreamContent file)
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HHmmssss}.jpg";
            var stream = file.GetStream();
            await _postImgBlobContainer.SaveAsync(fileName, stream);
            return $"/host/post-imgs/{fileName}";
        }

        public async Task<byte[]> GetBytesAsync(string fileName)
        {
            return await _blobContainer.GetAllBytesOrNullAsync(fileName);
        }
    }
}
