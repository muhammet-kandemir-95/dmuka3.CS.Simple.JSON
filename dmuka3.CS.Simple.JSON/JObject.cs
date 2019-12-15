using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace dmuka3.CS.Simple.JSON
{
    /// <summary>
    /// Json Object
    /// </summary>
    public class JObject : DynamicObject
    {
        #region Variables
        private readonly Dictionary<string, object> _dynamicProperties = new Dictionary<string, object>();
        private object _value = null;
        private bool _obj = false;

        /// <summary>
        /// Get an item by name.
        /// </summary>
        /// <param name="name">Item name.</param>
        /// <returns></returns>
        public object this[string name]
        {
            get => this._dynamicProperties[name];
            set => this.TrySetMemberByName(name, value);
        }
        #endregion

        #region Constructors
        internal JObject()
        {
            this._obj = true;
        }

        internal JObject(object value)
        {
            this._value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Try to set a value.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return this.TrySetMemberByName(binder.Name, value);
        }

        internal bool TrySetMemberByName(string name, object value)
        {
            if (value == json.undefined)
            {
                if (_dynamicProperties.ContainsKey(name))
                    _dynamicProperties.Remove(name);
                return true;
            }

            object result = json.dic(value);

            if (_dynamicProperties.ContainsKey(name))
                _dynamicProperties[name] = result;
            else
                _dynamicProperties.Add(name, result);

            return true;
        }

        /// <summary>
        /// Try to get a value.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "v")
            {
                result = this._value;
                return true;
            }
            return _dynamicProperties.TryGetValue(binder.Name, out result);
        }

        private string clearString(string value)
        {
            return value
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t")
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
        }

        /// <summary>
        /// Get string as JSON.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this._obj)
            {
                StringBuilder json = new StringBuilder();
                json.Append("{");
                int itemCounter = 0;
                foreach (var item in this._dynamicProperties)
                {
                    if (itemCounter != 0)
                        json.Append(',');
                    json.Append('"' + clearString(item.Key) + "\":" + item.Value.ToString());
                    itemCounter++;
                }
                json.Append("}");
                return json.ToString();
            }
            if (this._value == null)
                return "null";
            else if (this._value is string)
                return '"' + clearString((string)this._value) + '"';
            else if (this._value is bool)
                return (bool)this._value == true ? "true" : "false";
            else if (this._value is DateTime)
                return '"' + ((DateTime)this._value).ToString("yyyy-MM-dd HH:mm:ss.fff") + '"';
            return ((IConvertible)this._value).ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
