using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class EmptyCard : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private RectTransform _frontImage;
        [SerializeField] private RectTransform _backImage;
        [SerializeField] private float _animTime = 0.2f;

        public bool IsOpen { get; private set; }

        private Sequence _currentSequence;

        private void SetTexture(Texture2D texture)
        {
            _rawImage.texture = texture;
        }

        private void SetTexture(byte[] bytes)
        {
            Texture2D tex = new Texture2D(240, 320);
            tex.LoadImage(bytes);
            tex.Apply();
            SetTexture(tex);
        }

        public void LoadImage(byte[] bytes)
        {
            SetTexture(bytes);
            Open();
        }
        
        [Button]
        private void Open()
        {
            if (IsOpen) return;
            IsOpen = true;

            FlipImage(_backImage, _frontImage, _animTime);
        }

        [Button]
        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;

            FlipImage(_frontImage, _backImage, _animTime);
        }

        private void FlipImage(RectTransform from, RectTransform to, float animTime)
        {
            from.localScale = Vector3.one;
            to.localScale = new Vector3(0, 1, 1);

            _currentSequence.Kill();
            _currentSequence = DOTween.Sequence();
            _currentSequence.Append(from.DOScaleX(0f, animTime).SetEase(Ease.InBack));
            _currentSequence.Append(to.DOScaleX(1f, animTime).SetEase(Ease.OutBack));
            _currentSequence.Play();
        }
    }
}