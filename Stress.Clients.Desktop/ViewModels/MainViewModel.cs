using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
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
        public Card LeftStackTopCard { get => _stressGameplay?.LeftStack?.TopCard; }
        public Card RightStackTopCard { get => _stressGameplay?.RightStack?.TopCard; }

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

            _stressGameplay.PlayerWon += StressGameplay_PlayerWon;

            _stressGameplay.Draw();

            PlayerOne = new PlayerViewModel(_stressGameplay.PlayerOne);
            RaisePropertyChanged(nameof(PlayerOne));

            PlayerTwo = new PlayerViewModel(_stressGameplay.PlayerTwo);
            RaisePropertyChanged(nameof(PlayerTwo));

            RaisePropertyChanged(nameof(LeftStackTopCard));
            RaisePropertyChanged(nameof(RightStackTopCard));

            MessengerInstance.Register<PlayCardMessage>(this, OnPlayCardReceived);
            MessengerInstance.Register<StressEventCalledMessage>(this, OnStressEventCalledReceived);
        }

        private void StressGameplay_PlayerWon(object sender, EventArgs e)
        {
            Player winner = (Player)sender;
            MessageBox.Show($"{winner.NickName} won the game!", "We have a winner!", MessageBoxButton.OK, MessageBoxImage.Information);
            ExecuteResetCommand();
        }

        private void OnStressEventCalledReceived(StressEventCalledMessage message)
        {
            _stressGameplay.PlayerCallsStressEvent(message.CallingPlayer);
            RaisePropertyChanged(nameof(PlayerOne));
            RaisePropertyChanged(nameof(PlayerTwo));
            RaisePropertyChanged(nameof(LeftStackTopCard));
            RaisePropertyChanged(nameof(RightStackTopCard));
        }

        private void OnPlayCardReceived(PlayCardMessage message)
        {
            OpenStackOfCards currentStack = _stressGameplay.LeftStack;
            if (!message.PlayOnLeftStack)
                currentStack = _stressGameplay.RightStack;

            _stressGameplay.PlayCardOnStack(message.Player, message.Card, currentStack);
            RaisePropertyChanged(nameof(PlayerOne));
            RaisePropertyChanged(nameof(PlayerTwo));
            RaisePropertyChanged(nameof(LeftStackTopCard));
            RaisePropertyChanged(nameof(RightStackTopCard));
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

            RaisePropertyChanged(nameof(LeftStackTopCard));
            RaisePropertyChanged(nameof(RightStackTopCard));

            MessengerInstance.Send(new DrawMessage());
        }

        private bool CanDrawExecute()
        {
            return true;
        }

    }
}
