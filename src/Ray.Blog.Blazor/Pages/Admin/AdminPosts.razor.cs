using Blazorise;
using Ray.Blog.Permissions;
using Ray.Blog.Posts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Net.Http.Headers;
using Ray.Blog.Blobs;
using Volo.Abp.Content;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Ray.Blog.Blazor.Pages.Admin
{
    public partial class AdminPosts
    {
        [Inject]
        protected IBlobAppService BlobAppService { get; set; }

        [Inject]
        protected IWebAssemblyHostEnvironment Environment { get; set; }

        IReadOnlyList<CategoryLookupDto> categoryAllList = Array.Empty<CategoryLookupDto>();
        IReadOnlyList<TagLookupDto> tagAllList = Array.Empty<TagLookupDto>();

        public AdminPosts()
        {
            CreatePolicyName = BlogPermissions.Posts.Create;
            UpdatePolicyName = BlogPermissions.Posts.Edit;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //获取类别下拉列表
            categoryAllList = (await AppService.GetCategoryLookupAsync()).Items;

            //获取标签列表
            tagAllList = (await AppService.GetTagLookupAsync()).Items;
        }

        private TagLookupDto SelectedTag { get; set; }

        protected TagLookupDto GetTagById(Guid tagId)
        {
            return tagAllList.FirstOrDefault(x => x.Id == tagId);
        }

        protected Color GetRandomColor()
        {
            Color[] enums = Enum.GetValues(typeof(Color)) as Color[];
            Random random = new();
            return enums[random.Next(0, enums.Length)];
        }


        #region UploadImg

        private Dictionary<string, Stream> _imageStreamDictionary = new Dictionary<string, Stream>();

        async Task OnImageUploadChanged(FileChangedEventArgs e)
        {
            try
            {
                var file = e.Files.FirstOrDefault();
                await using var stream = new System.IO.MemoryStream();
                _imageStreamDictionary.Add(file.Name, stream);
                await file.WriteToStreamAsync(stream);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                this.StateHasChanged();
            }
        }

        async Task OnImageUploadEnded(FileEndedEventArgs e)
        {
            var stream = _imageStreamDictionary[e.File.Name];
            stream.Seek(0, SeekOrigin.Begin);
            var imgUri = await BlobAppService.UploadPics(new RemoteStreamContent(stream, e.File.Name));

            e.File.UploadUrl = $"{this.Environment.BaseAddress}{imgUri}";// We need to report back to Markdown that upload is done. We do this by setting the UploadUrl.

            Console.WriteLine($"Finished Image: {e.File.Name}, Success: {e.Success}");

            _imageStreamDictionary.Remove(e.File.Name);
        }

        #endregion
    }
}
