using System.Runtime.Serialization;
using System;
using UnityEngine;
using Newtonsoft.Json;

namespace SaveSystem.Saveable.Components
{
    internal class SaveableTransform : MonoBehaviour, ISaveable<SaveableTransform.Transform>
    {
        [SerializeField]
        private UnityEngine.Transform _transform;

        public Transform GetData() => Transform.From(_transform);
        public bool TrySetData(Transform saveData) => saveData.As(_transform);

        [DataContract]
        [JsonObject(MemberSerialization.OptOut)]
        [Serializable]
        public struct Transform
        {
            public float positionX;
            public float positionY;
            public float positionZ;

            public float rotationX;
            public float rotationY;
            public float rotationZ;
            public float rotationW;

            public float scaleX;
            public float scaleY;
            public float scaleZ;

            public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                positionX = position.x;
                positionY = position.y;
                positionZ = position.z;

                rotationX = rotation.x;
                rotationY = rotation.y;
                rotationZ = rotation.z;
                rotationW = rotation.w;

                scaleX = scale.x;
                scaleY = scale.y;
                scaleZ = scale.z;
            }

            public readonly UnityEngine.Transform As(UnityEngine.Transform transform)
            {
                transform.SetPositionAndRotation(
                    new Vector3(positionX, positionY, positionZ),
                    new Quaternion(rotationX, rotationY, rotationZ, rotationW));
                transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                return transform;
            }

            public static Transform From(UnityEngine.Transform transform)
            {
                return new Transform(transform.position, transform.rotation, transform.localScale);
            }
        }
    }
}
