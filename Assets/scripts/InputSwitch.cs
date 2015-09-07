using UnityEngine;
using System.Collections;

public class InputSwitch : MonoBehaviour {

    public string switchButton;
    public RadialMovementInput[] inputs;
    public int index;
    public int Index {
        get { return index; }
        set { index = (value + inputs.Length) % inputs.Length;
            
        }
    }

    void Set() {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].inUse = i == Index;
            Debug.Log(Index);
        }
    }
    void Start() {
        Set();
    }

    void Update() {
        if (Input.GetButtonDown(switchButton)) {
            Index += 1;
            Set();
        }
    }

}
