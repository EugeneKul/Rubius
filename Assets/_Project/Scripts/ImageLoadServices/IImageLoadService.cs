using System.Threading;
using System.Threading.Tasks;

namespace _Project.Scripts.ImageLoadServices
{
    public interface IImageLoadService
    {
        public Task<byte[]> AsyncLoadRandomImage(int width, int height,CancellationTokenSource cancelTokenSource);
    }
}