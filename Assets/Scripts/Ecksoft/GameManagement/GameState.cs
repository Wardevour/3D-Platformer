namespace Ecksoft.GameManagement {

    using UnityEngine;
    using UnityEngine.Events;

    public abstract class GameState : MonoBehaviour {

        [SerializeField]
        private UnityEvent onAwake = new UnityEvent();

        [SerializeField]
        private UnityEvent onAsleep = new UnityEvent();


        public void Wake() {
            onAwake.Invoke();
        }

        public void AddWakeListener(UnityAction action) {
            onAwake.AddListener(action);
        }

        public void RemoveWakeListener(UnityAction action) {
            onAwake.RemoveListener(action);
        }


        public void Sleep() {
            onAsleep.Invoke();
        }

        public void AddSleepListener(UnityAction action) {
            onAsleep.AddListener(action);
        }

        public void RemoveSleepListener(UnityAction action) {
            onAsleep.RemoveListener(action);
        }


        public void RemoveAllListeners() {
            onAwake.RemoveAllListeners();
            onAsleep.RemoveAllListeners();
        }
    }
}
