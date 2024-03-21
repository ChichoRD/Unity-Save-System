using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem.SaveService.Factory
{
    [CreateAssetMenu(fileName = "XmlSaveServiceFactory", menuName = "Save System/Save Service/Xml Factory")]
    internal class XmlSaveServiceFactory : SaveServiceFactory
    {
        private readonly List<Type> _xmlSerializerKnownTypes = new List<Type>();

        public override ISaveService Create() => CreateService();
        public XmlSaveService CreateService() => new XmlSaveService(_xmlSerializerKnownTypes);

        public XmlSaveServiceFactory WithXmlSerializerKnownTypes(IEnumerable<Type> types)
        {
            _xmlSerializerKnownTypes.AddRange(types);
            return this;
        }

        public XmlSaveServiceFactory WithoutXmlSerializerKnownTypes(IEnumerable<Type> types)
        {
            _xmlSerializerKnownTypes.RemoveAll(types.Contains);
            return this;
        }

        public XmlSaveServiceFactory WithXmlSerializerKnownTypes(params Type[] types) =>
            WithXmlSerializerKnownTypes(types.AsEnumerable());

        public XmlSaveServiceFactory WithoutXmlSerializerKnownTypes(params Type[] types) =>
            WithoutXmlSerializerKnownTypes(types.AsEnumerable());

        private void OnEnable()
        {
            _xmlSerializerKnownTypes.Clear();
        }
    }
}