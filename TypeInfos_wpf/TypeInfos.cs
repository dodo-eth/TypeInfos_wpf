 
using System.Collections.Generic; 
using Newtonsoft.Json;   

namespace TypeInfos_wpf
{
    public class TypeInfos_json
    {

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Root
        {
            [JsonConstructor]
            public Root(
                [JsonProperty("TypeInfos")] List<TypeInfo> typeInfos
            )
            {
                this.TypeInfos = typeInfos;
            }

            [JsonProperty("TypeInfos")]
            public IReadOnlyList<TypeInfo> TypeInfos { get; }
        }

        public class TypeInfo
        {
            [JsonConstructor]
            public TypeInfo(
                [JsonProperty("TypeName")] string typeName,
                [JsonProperty("Propertys")] object propertys
            )
            {
                this.TypeName = typeName;
                this.Propertys = propertys;
            }

            [JsonProperty("TypeName")]
            public string TypeName { get; }

            [JsonProperty("Propertys")]
            public object Propertys { get; }
        }





    }




}
