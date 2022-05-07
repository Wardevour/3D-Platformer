namespace Ecksoft.GameManagement {

    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(ServiceContainer))]
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; } = null;

        public List<GameState> gameStates = new List<GameState>();

        private Dictionary<string, GameObject> services = null;

        private GameState currentState = null;

        public GameState CurrentState {

            get {
                return currentState;
            }

            set {
                if (value == currentState)
                {
                    return;
                }

                ChangeState(value);
            }
        }


        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            ServiceContainer serviceContainer = GetComponent<ServiceContainer>();
            if (services == null) {
                services = serviceContainer.GetDictionary();
            }
        }

        private void Start() {
            if (gameStates.Count <= 0) {
                Debug.LogError("Game Manager is missing Game States");
                return;
            }

            ChangeState(gameStates[0]);
        }


        public void ChangeState(GameState nextState) {
            GameState foundState = gameStates.Find((GameState possibleState) => {
                return possibleState == nextState;
            });

            if (currentState != null)
            {
                currentState.Sleep();
            }

            currentState = foundState;
            if (currentState != null)
            {
                currentState.Wake();
            }
        }

        public GameObject GetService(string serviceName) {
            if (services.ContainsKey(serviceName)) {
                return services[serviceName];
            }

            return null;
        }

        public GameObject AddService(string serviceName, GameObject serviceObject, bool setAsParent = false)
        {
            if (services.ContainsKey(serviceName))
            {
                return services[serviceName];
            }

            if (setAsParent)
            {
                // set the new service as a child to keep it between scenes with the GM
                serviceObject.transform.SetParent(transform, true);
            }

            services.Add(serviceName, serviceObject);
            return services[serviceName];
        }
    }
}
