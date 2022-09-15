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
        public IImageLoadService ImageLoadService { get; private set;}
        public CardHolder CardHolder { get; private set;}
        public bool IsBusy { get; private set; }

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
                tasks.Add(ImageLoadService.AsyncLoadRandomImage(240,320,_currentTaskToken));
            }
        
            while (tasks.Any() || !_currentTaskToken.IsCancellationRequested)
            {
                var finishedTask = await Task.WhenAny(tasks);
                tasks.Remove(finishedTask);
                var card = CardHolder.GetRandomClosedCard();
                card.SetTexture(finishedTask.Result);
                card.Open();
            }
        }
    
        public void Cancel()
        {
            _currentTaskToken?.Cancel();
            IsBusy = false;
        }
    }
}