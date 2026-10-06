using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.HandActionManager;

public class HandActionManager : IHandActionManager
{
    public void PostSmallBlind(ActiveHand hand)
    {
        if (hand.CurrentStreet != Street.PreFlop)
            throw new InvalidOperationException("Small blind can only be posted in PreFlop street.");

        var seat = hand.Seats.Single(s => s.IsSmallBlind);

        var userBalance = seat.Player.Stack;

        var amount = hand.SmallBlindAmount;

        if (userBalance < amount) throw new InvalidOperationException("Insufficient balance to post small blind.");

        var action = new HandAction
        {
            Street = Street.PreFlop,
            ActorSeatOrder = seat.Order,
            Type = HandActionType.PostSmallBlind,
            Amount = amount,
            IsAllIn = userBalance == amount
        };

        seat.Player.Stack -= amount;

        hand.Actions.Add(action);
    }

    public void PostBigBlind(ActiveHand hand)
    {
        if (hand.CurrentStreet != Street.PreFlop)
            throw new InvalidOperationException("Small blind can only be posted in PreFlop street.");

        var seat = hand.Seats.Single(s => s.IsBigBlind);

        var userBalance = seat.Player.Stack;

        var amount = hand.SmallBlindAmount;

        if (userBalance < amount) throw new InvalidOperationException("Insufficient balance to post big blind.");

        var action = new HandAction
        {
            Street = Street.PreFlop,
            ActorSeatOrder = seat.Order,
            Type = HandActionType.PostBigBlind,
            Amount = amount,
            IsAllIn = userBalance == amount
        };


        hand.Actions.Add(action);
        seat.Player.Stack -= amount;
    }
}