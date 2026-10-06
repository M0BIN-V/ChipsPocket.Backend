using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Extensions;

internal static class ActiveHandExtensions
{
    extension(ActiveHand hand)
    {
        public ActiveHand Clone()
        {
            return new ActiveHand
            {
                TableId = hand.TableId,
                HandId = hand.HandId,
                Seats = hand.Seats.Select(s => new HandSeat
                    {
                        Order = s.Order,
                        Player = new HandPlayer
                        {
                            UserId = s.Player.UserId,
                            Username = s.Player.Username,
                            Stack = s.Player.Stack
                        },
                        IsDealer = s.IsDealer,
                        IsSmallBlind = s.IsSmallBlind,
                        IsBigBlind = s.IsBigBlind,
                        IsFolded = s.IsFolded
                    })
                    .ToList(),
                PotValue = hand.PotValue,
                BigBlindAmount = hand.BigBlindAmount,
                SmallBlindAmount = hand.SmallBlindAmount,
                MinimumRaiseAmount = hand.MinimumRaiseAmount,
                CurrentStreet = hand.CurrentStreet,
                Actions = hand.Actions.Select(a => new HandAction
                {
                    Street = a.Street,
                    ActorSeatOrder = a.ActorSeatOrder,
                    Type = a.Type,
                    Amount = a.Amount,
                    IsAllIn = a.IsAllIn
                }).ToList()
            };
        }

        public void CopyFrom(ActiveHand source)
        {
            hand.Seats = source.Seats.Select(s => new HandSeat
            {
                Order = s.Order,
                Player = new HandPlayer
                {
                    UserId = s.Player.UserId,
                    Username = s.Player.Username,
                    Stack = s.Player.Stack
                },
                IsDealer = s.IsDealer,
                IsSmallBlind = s.IsSmallBlind,
                IsBigBlind = s.IsBigBlind,
                IsFolded = s.IsFolded
            }).ToList();
            hand.PotValue = source.PotValue;
            hand.BigBlindAmount = source.BigBlindAmount;
            hand.SmallBlindAmount = source.SmallBlindAmount;
            hand.MinimumRaiseAmount = source.MinimumRaiseAmount;
            hand.CurrentStreet = source.CurrentStreet;
            hand.Actions = source.Actions.Select(a => new HandAction
            {
                Street = a.Street,
                ActorSeatOrder = a.ActorSeatOrder,
                Type = a.Type,
                Amount = a.Amount,
                IsAllIn = a.IsAllIn
            }).ToList();
        }
    }
}