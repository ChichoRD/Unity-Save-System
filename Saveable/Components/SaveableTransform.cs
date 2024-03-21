using UnityEngine;

namespace SaveSystem.Saveable.Components
{
    internal class SaveableTransform : MonoBehaviour, ISaveable<SaveableTransform.Transform>
    {
        [SerializeField]
        private UnityEngine.Transform _transform;

        public Transform GetData() => Transform.From(transform);
        public bool TrySetData(Transform saveData) => saveData.As(transform);

        public struct Transform
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;

            public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                this.position = position;
                this.rotation = rotation;
                this.scale = scale;
            }

            public readonly UnityEngine.Transform As(UnityEngine.Transform transform)
            {
                transform.SetPositionAndRotation(position, rotation);
                transform.localScale = scale;
                return transform;
            }

            public static Transform From(UnityEngine.Transform transform)
            {
                return new Transform(transform.position, transform.rotation, transform.localScale);
            }
        }

    }
}
