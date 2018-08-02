using UnityEngine;
using System.Collections.Generic;

public class Rule
{

    public bool CheckInCollum(List<int>[] Collums, int index, int collum)
    {

        int count = Collums[collum].Count;
        if (count > 1 && index < count - 1)
        {
            for (int i = index; i < count - 1; i++)
            {
                float temp = (Collums[collum][i] % 4) - (Collums[collum][i + 1] % 4);
                if ((temp == 0) && ((Collums[collum][i] / 4 - Collums[collum][i + 1] / 4) == 1))
                {

                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        else
            return true;

    }

    public void CheckAllCollum(List<int>[] collums, int[] lastvaluecollums,
        int value, List<int> result, int collum, List<int> currentdrag)
    {
        //Debug.Log("At");
        //for (int i = GameData.NUMBER_COLLUM; i < GameData.TOTAL_COLLUM; i++)
        //{
        //    if (collums[i].Count == 0)
        //    {
        //        result.Add(i);

        //    }
        //    //Debug.Log(result.Count);

        //}

        //int totalcolum = GameData.TOTAL_COLLUM;
        //for (int i = GameData.NUMBER_COLLUM; i < totalcolum; i++)
        //{
        //    int _count = collums[i].Count;
        //    //float temp = (lastvaluecollums[i] % 4) / 2 - (value % 4) / 2;
        //    int _currentdrag = currentdrag.Count;
        //    if (((value / 4 - lastvaluecollums[i] / 4) == 1) && (value % 4 == lastvaluecollums[i] % 4) && (_count > 0) && (_currentdrag == 1))
        //    {
        //        result.Add(i);
        //        // Debug.Log(result.Count);
        //        //Debug.Log(result[0]);
        //    }

        //}

        int count = GameData.NUMBER_COLLUM;
        for (int i = 0; i < count; i++)
        {

            int _count = collums[i].Count;
            if (_count > 0)
            {
                float temp = ((lastvaluecollums[i] % 4)) - (value % 4);
                if (((lastvaluecollums[i] / 4 - value / 4) == 1) && temp == 0)
                {
                    result.Add(i);
                    //Debug.Log(result.Count);
                    //Debug.Log(result[0]);
                }
            }
            //else if (_count == 0)
            //{

            //    result.Add(i);
            //    //Debug.Log("Quan K");
            //}


        }
        int resultcount = result.Count;
        if (resultcount >= 2)
        {
            for (int i = 1; i < resultcount; i++)
            {
                int temp = result[0];
                if (NumberCardCanMove(collums[temp]) < NumberCardCanMove(collums[result[i]]))
                {
                    Swap(result, 0, i);
                }
            }
        }
        int CountZero = 0;
        for (int i = 0; i < count; i++)
        {
            int _count = collums[i].Count;
            if (_count != 0)
            {
                if (((lastvaluecollums[i] / 4 - value / 4) == 1) && !(result.Contains(i)))
                {
                    result.Add(i);
                    //Debug.Log(result.Count);
                    //Debug.Log(result[0]);
                }
            }
            else
            {

                result.Add(i);
                CountZero++;
                // Debug.Log("Quan K");
            }
        }
        if(result.Count - CountZero ==1 && CountZero >=8)
        {
            for(int i = 0; i < GameData.NUMBER_COLLUM; i++)
            {
                if(collums[i].Count == 0)
                {
                    result.Remove(i);
                }
            }
        }
       
    }



    public bool CheckPosition(Vector2 position, Vector2 des, bool isDes)
    {
        float temp = GameData.CARD_WIDTH / 2 + GameData.CARD_PADDING_WIDTH;
        float range = Mathf.Abs(position.x - des.x);

        // Debug.Log(des.y);
        //Debug.Log(bound);
        //Debug.Log(position.y);
        //Debug.Log(des.y);
        if (isDes)
        {

            float rangeY = position.y + GameData.CARD_HEIGHT;
            if (range <= temp)
            {
                {

                    return true;

                }

            }
            else
            {
                // Debug.Log("xxx");
                return false;

            }
        }
        else
        {
            float BoundY = des.y + GameData.CARD_HEIGHT;
            if (range <= temp)
            {
                //Debug.Log("xxxx");
                return true;
            }
            else
                return false;
        }

    }

    public bool CheckHintAllCollum(List<int>[] collums, int[] lastvaluecollums, List<int>[] indexdeck,
        int value, int indexarray, int collum, int index)
    {


        int totalcolum = GameData.TOTAL_COLLUM;
        int count = GameData.NUMBER_COLLUM;
        for (int i = 0; i < count; i++)
        {
            if (i != collum)
            {
                int _count = collums[i].Count;
                if (_count > 0)
                {
                    float temp = ((lastvaluecollums[i] % 4)) - ((value % 4));
                    if (_count >= 1)
                    {

                        if ((lastvaluecollums[i] / 4 - value / 4) == 1 && temp == 0)
                        {
                            if (collums[collum].Count == 1)
                            {
                                //Debug.LogError("count = 1" + " " + i);
                                return true;
                            }
                            else if (index >= 1)
                            {
                                int _countCollumdes = CheckHintCollum(collums[i], indexdeck[i], i);

                                int _countNumberCard = collums[collum].Count - index;
                                //Debug.LogError(_countCollumdes + " " + i + "_countNumberCard " + _countNumberCard + "collum " + collum);
                                //Debug.LogWarning(_countNumberCard + _countCollumdes);
                                if (_countCollumdes + _countNumberCard == 13)
                                {
                                    //  Debug.LogError("13" + "i " + i + " collum" + collum + "_countNumberCard " + _countNumberCard);
                                    return true;
                                }
                                else if ((lastvaluecollums[i] != collums[collum][index - 1]))
                                {
                                    //Debug.LogError("count >= 1" + " " + i);

                                    return true;
                                }
                                else
                                {
                                    if (!(Table.m_Instance.GetStateCard(indexdeck[collum][index - 1])))
                                        return true;
                                    //else
                                    //{
                                    //    if (CheckHintCollum(collums[i], indexdeck[i], i) == 12)
                                    //        return true;
                                    //}
                                }


                            }
                            else if (index == 0)
                            {
                                // Debug.LogError("count = 0" + " " + i);
                                return true;
                            }
                            //Debug.LogError(false);

                        }
                        else if ((lastvaluecollums[i] / 4 - value / 4) == 1)
                        {
                            if (index >= 1)
                            {

                                if (!Table.m_Instance.GetStateCard(indexdeck[collum][index - 1]) ||
                                    (collums[collum][index - 1]) / 4 != lastvaluecollums[i] / 4)
                                {
                                    return true;
                                }

                            }

                        }
                    }
                    else
                    {
                        if ((lastvaluecollums[i] / 4 - value / 4) == 1 && temp == 0)
                        {
                            return true;
                        }
                        else if ((lastvaluecollums[i] / 4 - value / 4) == 1)
                        {
                            if (index >= 1)
                            {
                                if (!Table.m_Instance.GetStateCard(indexdeck[collum][index - 1]))
                                {
                                    return true;
                                }
                            }

                        }
                    }
                }


            }
        }

        for (int i = 0; i < count; i++)
        {

            int _count = collums[i].Count;
            if (_count == 0)
                return true;
        }
        return false;


    }

    public void ComputeHintDes(List<int>[] collums, List<int>[] indexdeck, int[] lastvaluecollums,
        int value, int indexarray, int collum, int index, Vector2[] pos, Hint hintdes)
    {
        hintdes.Index = index;
        hintdes.Collum = collum;
        int count = GameData.NUMBER_COLLUM;
        for (int i = 0; i < count; i++)
        {
            int _count = collums[i].Count;
            if (i != collum)
            {
                if (_count > 0)
                {
                    float temp = ((lastvaluecollums[i] % 4)) - ((value % 4));
                    if (_count >= 1)
                    {

                        if ((lastvaluecollums[i] / 4 - value / 4) == 1 && temp == 0)
                        {

                            if (collums[collum].Count == 1)
                            {
                                hintdes.ListDes.Add(i);
                                Vector3 _temp = pos[i];
                                _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                _temp.z = -(collums[collum].Count);
                                hintdes.ListPos.Add(_temp);
                            }
                            else if (index >= 1)
                            {
                                int _countCollumdes = CheckHintCollum(collums[i], indexdeck[i], i);
                                //Debug.LogError(_countCollumdes);
                                int _countNumberCard = collums[collum].Count - index;
                                //Debug.LogWarning(_countNumberCard + " " + collum + "value" + value);
                                if (_countCollumdes + _countNumberCard == 13)
                                {
                                    hintdes.ListDes.Add(i);
                                    Vector3 _temp = pos[i];
                                    _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                    _temp.z = -(collums[collum].Count);
                                    hintdes.ListPos.Add(_temp);
                                }
                                else if (lastvaluecollums[i] != collums[collum][index - 1])
                                    {
                                        hintdes.ListDes.Add(i);
                                        Vector3 _temp = pos[i];
                                        _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                        _temp.z = -(collums[collum].Count);
                                        hintdes.ListPos.Add(_temp);
                                    }
                                    else
                                    {
                                        if (!(Table.m_Instance.GetStateCard(indexdeck[collum][index - 1])))
                                        {
                                            hintdes.ListDes.Add(i);
                                            Vector3 _temp = pos[i];
                                            _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                            _temp.z = -(collums[collum].Count);
                                            hintdes.ListPos.Add(_temp);
                                        }
                                        else
                                        {
                                            if (CheckHintCollum(collums[i], indexdeck[i], i) == 12)
                                            {
                                                hintdes.ListDes.Add(i);
                                                Vector3 _temp = pos[i];
                                                _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                                _temp.z = -(collums[collum].Count);
                                                hintdes.ListPos.Add(_temp);
                                            }
                                        }
                                    }
                            }
                            else if (index == 0)
                            {
                                hintdes.ListDes.Add(i);
                                Vector3 _temp = pos[i];
                                _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                _temp.z = -(collums[collum].Count);
                                hintdes.ListPos.Add(_temp);
                            }

                        }
                        else if ((lastvaluecollums[i] / 4 - value / 4) == 1)
                        {
                            if (index >= 1)
                            {

                                if (!Table.m_Instance.GetStateCard(indexdeck[collum][index - 1]) ||
                                    (collums[collum][index - 1]) / 4 != lastvaluecollums[i] / 4)
                                {
                                    hintdes.ListDes.Add(i);
                                    Vector3 _temp = pos[i];
                                    _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                    _temp.z = -(collums[collum].Count);
                                    hintdes.ListPos.Add(_temp);
                                }

                            }

                        }

                    }
                    else
                    {
                        if ((lastvaluecollums[i] / 4 - value / 4) == 1 && temp == 0)
                        {
                            {
                                hintdes.ListDes.Add(i);
                                Vector3 _temp = pos[i];
                                _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                _temp.z = -(collums[collum].Count);
                                hintdes.ListPos.Add(_temp);
                            }
                        }
                        else if ((lastvaluecollums[i] / 4 - value / 4) == 1)
                        {
                            if (index >= 1)
                            {
                                if (!Table.m_Instance.GetStateCard(indexdeck[collum][index - 1]))
                                {
                                    hintdes.ListDes.Add(i);
                                    Vector3 _temp = pos[i];
                                    _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                                    _temp.z = -(collums[collum].Count);
                                    hintdes.ListPos.Add(_temp);
                                }
                            }

                        }
                    }
                }
                //else if (_count == 0)
                //{
                //    //if (index >= 1)
                //    //{
                //    //    if (!Table.m_Instance.GetStateCard(indexdeck[collum][index - 1]))
                //    //    {
                //    hintdes.ListDes.Add(i);
                //    Vector3 _temp = pos[i];
                //    _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                //    _temp.z = -(collums[collum].Count);
                //    hintdes.ListPos.Add(_temp);
                //    //    }
                //    //}

                //}

            }
        }

        for (int i = 0; i < count; i++)
        {

            int _count = collums[i].Count;
            if (_count == 0)
            {
                //if (index >= 1)
                //{
                //    if (!Table.m_Instance.GetStateCard(indexdeck[collum][index - 1]))
                //    {
                hintdes.ListDes.Add(i);
                Vector3 _temp = pos[i];
                _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                _temp.z = -(collums[collum].Count);
                hintdes.ListPos.Add(_temp);
                //    }
                //}

            }

        }
    }

    public bool CheckHintDrawCard(List<int> listcarddraw, List<int>[] collums, int[] lastvaluecollums)
    {
        int numbercarddraw = listcarddraw.Count - 1;
        // Debug.Log(numbercarddraw);
        if (numbercarddraw >= 0)
        {
            int value = listcarddraw[numbercarddraw];
            if (value / 4 == 0)
            {
                //Debug.Log("At");
                for (int i = GameData.NUMBER_COLLUM; i < GameData.TOTAL_COLLUM; i++)
                {
                    if (collums[i].Count == 0)
                    {
                        return true;

                    }


                }

            }
            else
            {

                int totalcolum = GameData.TOTAL_COLLUM;
                for (int i = GameData.NUMBER_COLLUM; i < totalcolum; i++)
                {
                    int _count = collums[i].Count;
                    //float temp = (lastvaluecollums[i] % 4) / 2 - (value % 4) / 2;
                    // Debug.Log(numbercarddrag + "number card drag" + " collum" + collum + "value" + value/4);
                    if (((value / 4 - lastvaluecollums[i] / 4) == 1) && (value % 4 == lastvaluecollums[i] % 4) && (_count > 0))
                    {
                        return true;
                    }

                }
            }
            int count = GameData.NUMBER_COLLUM;
            for (int i = 0; i < count; i++)
            {

                int _count = collums[i].Count;
                if ((value / 4 == 12) && (_count == 0))
                {

                    return true;
                }
                else if (_count > 0)
                {
                    float temp = ((lastvaluecollums[i] % 4) / 2) - ((value % 4) / 2);
                    if ((temp != 0) && ((lastvaluecollums[i] / 4 - value / 4) == 1))
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }

    public void ComputeHintDrawCard(List<int> listcarddraw, List<int>[] collums, Vector2[] pos, int[] lastvaluecollums, Hint hint)
    {
        int numbercarddraw = listcarddraw.Count - 1;
        int value = listcarddraw[numbercarddraw];
        hint.ListCard.Add(value);
        //hint.Index = index;
        //hint.Collum = collum;
        if (value / 4 == 0)
        {
            //Debug.Log("At");
            for (int i = GameData.NUMBER_COLLUM; i < GameData.TOTAL_COLLUM; i++)
            {
                if (collums[i].Count == 0)
                {
                    hint.ListDes.Add(i);
                    Vector3 temp = pos[i];
                    //temp.z = -(collums[collum].Count - 1);
                    hint.ListPos.Add(temp);

                }


            }

        }
        else
        {

            int totalcolum = GameData.TOTAL_COLLUM;
            for (int i = GameData.NUMBER_COLLUM; i < totalcolum; i++)
            {
                int _count = collums[i].Count;
                //float temp = (lastvaluecollums[i] % 4) / 2 - (value % 4) / 2;
                // Debug.Log(numbercarddrag + "number card drag" + " collum" + collum + "value" + value/4);
                if (((value / 4 - lastvaluecollums[i] / 4) == 1) && (value % 4 == lastvaluecollums[i] % 4) && (_count > 0))
                {
                    hint.ListDes.Add(i);
                    Vector3 temp = pos[i];
                    temp.z = -(collums[i].Count);
                    hint.ListPos.Add(temp);
                }

            }
        }
        int count = GameData.NUMBER_COLLUM;
        for (int i = 0; i < count; i++)
        {

            int _count = collums[i].Count;
            if ((value / 4 == 12) && (_count == 0))
            {

                hint.ListDes.Add(i);
                Vector3 temp = pos[i];
                temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                temp.z = -(collums[i].Count);
                hint.ListPos.Add(temp);
            }
            else if (_count > 0)
            {
                float temp = ((lastvaluecollums[i] % 4) / 2) - ((value % 4) / 2);
                if ((temp != 0) && ((lastvaluecollums[i] / 4 - value / 4) == 1))
                {
                    hint.ListDes.Add(i);
                    Vector3 _temp = pos[i];
                    _temp.y -= _count * GameData.CARD_RATIO_HEIGHT;
                    _temp.z = -(collums[i].Count);
                    hint.ListPos.Add(_temp);
                }
            }

        }
        //Debug.Log(hint.ListDes.Count);
    }

    public bool CheckHintDrawCard(List<int> listcarddraw, List<int>[] collums, int[] lastvaluecollums, int index)
    {
        int numbercarddraw = listcarddraw.Count - 1;
        // Debug.Log(numbercarddraw);
        if (numbercarddraw >= 0)
        {
            int value = listcarddraw[index];
            if (value / 4 == 0)
            {
                //Debug.Log("At");
                for (int i = GameData.NUMBER_COLLUM; i < GameData.TOTAL_COLLUM; i++)
                {
                    if (collums[i].Count == 0)
                    {
                        return true;

                    }


                }

            }
            else
            {

                int totalcolum = GameData.TOTAL_COLLUM;
                for (int i = GameData.NUMBER_COLLUM; i < totalcolum; i++)
                {
                    int _count = collums[i].Count;
                    //float temp = (lastvaluecollums[i] % 4) / 2 - (value % 4) / 2;
                    // Debug.Log(numbercarddrag + "number card drag" + " collum" + collum + "value" + value/4);
                    if (((value / 4 - lastvaluecollums[i] / 4) == 1) && (value % 4 == lastvaluecollums[i] % 4) && (_count > 0))
                    {
                        return true;
                    }

                }
            }
            int count = GameData.NUMBER_COLLUM;
            for (int i = 0; i < count; i++)
            {

                int _count = collums[i].Count;
                if ((value / 4 == 12) && (_count == 0))
                {

                    return true;
                }
                else if (_count > 0)
                {
                    float temp = ((lastvaluecollums[i] % 4) / 2) - ((value % 4) / 2);
                    if ((temp != 0) && ((lastvaluecollums[i] / 4 - value / 4) == 1))
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }

    private void Swap(List<int> list, int a, int b)
    {
        int temp = list[a];
        list[a] = list[b];
        list[b] = temp;
    }

    private int NumberCardCanMove(List<int> Collums)
    {
        int count = Collums.Count - 1;
        for (int i = 0; i < count; i++)
        {
            //Debug.LogError(Collums[count - i - 1] / 4 +" " + (Collums[count -i ] / 4));
            float temp = (Collums[count - i - 1] % 4) - (Collums[count - i] % 4);
            if ((temp == 0) && ((Collums[count - i - 1] / 4 - Collums[count - i] / 4) == 1))
            {

            }
            else
            {
                //Debug.LogError(i);
                return i;
            }
        }
        //Debug.LogError(count);
        return count;
    }

    public int CheckInCollum(int cardflip, List<int> collum)
    {
        int count = collum.Count;
        int _count = 0;
        for (int i = count - 1; i > cardflip; i--)
        {
            if ((collum[i - 1] / 4 - collum[i] / 4 == 1) && (collum[i - 1] % 4 == collum[i] % 4))
            {
                _count += 1;
            }
            else
            {
                return i;
            }
        }
        if (_count == 0)
        {
            return collum.Count - 1;
        }
        else
        {
            return collum.Count - _count - 1;
        }
    }

    public int GetLenghtInCollum(int cardflip, List<int> collum)
    {
        int count = collum.Count;
        int _count = 1;
        for (int i = count - 1; i > cardflip; i--)
        {
            if ((collum[i - 1] / 4 - collum[i] / 4 == 1) && (collum[i - 1] % 4 == collum[i] % 4))
            {
                _count += 1;
            }
            else
            {
                return _count;
            }
        }
        return _count;
        //if (_count == 0)
        //{
        //    return collum.Count - 1;
        //}
        //else
        //{
        //    return collum.Count - _count - 1;
        //}
    }

    public int CheckHintCollum(List<int> collums, List<int> indexdeck, int collum)
    {
        int count = collums.Count;
        int _count = 1;
        for (int i = count - 1; i >= 1; i--)
        {
            if ((collums[i - 1] / 4 - collums[i] / 4 == 1) && (collums[i - 1] % 4 == collums[i] % 4) &&
                (Table.m_Instance.GetStateCard(indexdeck[i - 1])))
            {
                _count += 1;
            }
            else
            {
                return _count;
            }
        }
        //Debug.LogWarning(_count + " " + collum + " count"  +count);
        return _count;
    }
}


