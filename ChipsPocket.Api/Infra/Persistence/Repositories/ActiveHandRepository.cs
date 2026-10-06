using System.Collections.Concurrent;
using ChipsPocket.Api.Infra.Persistence.Extensions;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public sealed class ActiveHandRepository : IActiveHandRepository
{
    private readonly ConcurrentDictionary<Guid, Entry> _hands = new();

    public void AddHand(ActiveHand hand)
    {
        if (!_hands.TryAdd(hand.TableId, new Entry(hand)))
            throw new InvalidOperationException(
                $"Active hand for table '{hand.TableId}' is already active.");
    }

    public ActiveHand? GetHand(Guid tableId)
    {
        if (!_hands.TryGetValue(tableId, out var entry))
            return null;

        entry.Lock.EnterReadLock();

        try
        {
            return entry.Hand.Clone();
        }
        finally
        {
            entry.Lock.ExitReadLock();
        }
    }

    public void SaveHand(ActiveHand hand)
    {
        var entry = GetEntry(hand.TableId);

        entry.Lock.EnterWriteLock();

        try
        {
            entry.Hand.CopyFrom(hand);
        }
        finally
        {
            entry.Lock.ExitWriteLock();
        }
    }

    public bool RemoveHand(Guid tableId)
    {
        if (!_hands.TryRemove(tableId, out var entry))
            return false;

        entry.Lock.Dispose();

        return true;
    }

    private Entry GetEntry(Guid tableId)
    {
        return !_hands.TryGetValue(tableId, out var entry)
            ? throw new KeyNotFoundException(
                $"Active hand for table '{tableId}' was not found.")
            : entry;
    }

    private sealed class Entry(ActiveHand hand)
    {
        public ActiveHand Hand { get; } = hand;

        public ReaderWriterLockSlim Lock { get; } = new();
    }
}