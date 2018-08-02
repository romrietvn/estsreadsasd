using UnityEngine;
using System.Collections.Generic;
using System;
[Serializable]
public class Undo  {

    public int Index;
    public int Collum;
    public int CollumDes;
    public List<int> ListValue;
    public  List<int> ListIndex;
    public bool Isflip;
    public bool IsGetCollum;
    public bool IsflipCollumdes;
    public int Lenght;
    public int IndexDes;

    public Undo(int index, int collum, int collumdes, List<int> listindex, List<int> listvalue, bool isgetcollum = false,bool isflip = false, int lenght = 0, bool isflipCollumdes = false)
    {
        ListIndex = new List<int>();
        ListValue = new List<int>();
        Index = index;
        Collum = collum;
        CollumDes = collumdes;
        ListIndex.AddRange(listindex);
        ListValue.AddRange(listvalue);
        Isflip = isflip;
        IsGetCollum = isgetcollum;
        Lenght = lenght;
        IsflipCollumdes = isflipCollumdes;
    }

    public Undo(int index, int collum, int collumdes, int indexdeck, int value, bool isgetcollum = false, bool isflip = false, int lenght = 0, bool isflipCollumdes = false)
    {
        ListIndex = new List<int>();
        ListValue = new List<int>();
        Index = index;
        Collum = collum;
        CollumDes = collumdes;
        ListIndex = new List<int>();
        ListValue.Add(value);
        ListIndex.Add(value);
        Isflip = isflip;
        IsGetCollum = isgetcollum;
        Lenght = lenght;
        IsflipCollumdes = isflipCollumdes;
    }
}
