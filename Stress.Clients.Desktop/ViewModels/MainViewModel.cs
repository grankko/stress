using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stress.Clients.Desktop.Messages;
using Stress.Game;
using Stress.Game.Cards;

namespace Stress.Clients.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private Gameplay _stressGameplay;

        public PlayerViewModel PlayerOne { get; set; }
        public PlayerViewModel PlayerTwo { get; set; }
        public Card LeftPileTopCard { get => _stressGameplay?.LeftPile?.TopCard; }
        public Card RightPileTopCard { get => _stressGameplay?.RightPile?.TopCard; }

        public RelayCommand ResetCommand { get; set; }
        public RelayCommand DrawCommand { get; set; }

        public MainViewModel()
        {
            Initialize();

            ResetCommand = new RelayCommand(ExecuteResetCommand, CanResetExecute);
            DrawCommand = new RelayCommand(ExecuteDrawCommand, CanDrawExecute);
        }

        private void Initialize()
        {
            _stressGameplay = new Gameplay();
            _stressGameplay.AddPlayer("Anders");
            _stressGameplay.AddPlayer("Edith");

            _stressGameplay.Draw();

            PlayerOne = new PlayerViewModel(_stressGameplay.PlayerOne);
            RaisePropertyChanged(nameof(PlayerOne));

            PlayerTwo = new PlayerViewModel(_stressGameplay.PlayerTwo);
            RaisePropertyChanged(nameof(PlayerTwo));

            RaisePropertyChanged(nameof(LeftPileTopCard));
            RaisePropertyChanged(nameof(RightPileTopCard));

            MessengerInstance.Register<PlayCardMessage>(this, OnPlayCardReceived);
        }

        private void OnPlayCardReceived(PlayCardMessage message)
        {
            OpenPileOfCards currentPile = _stressGameplay.LeftPile;
            if (!message.OnLeftPile)
                currentPile = _stressGameplay.RightPile;

            _stressGameplay.PlayCardOnPile(message.Player, message.Card, currentPile);
            RaisePropertyChanged(nameof(PlayerOne));
            RaisePropertyChanged(nameof(PlayerTwo));
            RaisePropertyChanged(nameof(LeftPileTopCard));
            RaisePropertyChanged(nameof(RightPileTopCard));
        }

        private void ExecuteResetCommand()
        {
            Initialize();
        }

        private bool CanResetExecute()
        {
            return true;
        }

        private void ExecuteDrawCommand()
        {
            _stressGameplay.Draw();

            RaisePropertyChanged(nameof(LeftPileTopCard));
            RaisePropertyChanged(nameof(RightPileTopCard));

            MessengerInstance.Send(new DrawMessage());
        }

        private bool CanDrawExecute()
        {
            return true;
        }

    }
}
