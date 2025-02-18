using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class LadderScript : MonoBehaviour
    {
        public Vector3 StartingScale;
        public bool isPut = false;
        void Start()
        {
            StartingScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
