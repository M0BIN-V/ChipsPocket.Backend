using ChipsPocket.Api.Common.Dtos;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Common.Extensions;

public static class ActiveHandExtensions
{
    public static ViewActiveHandDto ToDto(this ActiveHand hand)
    {
        return new ViewActiveHandDto
        {
            TableId = hand.TableId,
            HandId = hand.HandId,
            Seats = hand.Seats.Select(s => new ViewHandSeatDto
            {
                Order = s.Order,
                Player = new ViewHandPlayerDto
                {
                    UserId = s.Player.UserId,
                    Username = s.Player.Username,
                    Stack = s.Player.Stack
                },
                IsDealer = s.IsDealer,
                IsSmallBlind = s.IsSmallBlind,
                IsBigBlind = s.IsBigBlind,
                IsFolded = s.IsFolded
            }).ToList(),
            PotValue = hand.PotValue,
            BigBlindAmount = hand.BigBlindAmount,
            SmallBlindAmount = hand.SmallBlindAmount,
            MinimumRaiseAmount = hand.MinimumRaiseAmount,
            CurrentStreet = hand.CurrentStreet,
            Actions = hand.Actions.Select(a => new ViewHandActionDto
            {
                Street = a.Street,
                ActorSeatOrder = a.ActorSeatOrder,
                Type = a.Type,
                Amount = a.Amount,
                IsAllIn = a.IsAllIn
            }).ToList()
        };
    }
}