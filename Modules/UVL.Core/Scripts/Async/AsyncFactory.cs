using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL
{
    /// <summary>
    /// Reusable method for working with AsyncResult coroutines from outside Monobehaviours.
    /// </summary>
    public class AsyncFactory : MonoBehaviour
    {
        public static AsyncFactory Instance => GetOrCreateInstance();
        private static AsyncFactory instance = null;

        private static AsyncFactory GetOrCreateInstance()
        {
            if (instance == null)
                instance = new GameObject("Async Factory").AddComponent<AsyncFactory>();

            return instance;
        }
    }
}