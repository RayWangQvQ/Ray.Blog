﻿using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Ray.Blog.Blobs
{
    public interface IBlobAppService : IApplicationService
    {
        Task<string> UploadPics(IRemoteStreamContent file);

        Task<byte[]> GetBytesAsync(string fileName);
    }
}