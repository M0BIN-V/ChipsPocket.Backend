using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;
using ChipsPocket.Domain.Services.UserStack;

namespace ChipsPocket.Domain.Services.HandActionManager;

public class HandActionManager(
    ITransactionRepository transactionRepo,
    IHandActionRepository handActionRepo,
    ISeatRepository seatRepo,
    IUserStackService userStackService,
    IHandRepository handRepo) : IHandActionManager
{
    public async Task PostSmallBlindAsync(Guid handId, Guid seatId, int amount)
    {
        var hand = await handRepo.GetAsync(handId);

        if (hand is null) throw new InvalidOperationException("Hand not found.");

        var seat = await seatRepo.GetSeatAsync(hand.TableId, seatId);

        if (seat is null) throw new InvalidOperationException("Seat not found.");

        var userBalance = await userStackService.GetBalanceAsync(hand.TableId, seat.UserId!);

        if (userBalance < amount) throw new InvalidOperationException("Insufficient balance to post small blind.");

        var action = new HandAction
        {
            HandId = handId,
            Street = Street.PreFlop,
            ActorSeatId = seatId,
            Type = HandActionType.PostSmallBlind,
            Amount = amount,
            IsAllIn = userBalance == amount
        };

        handActionRepo.Add(action);

        var transaction = TransactionBuilder.Create(hand.TableId)
            .FromUser(seat.UserId!)
            .ToPot(hand.PotId)
            .WithAmount(amount)
            .Build();

        transactionRepo.Add(transaction);
    }

    public async Task PostBigBlindAsync(Guid handId, Guid seatId, int amount)
    {
        var hand = await handRepo.GetAsync(handId);

        if (hand is null) throw new InvalidOperationException("Hand not found.");

        var seat = await seatRepo.GetSeatAsync(hand.TableId, seatId);

        if (seat is null) throw new InvalidOperationException("Seat not found.");

        var userBalance = await userStackService.GetBalanceAsync(hand.TableId, seat.UserId!);

        if (userBalance < amount) throw new InvalidOperationException("Insufficient balance to post big blind.");

        var action = new HandAction
        {
            HandId = handId,
            Street = Street.PreFlop,
            ActorSeatId = seatId,
            Type = HandActionType.PostBigBlind,
            Amount = amount,
            IsAllIn = userBalance == amount
        };

        handActionRepo.Add(action);

        var transaction = TransactionBuilder.Create(hand.TableId)
            .FromUser(seat.UserId!)
            .ToPot(hand.PotId)
            .WithAmount(amount)
            .Build();

        transactionRepo.Add(transaction);
    }
}