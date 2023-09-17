using System;
using System.Runtime.Serialization;
using Unity.Collections;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public struct SaveTestData
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
