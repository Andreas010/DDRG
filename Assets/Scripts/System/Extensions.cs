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

    public static Sprite ToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);
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
