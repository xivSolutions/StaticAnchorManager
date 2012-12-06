using System;

namespace WLWStaticAnchorManager
{
    public class htmlAttribute
    {
        private string _Attribute = "";
        private string _value = "";
        private char _delimiter = (char)0;

        public htmlAttribute(String AttributeID, String Value, Char Delimiter)
        {
            _Attribute = AttributeID;
            _value = Value;
            _delimiter = Delimiter;
        }


        public String AttributeID
        {
            get { return _Attribute; }
        }


        public String Value
        {
            get { return _value; }
        }


        public Char Delimiter
        {
            get { return _delimiter; }
        }


        public override string ToString()
        {
            return _Attribute + "=" + _delimiter + _value + _delimiter;
        }
    }
}
