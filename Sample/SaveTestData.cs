#if UNITY_EDITOR
using System;
using System.Runtime.Serialization;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
[Serializable]
internal struct SaveTestData
{
    [JsonProperty]
    [DataMember]
    public int testInt;
    [JsonProperty]
    [DataMember]
    public float testFloat;
    [JsonProperty]
    [DataMember]
    public string testString;
    [JsonIgnore]
    [IgnoreDataMember]
    public Texture2D testTexture;
    [JsonProperty]
    [DataMember]
    [HideInInspector]
    public byte[] testTextureData;
}
#endif