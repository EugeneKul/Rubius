using System;
using _Project.Scripts.CardLoader.Handlers;
using _Project.Scripts.ImageLoadServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.CardLoader
{
    public class CardLoadController : MonoBehaviour
    {
        [SerializeField] private CardHolder _cardHolder;
        public bool IsBusy => CardLoadHandler is { IsBusy: true };
        public Action<bool> BusyChanged { get; set; }
        private ICardLoadHandler CardLoadHandler { get; set; }
        private ICardLoadHandler GetLoaderByType(CardLoadType type)
        {
            return type switch
            {
                CardLoadType.AllAtOnce => new AllAtOnceCardLoadHandler(new PicsumService(),_cardHolder),
                CardLoadType.OneByOne => new OneByOneCardLoadHandler(new PicsumService(),_cardHolder),
                CardLoadType.WhenImageReady => new WhenImageReadyCardLoadHandler(new PicsumService(),_cardHolder),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void Awake()
        {
            ChangeLoadType(CardLoadType.AllAtOnce);
        }

        [Button]
        public void ChangeLoadType(CardLoadType type)
        {
            CardLoadHandler = GetLoaderByType(type);
            CardLoadHandler.BusyChanged += BusyChanged;
        }

        public void ChangeLoadType(int index)
        {
            ChangeLoadType((CardLoadType)index);
        }
        
        [Button]
        public void Load()
        {
            if (CardLoadHandler == null) ChangeLoadType(CardLoadType.AllAtOnce);
            _cardHolder.ResetCards();
            CardLoadHandler.Load();
        }
    
        [Button]
        public void Cancel()
        {
            CardLoadHandler?.Cancel();
            _cardHolder.ResetCards();
        }
    
    }
}