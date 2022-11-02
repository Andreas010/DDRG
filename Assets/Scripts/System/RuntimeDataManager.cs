using System;
using System.Collections;
using System.Collections.Generic;

public static class RuntimeDataManager
{
    private readonly static Dictionary<string, object> collection = new();

    public static void Reset()
    {
        collection.Clear();
    }

    public static void Set(string name, object data)
    {
        if (Has(name))
            collection[name] = data;
        else
            collection.Add(name, data);
    }

    public static bool Has(string name) => collection.ContainsKey(name);

    public static T Get<T>(string name)
    {
        if (!Has(name))
            throw new ArgumentException();
        return (T)collection[name];
    }
}
