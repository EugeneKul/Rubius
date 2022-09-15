using System;
using System.Linq;
using _Project.Scripts.CardLoader;
using _Project.Scripts.CardLoader.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private CardLoadController _cardLoadController;
        [SerializeField] private TMP_Dropdown _dropdownMenu;
        [SerializeField] private Button _buttonLoad;
        [SerializeField] private Button _buttonCancel;
        
        void Start()
        {
            FillDropDownMenu();
            AddListeners();
        }

        private void AddListeners()
        {
            _buttonLoad.onClick.AddListener(_cardLoadController.Load);
            _buttonCancel.onClick.AddListener(_cardLoadController.Cancel);
            _dropdownMenu.onValueChanged.AddListener(_cardLoadController.ChangeLoadType);
        }

        public void Update()
        {
            if (_cardLoadController.IsBusy)
            {
                _buttonLoad.interactable = false;
                _dropdownMenu.interactable = false;
                _buttonCancel.interactable = true;
            }
            else
            {
                _buttonLoad.interactable = true;
                _dropdownMenu.interactable = true;
                _buttonCancel.interactable = false;
            }
        }

        private void FillDropDownMenu()
        {
            var names = Enum.GetNames(typeof(CardLoadType)).ToList();
            _dropdownMenu.AddOptions(names);
        }
    }
}
