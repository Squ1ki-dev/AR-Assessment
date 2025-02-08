using System.Collections;
using UnityEngine;

namespace Code.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
        public virtual void Close()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void Open()
        {
            this.gameObject.SetActive(true);
        }
    }
}