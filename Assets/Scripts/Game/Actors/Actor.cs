using UnityEngine;

namespace Game.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        [field: SerializeField] public Transform SelfTransform { get; private set; }

        public virtual void Initialize() {}
    }
}