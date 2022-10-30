using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static CustomIntEnumerator GetEnumerator(this Range range)
    {
        return new(range);
    }

    public static CustomIntEnumerator GetEnumerator(this int number)
    {
        return new(new Range(0, number));
    }
}

public ref struct CustomIntEnumerator
{
    private int current;
    private readonly int end;

    public CustomIntEnumerator(Range range)
    {
        if (range.End.IsFromEnd)
            throw new NotSupportedException();

        current = range.Start.Value - 1;
        end = range.End.Value;
    }

    public int Current => current;

    public bool MoveNext()
    {
        current++;
        return current <= end;
    }
}
