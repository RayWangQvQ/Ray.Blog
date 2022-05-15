using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Ray.Blog.Blobs;
using Ray.Blog.Categories;
using Ray.Blog.Localization;
using Ray.Blog.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Content;

namespace Ray.Blog.Blazor.Pages.Admin
{
    [Authorize(BlogPermissions.Categories.Default)]
    public partial class AdminCategories: AbpCrudPageBase<ICategoryAppService, CategoryDto, Guid, PagedAndSortedResultRequestDto, CreateCategoryDto>
    {
        [Inject]
        protected AbpBlazorMessageLocalizerHelper<BlogResource> LH { get; set; }

        [Inject]
        protected IBlobAppService BlobAppService { get; set; }

        [Inject]
        protected IWebAssemblyHostEnvironment Environment { get; set; }

        [Inject]
        protected IConfiguration Configuration { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CreatePolicyName = BlogPermissions.Categories.Create;
            UpdatePolicyName = BlogPermissions.Categories.Edit;
            DeletePolicyName = BlogPermissions.Categories.Delete;

            await base.OnInitializedAsync();
        }

        #region UploadImg

        private Dictionary<string, Stream> _imageStreamDictionary = new Dictionary<string, Stream>();

        async Task OnImageUploadChanged(FileChangedEventArgs e)
        {
            var file = e.Files.FirstOrDefault();
            await using var stream = new System.IO.MemoryStream();
            _imageStreamDictionary.Add(file.Name, stream);
            await file.WriteToStreamAsync(stream);
        }

        async Task OnImageUploadEnded(FileEndedEventArgs e)
        {
            var stream = _imageStreamDictionary[e.File.Name];
            stream.Seek(0, SeekOrigin.Begin);
            var imgUri = await BlobAppService.UploadCategoryPics(new RemoteStreamContent(stream, e.File.Name));

            var hostBaseAddress = Configuration["RemoteServices:Default:BaseUrl"];
            e.File.UploadUrl = $"{hostBaseAddress}{imgUri}";
            NewEntity.PicUrl = EditingEntity.PicUrl = e.File.UploadUrl;
            Console.WriteLine($"Finished Image: {e.File.Name}, Success: {e.Success}");

            _imageStreamDictionary.Remove(e.File.Name);
        }

        #endregion
    }
}
