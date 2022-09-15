using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _Project.Scripts.ImageLoadServices;

namespace _Project.Scripts.CardLoader.Handlers
{
    public class AllAtOnceCardLoadHandler : ICardLoadHandler
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
        
        public AllAtOnceCardLoadHandler(IImageLoadService imageLoadService, CardHolder cardHolder)
        {
            ImageLoadService = imageLoadService;
            CardHolder = cardHolder;
        }

        public void Load()
        {
            AsyncLoad();
        }

        private async void AsyncLoad()
        {
            IsBusy = true;
            var tasks = new List<Task<byte[]>>();
            
            _currentTaskToken = new CancellationTokenSource();

            for (int i = 0; i < CardHolder.Cards.Count; i++)
            {
                tasks.Add(ImageLoadService.AsyncLoadRandomImage(240, 320,_currentTaskToken));
            }

            await Task.WhenAll(tasks.ToArray());

            if (_currentTaskToken.IsCancellationRequested) return;
            
            for (int i = 0; i < CardHolder.Cards.Count; i++)
            {
                CardHolder.Cards[i].LoadImage(tasks[i].Result);
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