using UnityEngine;
using System.Collections;

public interface Attackable  {

    void Damage(MonoBehaviour damager);
    bool Attackable { get; }
}
