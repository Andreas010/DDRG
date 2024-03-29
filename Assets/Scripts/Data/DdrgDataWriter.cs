using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class DdrgDataWriter
{
    public const uint CHART_SERIALISER_VERSION = 1;
    public const ushort CHART_SERIALISER_PREFIX = 0xDDf7;

    public static byte[] SerialiseChart(ChartData data)
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream, Encoding.UTF8);

        writer.Write(CHART_SERIALISER_PREFIX);
        writer.Write(CHART_SERIALISER_VERSION);

        if (data.name is null)
            throw new InvalidChartException("The name of the chart is null");
        writer.Write(data.name);

        if (data.bpm <= 0)
            throw new InvalidChartException("The BPM of the chart is below or equals to zero");
        writer.Write((ushort)(data.bpm * 10f));

        if(data.speed <= 0)
            throw new InvalidChartException("The speed of the chart is below or equals to zero");
        writer.Write((ushort)(data.speed * 10f));

        if (data.difficulty <= 0)
            throw new InvalidChartException("The difficulty of the chart is below or equals to zero");
        writer.Write(data.difficulty);

        if (data.keyCount is <= 0 or >= 10)
            throw new InvalidChartException("The keyCount of the chart is outside the allowed Range");
        writer.Write((byte)data.keyCount);

        if (data.preferedStackSize <= 0)
            throw new InvalidChartException("The preferedStackSize of the chart is below zero");
        writer.Write(data.preferedStackSize);

        writer.Write(data.notes.Length);

        for(int i = 0; i < data.notes.Length; i++)
        {
            ChartNote cNote = data.notes[i];

            writer.Write((byte)cNote.type);

            writer.Write(cNote.time);

            writer.Write((byte)cNote.row);
        }

        byte[] buffer = new byte[memoryStream.Length];
        memoryStream.Position = 0;
        memoryStream.Read(buffer, 0, buffer.Length);
        return buffer;
    }

    public static ChartData DeserialiseChart(byte[] input)
    {
        using MemoryStream memoryStream = new(input);
        using BinaryReader reader = new(memoryStream, Encoding.UTF8);

        ChartData data = new();

        try
        {
            ushort prefix = reader.ReadUInt16();
            if (prefix != CHART_SERIALISER_PREFIX)
                throw new InvalidChartException("The chart prefixes don't match!");

            uint chartVersion = reader.ReadUInt32();

            data.name = reader.ReadString();

            data.bpm = reader.ReadUInt16() / 10f;
            data.speed = reader.ReadUInt16() / 10f;

            data.difficulty = reader.ReadSingle();

            data.keyCount = reader.ReadByte();
            data.preferedStackSize = reader.ReadInt32();

            ChartNote[] notes = new ChartNote[reader.ReadInt32()];
            for(int i = 0; i < notes.Length; i++)
            {
                notes[i] = new()
                {
                    type = (NoteType)reader.ReadByte(),
                    time = reader.ReadSingle(),
                    row = reader.ReadByte()
                };
            }
            data.notes = notes;
        }
        catch
        {
            return null;
        }

        return data;
    }
}


[Serializable]
public class InvalidChartException : Exception
{
    public InvalidChartException() { }
    public InvalidChartException(string message) : base(message) { }
    public InvalidChartException(string message, Exception inner) : base(message, inner) { }
    protected InvalidChartException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}