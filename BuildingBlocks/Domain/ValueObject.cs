﻿namespace BuildingBlocks.Domain;

public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right)
        => Equals(left, right);
    
    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        => !Equals(left, right);

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;
        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
        => GetEqualityComponents().Aggregate(1, (current, obj) => HashCode.Combine(current, obj));

    protected abstract IEnumerable<object> GetEqualityComponents();
}
