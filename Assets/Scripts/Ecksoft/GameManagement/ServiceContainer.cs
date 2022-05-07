namespace Ecksoft.GameManagement {

    using UnityEngine;
    using System.Collections.Generic;

    [ExecuteInEditMode]
    [System.Serializable]
    public class ServiceContainer : MonoBehaviour, ISerializationCallbackReceiver {

        private Dictionary<string, GameObject> _container = null;

        public string[] _keys = null;
        public GameObject[] _values = null;

        public void OnBeforeSerialize() {
            string[] _keys = null;
            GameObject[] _values = null;

            if (_container == null) {
                _container = new Dictionary<string, GameObject>();
            }

            _keys = new string[_container.Count];
            _values = new GameObject[_container.Count];

            int i = 0;
            foreach (KeyValuePair<string, GameObject> kvp in _container) {
                _keys[i] = kvp.Key;
                _values[i] = kvp.Value;
                i++;
            }
        }

        public void OnAfterDeserialize() {
            if (_container == null) {
                _container = new Dictionary<string, GameObject>();
            }

            for (var i = 0; i != System.Math.Min(_keys.Length, _values.Length); i++)
                _container[_keys[i]] = _values[i];
        }

        public Dictionary<string, GameObject> GetDictionary() {
            if (_container == null) {
                _container = new Dictionary<string, GameObject>();
            }

            return _container;
        }
    }
}
