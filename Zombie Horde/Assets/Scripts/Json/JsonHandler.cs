using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class JsonHandler<T> : MonoBehaviour
{
    /// <summary>
    /// A list of all the possible entries based on the generic type
    /// </summary>
    public List<T> entries = new List<T>();

    /// <summary>
    /// The file name of the json file
    /// </summary>
    /// <returns>The file name of the json file</returns>
    protected abstract string GetFileName();

    /// <summary>
    /// The path where the file can be found
    /// </summary>
    /// <returns>The path where the file can be found</returns>
    protected abstract string GetPath();

    public virtual void Start()
    {
        Load();
    }

    /// <summary>
    /// Handles saving to the json file
    /// </summary>
    public void Save()
    {
        //Creates a new array with generic typing with the size of the list
        var array = new T[entries.Count];
        
        //Fills the array with the list entries
        for (var index = 0; index < array.Length; index++)
            array[index] = entries[index];
        
        //Converts the array to json
        var toJson = JsonHelper.ToJson(array, true);
        
        //Writes the json to the file using the correct path and file name
        var sr = File.CreateText($"{GetPath()}{GetFileName()}");
        sr.WriteLine (toJson);
        sr.Close();
    }

    /// <summary>
    /// Handles loading a json file
    /// </summary>
    public void Load()
    {
        if (!File.Exists($"{GetPath()}{GetFileName()}")) return;
        
        var jsonString = "";
        //Read the json file
        //And closes the stream reader
        var reader = new StreamReader($"{GetPath()}{GetFileName()}");
        jsonString = reader.ReadToEnd();
        reader.Close();
        
        //Converts the json to an array
        //And fills the list with the array entries
        var array = JsonHelper.FromJson<T>(jsonString);
        foreach (var entry in array)
            entries.Add(entry);
    }
}