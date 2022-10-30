using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ChartNote : IEquatable<ChartNote>
{
    public NoteType type;
    public float time;
    public int row;

    public bool Equals(ChartNote other)
    {
        if (other == null)
            return false;

        return type == other.type && time == other.time && row == other.row;
    }
}

public enum NoteType
{
    Normal,
    Hold,
    HoldEnd,
    Damage,
    Poison,
    Death,
}