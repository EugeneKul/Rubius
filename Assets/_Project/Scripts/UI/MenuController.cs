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
            _cardLoadController.BusyChanged += OnBusyChanged;
        }

        private void OnBusyChanged(bool value)
        {
            _buttonLoad.interactable = !value;
            _dropdownMenu.interactable = !value;
            _buttonCancel.interactable = value;
        }
        
        private void AddListeners()
        {
            _buttonLoad.onClick.AddListener(_cardLoadController.Load);
            _buttonCancel.onClick.AddListener(_cardLoadController.Cancel);
            _dropdownMenu.onValueChanged.AddListener(_cardLoadController.ChangeLoadType);
        }

        private void FillDropDownMenu()
        {
            var names = Enum.GetNames(typeof(CardLoadType)).ToList();
            _dropdownMenu.AddOptions(names);
        }
    }
}
