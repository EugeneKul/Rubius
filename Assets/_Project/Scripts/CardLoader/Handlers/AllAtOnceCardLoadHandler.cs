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
        public bool IsBusy { get; private set; }

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

            EmptyCard card;

            if (_currentTaskToken.IsCancellationRequested) return;
            
            for (int i = 0; i < CardHolder.Cards.Count; i++)
            {
                card = CardHolder.Cards[i];
                card.SetTexture(tasks[i].Result);
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