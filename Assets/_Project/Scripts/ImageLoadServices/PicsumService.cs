using System.Threading;
using System.Threading.Tasks;
using _Project.Scripts.Utility;

namespace _Project.Scripts.ImageLoadServices
{
    public class PicsumService : IImageLoadService
    {
        public async Task<byte[]> AsyncLoadRandomImage(int width, int height,CancellationTokenSource cancelTokenSource)
        {
            var task =  WebRequestHelper.AsyncGetRequest($"https://picsum.photos/{width}/{height}",cancelTokenSource);
            await task;
            return task.Result.DownloadHandler.data;
        }
    }
}