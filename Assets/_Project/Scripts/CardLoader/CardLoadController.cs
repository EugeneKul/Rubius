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
        public bool IsBusy => _cardLoadHandler is { IsBusy: true };
        private ICardLoadHandler _cardLoadHandler;
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
        [Button]
        public void ChangeLoadType(CardLoadType type)
        {
            _cardLoadHandler = GetLoaderByType(type);
        }

        public void ChangeLoadType(int index)
        {
            ChangeLoadType((CardLoadType)index);
        }
        
        [Button]
        public void Load()
        {
            if (_cardLoadHandler == null) ChangeLoadType(CardLoadType.AllAtOnce);
            _cardHolder.ResetCards();
            _cardLoadHandler.Load();
        }
    
        [Button]
        public void Cancel()
        {
            _cardLoadHandler?.Cancel();
            _cardHolder.ResetCards();
        }
    
    }
}