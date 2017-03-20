using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InputInterface{

    public class InputManager : MonoBehaviour {


        static InputManager instance = null;
        public static InputManager Instance
        {
            get { return instance; }
        }

        void Awake()
        {
            if (!instance) instance = this;
            if (instance != this) Destroy(this);
            DontDestroyOnLoad(this);
        }

        public static KeyCode Up, Down, Right, Left, Interact, Interact2, Past, Now, Future, Esc;

        // Use this for initialization
        void Start () {
        
        }

        InputState activeState;

        Stack<InputState> stateHolder;
                
        void Update () {
            if (activeState != null ) activeState.activeAction.Invoke();        
        }

        ///Assign a new InputState, noted that this will not preserve the overriding state, if the new state is just temporary, consider use SetTempState() instead.
        public void SetActiveState(InputState state) {
            activeState = state;
        }

        ///Use this method to preserve currently active state, then use EndTempState() to put it back later.
        public void SetTempState (InputState state) {
            stateHolder.Push(activeState);
            activeState = state;
        }

        public void EndTempState (InputState state) {
            if (!stateHolder.Contains(state)) Debug.LogWarning("InputManager:: EndTempState():: StateHolder does not contain the state.");
            else {
                while (activeState != state) {
                    activeState = stateHolder.Pop();
                    Debug.Log("InputManager:: EndTempState():: Popped state (ID: " + activeState.stateID + ")");
                }
                activeState = stateHolder.Pop();
            }
        }

        public void EndTempState (string stateID) {
            if (!stateHolder.Any(e => e.stateID == stateID)) Debug.LogWarning("InputManager:: EndTempState():: StateHolder does not contain the state.");
            else {
                while (activeState.stateID != stateID) {
                    activeState = stateHolder.Pop();
                    Debug.Log("InputManager:: EndTempState():: Popped state (ID: " + activeState.stateID + ")");
                }
                activeState = stateHolder.Pop();
            }
        }

        void LoadDefult()
        {
            Up = KeyCode.W;
            Down = KeyCode.S;
            Left = KeyCode.A;
            Right = KeyCode.D;
            Interact = KeyCode.F;
            Interact2 = KeyCode.H;
            Past = KeyCode.J;
            Now = KeyCode.K;
            Future = KeyCode.L;
            Esc = KeyCode.Escape;
        }

        void LoadPreset(){
            
        }
    }
    
    public delegate void InputAction();

    public class InputState {
        
        public string stateID;

        protected InputManager context;

        //Invoked by InputManager.Update().
        public InputAction activeAction;

        public InputState (string stateID = "") {
            this.stateID = stateID;
            this.context = InputManager.Instance;
        }

    }

}