using UnityEngine;
using System.Collections;

public class InputSwitch : MonoBehaviour {

    public string switchLButton;
    public string switchRButton;
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
        }
    }
    void Start() {
        Set();
    }

    void Update() {
        if (Input.GetButtonDown(switchLButton)) {
            Index += 1;
            Set();
        }else if (Input.GetButtonDown(switchRButton))
            {
                Index -= 1;
                Set();
            }
    }

}
