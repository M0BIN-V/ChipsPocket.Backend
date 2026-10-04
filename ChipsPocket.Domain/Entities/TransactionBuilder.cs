namespace ChipsPocket.Domain.Entities;

public sealed class TransactionBuilder
{
    private readonly Transaction _transaction;

    private TransactionBuilder(Guid tableId)
    {
        _transaction = new Transaction
        {
            TableId = tableId
        };
    }

    public static TransactionBuilder Create(Guid tableId)
    {
        if (tableId == Guid.Empty)
            throw new ArgumentException("Table ID cannot be empty.", nameof(tableId));

        return new TransactionBuilder(tableId);
    }

    public TransactionBuilder FromUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        EnsureSourceIsEmpty();

        _transaction.SetFromUser(userId);

        return this;
    }

    public TransactionBuilder FromPot(Guid potId)
    {
        if (potId == Guid.Empty)
            throw new ArgumentException("Pot ID cannot be empty.", nameof(potId));

        EnsureSourceIsEmpty();

        _transaction.SetFromPot(potId);

        return this;
    }

    public TransactionBuilder ToUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        EnsureDestinationIsEmpty();

        _transaction.SetToUser(userId);

        return this;
    }

    public TransactionBuilder ToPot(Guid potId)
    {
        if (potId == Guid.Empty)
            throw new ArgumentException("Pot ID cannot be empty.", nameof(potId));

        EnsureDestinationIsEmpty();

        _transaction.SetToPot(potId);

        return this;
    }

    public TransactionBuilder WithAmount(int value)
    {
        if (value < 1) throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than zero.");

        _transaction.SetValue(value);

        return this;
    }

    public Transaction Build()
    {
        if (!_transaction.HasSource())
            throw new InvalidOperationException(
                "Transaction must have a source.");

        if (!_transaction.HasDestination())
            throw new InvalidOperationException(
                "Transaction must have a destination.");

        if (_transaction.Value == 0)
            throw new InvalidOperationException(
                "Transaction must contain value");

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

    public TransactionBuilder ToShop()
    {
        EnsureDestinationIsEmpty();

        _transaction.SetToShop(true);

        return this;
    }

    public TransactionBuilder FromShop()
    {
        EnsureSourceIsEmpty();

        _transaction.SetFromShop(true);

        return this;
    }
}