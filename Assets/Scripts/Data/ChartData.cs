using System;

public sealed class ChartData : IEquatable<ChartData>
{
    public string name;

    public float bpm;
    public float speed;

    public float difficulty;

    public int keyCount;

    public int preferedStackSize;

    public ChartNote[] notes;

    public bool Equals(ChartData other)
    {
        if(other == null)
            return false;

        if (name != other.name || bpm != other.bpm ||
            speed != other.speed || difficulty != other.difficulty ||
            keyCount != other.keyCount || preferedStackSize != other.preferedStackSize)
            return false;

        if (notes.Length != other.notes.Length)
            return false;

        for(int i = 0; i < notes.Length; i++)
        {
            if (!notes[i].Equals(other.notes[i]))
                return false;
        }

        return true;
    }
}
