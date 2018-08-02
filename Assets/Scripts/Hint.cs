using UnityEngine;
using System.Collections.Generic;

public class Hint  {

    public int Index;
    public int Collum;
    public List<int> ListDes;
    public List<Vector3> ListPos;
    public List<int> ListCard;

    public Hint()
    {
        ListDes = new List<int>();
        ListPos = new List<Vector3>();
        ListCard = new List<int>();
    }
}
