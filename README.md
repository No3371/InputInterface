# InputInterface
A tool to integrate multiple input contexts.

:key: Insert "using InputInterface" to use.

## InputManager 
*MonoBehaviour/Singleton*

 :information_source: As a singleton Monobehaviour, this will mark the GameObject it attached to "Don't Destroy On Load".


### Members
- #### InputState activeState: 
    - The active InputState to use.
- #### Stack\<InputState\> stateHolder:
    - Holds states for later recovery.
- #### void Update()
    - The orignal Update() of Monobehaviours, get called every frame.
    ```
    void Update () {
        if (activeState != null ) activeState.activeAction.Invoke();        
    }
    ```

### Public Methods
        
- #### public void SetActiveState(InputState state)
    - Assign a new InputState.
    - :information_source: Noted that this will not preserve the overriding state, if the new state is just temporary, consider use SetTempState() instead.


- #### public void SetTempState (InputState state)
    - Assign a new InputState.
    - This will save currently active state to **stateHolder**.
    - Use EndTempState() to put the last state put in stateHolder.

- #### public void EndTempState (InputState state)
    - End the passed in InputState.
    - No matter how many new states has been set after the designated state, it will keep removing states until the state had been removed from InputManager.
    - **Example**
        - activeState: state4.
        - stateHolder: state3 > state2 > state1 > state 0.
        - Execute *EndTempState(**state2**);*
            - state4 is **state2**? No-> remove state4.
            - state3 is **state2**? No-> remove state3.
            - state2 is **state2**? Yes-> remove state2 and stop here.
        - activeState: state1.
        - stateHolder: state 0.
    
- #### public void EndTempState (string stateID)
    - End the InputState whose ID match the *stateID*.
    - No matter how many new states has been set after the designated state, it will keep removing states until the state had been removed from InputManager.
    - **Example**
        - activeState: state4.
        - stateHolder: state3 > state2 > state1 > state 0.
        - Execute *EndTempState(**state2**);*
            - state4 is **state2**? No-> remove state4.
            - state3 is **state2**? No-> remove state3.
            - state2 is **state2**? Yes-> remove state2 and stop here.
        - activeState: state1.
        - stateHolder: state 0.

## InputState
*Normal Class*

### Description
A state of InputManager, every state instance store what would happens when a input detected. Works with Unity's Input System.

### Content
```C#
//Optinal. Use the ID to find this when you want to end the state.
public string stateID;

protected InputManager context;

//Invoked by InputManager.Update().
//InputAction is a void delegate without parameters.
public InputAction activeAction;

//Constructor
public InputState (string stateID = "") {
    this.stateID = stateID;
    this.context = InputManager.Instance;
}
```

### Usage Example 1
#### Context: When the player character jumps up and in the air, you want it to perform differently.
```C#
inputState = new InputState("InTheAir");
//Lambda
inputState.activeAction += () => {
        if (Input.GetKeyUp(InputManager.Down))
            QuickDrop();
        if (Input.GetKey(InputManager.Up)) 
            TryToClimb()
    }
};
InputManager.Instance.SetTempState(inputState);
```

#### And when the character falls back on ground:
```
InputManager.Instance.EndTempState("InTheAir");
```

### Usage Example 2
#### Context: When a line of dialog text is showing up, if "Interact" key is pressed, skip the popping process.
```C#
internal virtual void DefineInput_SkipPopping(){
    inputState = new InputState("DialogBubbleController_SkipPopping");
    //Lambda
    inputState.activeAction += () => {
        if (Input.GetKeyUp(InputManager.Interact)) {
            SkipPopping();
            InputManager.Instance.EndTempState("DialogBubbleController_SkipPopping");
        }
    };
    InputManager.Instance.SetTempState(inputState);
}
```


