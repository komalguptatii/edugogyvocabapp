using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class JsonArrayHelper
{
    // public static T[] FromJson<T>(string json)
    // {
    //     Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
    //     return wrapper.Items;
    // }

    // public static string ToJson<T>(T[] array)
    // {
    //     Wrapper<T> wrapper = new Wrapper<T>();
    //     wrapper.Items = array;
    //     return JsonUtility.ToJson(wrapper);
    // }

    // public static string ToJson<T>(T[] array, bool prettyPrint)
    // {
    //     Wrapper<T> wrapper = new Wrapper<T>();
    //     wrapper.Items = array;
    //     return JsonUtility.ToJson(wrapper, prettyPrint);
    // }

public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
        return wrapper.array;
    }
 
 [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
   
    
}