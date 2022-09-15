using System;
using System.Threading;
using _Project.Scripts.ImageLoadServices;

namespace _Project.Scripts.CardLoader.Handlers
{
    public class OneByOneCardLoadHandler : ICardLoadHandler
    {
        public IImageLoadService ImageLoadService { get; private set; }
        public CardHolder CardHolder { get; private set; }
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                BusyChanged?.Invoke(value);
            }
        }
        public Action<bool> BusyChanged { get; set; }

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
                var task = service.AsyncLoadRandomImage(240,320,_currentTaskToken);
                await task;
                if (_currentTaskToken.IsCancellationRequested) return;
                card.LoadImage(task.Result);
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