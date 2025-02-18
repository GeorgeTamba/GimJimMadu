using UnityEngine;
using UnityEngine.Events;

namespace cowsins
{
    public class Trigger : MonoBehaviour
    {
        [System.Serializable]
        public class Events
        {
            public UnityEvent onEnter, onStay, onExit;
        }

        [SerializeField] private Events events;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                events.onEnter?.Invoke();
                TriggerEnter(other);    
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                events.onStay?.Invoke();
                TriggerStay(other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                events.onExit?.Invoke();
                TriggerExit(other);
            }
        }

        public virtual void TriggerEnter(Collider other)
        {

        }
        public virtual void TriggerStay(Collider other)
        {
        }

        public virtual void TriggerExit(Collider other)
        {

        }
    }

}