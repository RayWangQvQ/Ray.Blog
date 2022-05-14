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
        private readonly IBlobContainer<CategoryImgContainer> _categoryImgBlobContainer;

        public BlobAppService(
            IBlobContainer blobContainer,
            IBlobContainer<PostImgContainer> postImgBlobContainer,
            IBlobContainer<CategoryImgContainer> categoryImgBlobContainer)
        {
            _blobContainer = blobContainer;
            _postImgBlobContainer = postImgBlobContainer;
            _categoryImgBlobContainer = categoryImgBlobContainer;
        }

        public async Task<string> UploadPostPics(IRemoteStreamContent file)
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HHmmssss}.jpg";
            var stream = file.GetStream();
            await _postImgBlobContainer.SaveAsync(fileName, stream);
            return $"/host/post-imgs/{fileName}";
        }

        public async Task<string> UploadCategoryPics(IRemoteStreamContent file)
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HHmmssss}.jpg";
            var stream = file.GetStream();
            await _categoryImgBlobContainer.SaveAsync(fileName, stream);
            return $"/host/category-imgs/{fileName}";
        }

        public async Task<byte[]> GetBytesAsync(string fileName)
        {
            return await _blobContainer.GetAllBytesOrNullAsync(fileName);
        }
    }
}
