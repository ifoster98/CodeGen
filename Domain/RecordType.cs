using System.Collections.Generic;

namespace CodeGen.Domain
{
    public class RecordType {
        public string Modulename { get; }
        public string Entityname { get; }
        public string Entitynamelower { get; }
        public List<PropertyDefinition> Entityproperties { get; }

        public RecordType(
            string moduleName,
            string entityName, 
            List<PropertyDefinition> entityProperties
        )
        {
            Modulename = moduleName;
            Entityname = entityName;
            Entitynamelower = ToCamelCase(entityName);
            Entityproperties = entityProperties;
        }

        private string ToCamelCase(string item) => item.ToCharArray()[0].ToString().ToLower() + item.Substring(1);
    }

    public class PropertyDefinition {
        public string Propertyname { get; }
        public string Propertynamelower { get; }
        public string Propertytype { get; }
        public string Propertyrawtype { get; }

        public PropertyDefinition(string propertyName, string propertyType, string propertyrawtype)
        {
            Propertyname = propertyName;
            Propertytype = propertyType;
            Propertyrawtype = propertyrawtype;
            Propertynamelower = ToCamelCase(propertyName);
        }

        private string ToCamelCase(string item) => item.ToCharArray()[0].ToString().ToLower() + item.Substring(1);
    }
}
