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
    /// JSON Array
    /// </summary>
    public class JArray : IEnumerable<object>
    {
        #region Variables
        private readonly List<object> _list = new List<object>();

        /// <summary>
        /// Get an item by index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public object this[int index]
        {
            get => this._list[index];
            set => this._list[index] = json.dic(value);
        }

        /// <summary>
        /// Length of list.
        /// </summary>
        public int length => this._list.Count;
        #endregion

        #region Constructors
        internal JArray() { }

        internal JArray(params object[] values)
        {
            foreach (var item in values)
                this.push(item);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new item to list.
        /// </summary>
        /// <param name="item"></param>
        public void push(object item)
        {
            this._list.Add(json.dic(item));
        }

        /// <summary>
        /// Clear list.
        /// </summary>
        public void clear()
        {
            this._list.Clear();
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        /// <summary>
        /// Insert an item by index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void insert(int index, object item)
        {
            this._list.Insert(index, json.dic(item));
        }

        /// <summary>
        /// Remove an item by index.
        /// </summary>
        /// <param name="index"></param>
        public void removeAt(int index)
        {
            this._list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        /// <summary>
        /// Get string as JSON.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            int itemCounter = 0;
            foreach (var item in this._list)
            {
                if (itemCounter != 0)
                    json.Append(',');
                json.Append(item.ToString());
                itemCounter++;
            }
            json.Append("]");
            return json.ToString();
        }
        #endregion
    }
}
