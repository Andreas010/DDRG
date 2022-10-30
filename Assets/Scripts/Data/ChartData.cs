using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChartData : IEquatable<ChartData>
{
    public string name;
    public string chartAuthor;
    public string musicAuthor;

    public float bpm;
    public float speed;

    public float difficulty;
    public string description;

    public int keyCount;

    public int preferedStackSize;

    public ChartNote[] notes;

    public bool Equals(ChartData other)
    {
        if(other == null)
            return false;

        if (name != other.name || chartAuthor != other.chartAuthor || musicAuthor != other.musicAuthor || bpm != other.bpm ||
            speed != other.speed || difficulty != other.difficulty || description != other.description ||
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
