using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {
    public List<House> houses = new List<House>();
    public float GetMinionsPerMinute(int direction = 0, Minion.Type t = Minion.Type.All) {

        float i = 0;
        List<House> h = houses;

        if (direction < 0)
        {
            h = h.FindAll(e => e.directionIsToLeft);
        }
        else if (direction > 0)
        {
            h = h.FindAll(e => !e.directionIsToLeft);
        }

        switch (t)
        {
            case Minion.Type.Melee:
                h = h.FindAll(e => e.type == Minion.Type.Melee);
                break;
            case Minion.Type.Ranged:
                h = h.FindAll(e => e.type == Minion.Type.Ranged);
                break;
            case Minion.Type.Shield:
                h = h.FindAll(e => e.type == Minion.Type.Shield);
                break;
        }
        h.ForEach(e => i += e.MinionsPerMinute);
        return i;
    }

    public bool isLeftTheWeakest { get { return GetMinionsPerMinute(-1) < GetMinionsPerMinute(1); } }
    
}
