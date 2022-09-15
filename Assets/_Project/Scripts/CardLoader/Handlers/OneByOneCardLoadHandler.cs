using System.Threading;
using _Project.Scripts.ImageLoadServices;

namespace _Project.Scripts.CardLoader.Handlers
{
    public class OneByOneCardLoadHandler : ICardLoadHandler
    {
        public IImageLoadService ImageLoadService { get; private set; }
        public CardHolder CardHolder { get; private set; }
        public bool IsBusy { get; private set; }

        private CancellationTokenSource _currentTaskToken;
        
        public OneByOneCardLoadHandler(IImageLoadService imageLoadService, CardHolder cardHolder)
        {
            ImageLoadService = imageLoadService;
            CardHolder = cardHolder;
        }
    
        public void Load()
        {
            LoadAsync();
        }

        public async void LoadAsync()
        {
            IsBusy = true;
            
            var service = new PicsumService();
            
            _currentTaskToken = new CancellationTokenSource();

            foreach (var card in CardHolder.Cards)
            {
                if (_currentTaskToken.IsCancellationRequested) return;
                var task = service.AsyncLoadRandomImage(240,320,_currentTaskToken);
                await task;
                card.SetTexture(task.Result);
                card.Open();
            }

            IsBusy = false;
        }
    
        public void Cancel()
        {
            _currentTaskToken?.Cancel();
            IsBusy = false;
        }
    }
}