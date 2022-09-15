using _Project.Scripts.ImageLoadServices;

namespace _Project.Scripts.CardLoader.Handlers
{
    public interface ICardLoadHandler
    {
        public IImageLoadService ImageLoadService { get; }
        public CardHolder CardHolder { get; }
        public bool IsBusy { get; }
        public void Load();
        public void Cancel();
    }
}