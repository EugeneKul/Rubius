using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _Project.Scripts.ImageLoadServices;

namespace _Project.Scripts.CardLoader.Handlers
{
    public class WhenImageReadyCardLoadHandler : ICardLoadHandler
    {
        public IImageLoadService ImageLoadService { get; private set; }
        public CardHolder CardHolder { get; private set; }

        public Action<bool> BusyChanged { get; set; }
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


        private CancellationTokenSource _currentTaskToken;

        public WhenImageReadyCardLoadHandler(IImageLoadService imageLoadService, CardHolder cardHolder)
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
                tasks.Add(ImageLoadService.AsyncLoadRandomImage(240, 320, _currentTaskToken));
            }

            while (tasks.Any() && !_currentTaskToken.IsCancellationRequested)
            {
                var finishedTask = await Task.WhenAny(tasks);
                if(_currentTaskToken.IsCancellationRequested) return;
                tasks.Remove(finishedTask);
                var card = CardHolder.GetRandomClosedCard();
                card.LoadImage(finishedTask.Result);
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