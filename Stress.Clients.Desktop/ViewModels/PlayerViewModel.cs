using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Stress.Game;
using Stress.Game.Cards;
using Stress.Clients.Desktop.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;

namespace Stress.Clients.Desktop.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        
        private Player _player;

        public List<Card> OpenCards { get; set; }
        public string NickName { get => _player.NickName; }
        public int CardsLeft { get => _player.Hand.Cards.Count; }

        private bool _playLeftPile;
        public bool PlayLeftPile
        {
            get => _playLeftPile;
            set => Set(nameof(PlayLeftPile), ref _playLeftPile, value);
        }

        public RelayCommand<Card> PlayCard { get; set; }

        public PlayerViewModel(Player player)
        {
            _player = player;
            InvalidateOpenCards();
            PlayLeftPile = true;

            MessengerInstance.Register<DrawMessage>(this, OnDrawMessage);
            PlayCard = new RelayCommand<Card>(ExecutePlayCard, CanPlayCard);
        }

        private void ExecutePlayCard(Card card)
        {
            MessengerInstance.Send(new PlayCardMessage(_player, card, _playLeftPile));
            InvalidateOpenCards();
        }

        private void InvalidateOpenCards()
        {
            OpenCards = new List<Card>(_player.OpenCards.Where(c => c != null));
            RaisePropertyChanged(nameof(OpenCards));
        }

        private bool CanPlayCard(Card card)
        {
            return true;
        }

        private void OnDrawMessage(DrawMessage message)
        {
            RaisePropertyChanged(nameof(CardsLeft));
        }

    }
}
