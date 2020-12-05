using System;

namespace CodeGen.Domain {
    public class TypeAlias {
        public string Businessname { get; }
        public string Applicationname { get; }
        public string Modulename { get; }
        public string Typename { get; }
        public string Propertytype { get; }
        public string Lowername { get; }

        public TypeAlias(
            string businessname,
            string applicationname,
            string modulename,
            string typename,
            string propertytype
            )
            {
                Businessname = businessname;
                Applicationname = applicationname;
                Modulename = modulename;
                Typename = typename;
                Propertytype = propertytype;
                Lowername = toLowerFirstChar(typename);
            }

        private string toLowerFirstChar(string str) =>
            str[0].ToString().ToLower() + str.Substring(1);
    }
}
