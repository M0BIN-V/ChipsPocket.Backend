namespace ChipsPocket.Api.Data.Entities;

public sealed class ChipTransactionBuilder
{
    private readonly ChipTransaction _transaction;

    private ChipTransactionBuilder(Guid tableId)
    {
        _transaction = new ChipTransaction
        {
            TableId = tableId
        };
    }

    public static ChipTransactionBuilder Create(Guid tableId)
    {
        if (tableId == Guid.Empty)
            throw new ArgumentException("Table ID cannot be empty.", nameof(tableId));

        return new ChipTransactionBuilder(tableId);
    }

    public ChipTransactionBuilder FromUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        EnsureSourceIsEmpty();

        _transaction.SetFromUser(userId);

        return this;
    }

    public ChipTransactionBuilder FromPot(Guid potId)
    {
        if (potId == Guid.Empty)
            throw new ArgumentException("Pot ID cannot be empty.", nameof(potId));

        EnsureSourceIsEmpty();

        _transaction.SetFromPot(potId);

        return this;
    }

    public ChipTransactionBuilder ToUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        EnsureDestinationIsEmpty();

        _transaction.SetToUser(userId);

        return this;
    }

    public ChipTransactionBuilder ToPot(Guid potId)
    {
        if (potId == Guid.Empty)
            throw new ArgumentException("Pot ID cannot be empty.", nameof(potId));

        EnsureDestinationIsEmpty();

        _transaction.SetToPot(potId);

        return this;
    }

    public ChipTransactionBuilder AddChip(Guid chipId, int count)
    {
        if (chipId == Guid.Empty)
            throw new ArgumentException("Chip ID cannot be empty.", nameof(chipId));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(count),
                count,
                "Chip count must be greater than zero.");

        var existing = _transaction.Chips
            .FirstOrDefault(x => x.ChipId == chipId);

        if (existing is null)
        {
            _transaction.Chips.Add(new TransactionChip
            {
                ChipId = chipId,
                ChipCount = count
            });
        }
        else
        {
            existing.ChipCount += count;
        }

        return this;
    }

    public ChipTransaction Build()
    {
        if (!_transaction.HasSource())
            throw new InvalidOperationException(
                "Transaction must have a source.");

        if (!_transaction.HasDestination())
            throw new InvalidOperationException(
                "Transaction must have a destination.");

        if (_transaction.Chips.Count == 0)
            throw new InvalidOperationException(
                "Transaction must contain at least one chip.");

        return _transaction;
    }

    private void EnsureSourceIsEmpty()
    {
        if (_transaction.HasSource())
            throw new InvalidOperationException(
                "Transaction already has a source.");
    }

    private void EnsureDestinationIsEmpty()
    {
        if (_transaction.HasDestination())
            throw new InvalidOperationException(
                "Transaction already has a destination.");
    }
}