using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL
{
    public class AsyncSpinner : MonoBehaviour
    {
        public float speed = 90f;

        private void Update()
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }

        public void Show()
            => gameObject.SetActive(true);

        public void Hide()
            => gameObject.SetActive(false);
    }
}