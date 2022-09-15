using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts
{
    public class CardHolder : MonoBehaviour
    {
        [SerializeField] private EmptyCard _card;
        [SerializeField] private int _count;
        private List<EmptyCard> _cards = new List<EmptyCard>();
        public List<EmptyCard> Cards => _cards;

        private void Start()
        {
            SpawnCards(_count);
        }
    
        [Button]
        private void SpawnCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                EmptyCard card = Instantiate(_card, transform);
                _cards.Add(card);
            }
        }
    
        [Button]
        public void ResetCards()
        {
            foreach (var card in Cards)
            {
                card.Close();
            }
        }

        public EmptyCard GetRandomClosedCard()
        {
            var cards = Cards.Where(card => !card.IsOpen).ToList();
            return cards[Random.Range(0, cards.Count)];
        }
    
    }
}
