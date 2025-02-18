using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class NoteScript : MonoBehaviour
    {
        [Multiline]
        public string noteText = "";
        [HideInInspector]
        public bool isReading = false;
        public MeshRenderer meshRenderer;

        void Start()
        {

        }

        public void Read()
        {
            if (isReading == false)
            {
                isReading = true;
                meshRenderer.enabled = false;
                GetComponent<Collider>().enabled = false;
                GameCanvas.Instance.Show_Note(noteText);
                GameCanvas.Instance.CurrentNote = this;
                BlinkEffect blinkEffect = GetComponentInChildren<BlinkEffect>();
                blinkEffect.Disable();
                GameCanvas.Instance.LastClickedArea = null;
            }
        }

        public void Unread()
        {
            isReading = false;
            GetComponent<SphereCollider>().enabled = true;
            meshRenderer.enabled = true;
        }
    }
}