namespace Ecksoft.GameManagement.Tests {

    using UnityEngine;
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System.Collections;
    using Ecksoft.GameManagement;

    public class GameManagerTests : IPrebuildSetup {

        public void Setup() {
            // setup a new game manager
            GameObject go = new GameObject("Game Manager");

            GameManager gc = go.AddComponent<GameManager>();

            // loading state is first and should be the current state on Awake
            gc.gameStates.Add(go.AddComponent<LoadingState>());
            gc.gameStates.Add(go.AddComponent<RunningState>());
            gc.gameStates.Add(go.AddComponent<PausedState>());
            gc.gameStates.Add(go.AddComponent<WaitingState>());
            gc.gameStates.Add(go.AddComponent<GameOverState>());
        }

        [UnityTest]
        public IEnumerator GetGameController() {
            GameManager test = null;

            // look for the game manager
            yield return new WaitUntil(() => {
                test = GameManager.Instance;
                return test != null;
            });

            // get the loading state, it should be the current state
            LoadingState ls = test.gameStates.Find((state) => {
                return state is LoadingState;
            }) as LoadingState;

            // assert that the loading state is the current one
            Assert.IsTrue(ls == test.CurrentState);
        }

        [UnityTest]
        public IEnumerator ChangeGameStates() {
            GameManager test = null;

            // look for the game manager
            yield return new WaitUntil(() => {
                test = GameManager.Instance;
                return test != null;
            });

            // get the running state
            RunningState rs = test.gameStates.Find((state) => {
                return state is RunningState;
            }) as RunningState;

            // get the current state
            GameState oldState = test.CurrentState;

            // assert that oldState is the loading state
            Assert.IsTrue(oldState is LoadingState);

            // make wake and sleep delegates for the running state
            UnityEngine.Events.UnityAction wake = () => {
                Debug.Log("wake");
            };

            UnityEngine.Events.UnityAction sleep = () => {
                Debug.Log("sleep");
            };

            // make wake delegate for the oldState
            UnityEngine.Events.UnityAction oldWake = () => {
                Debug.Log("old wake");
            };

            // add the delegates to the state events
            rs.AddWakeListener(wake);
            rs.AddSleepListener(sleep);
            oldState.AddWakeListener(oldWake);

            // trigger a game state change
            test.ChangeState(rs);
            LogAssert.Expect(LogType.Log, "wake");

            // change state back to the original state
            test.ChangeState(oldState);
            LogAssert.Expect(LogType.Log, "sleep");
            LogAssert.Expect(LogType.Log, "old wake");

            // remove the delegates
            rs.RemoveWakeListener(wake);
            rs.RemoveSleepListener(sleep);
            oldState.RemoveWakeListener(oldWake);

            // assert that current state is the loading state
            Assert.IsTrue(test.CurrentState is LoadingState);
        }

        [UnityTest]
        public IEnumerator TestServices() {
            GameManager test = null;

            // look for the game manager
            yield return new WaitUntil(() => {
                test = GameManager.Instance;
                return test != null;
            });

            // add a new service
            GameObject testService = new GameObject("TestService");
            test.AddService("Test Service", testService);

            // assert that the service is available
            Assert.IsTrue(testService == test.GetService("Test Service"));

            // test that the original service cannot be replaced or recreated
            GameObject badService = new GameObject("BadService");
            test.AddService("Test Service", badService);

            Assert.IsTrue(badService != test.GetService("Test Service"));

            // make sure the original is still available
            Assert.IsTrue(testService == test.GetService("Test Service"));
        }
    }
}
