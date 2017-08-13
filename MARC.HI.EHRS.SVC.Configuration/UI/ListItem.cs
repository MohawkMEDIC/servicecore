using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    /// <summary>
    /// List item
    /// </summary>
    public class ListItem<TKey, TDisplay>
    {
        /// <summary>
        /// Gets the key
        /// </summary>
        public TKey Key { get; private set;  }

        /// <summary>
        /// Gets the display
        /// </summary>
        public TDisplay Display { get; private set; }

        /// <summary>
        /// Creates a new list item
        /// </summary>
        public ListItem(TKey key, TDisplay display)
        {
            this.Key = key;
            this.Display = display;
        }

        /// <summary>
        /// Represent as a strin
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Display.ToString();
        }

        /// <summary>
        /// Equals operation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ListItem<TKey, TDisplay>)
                return (obj as ListItem<TKey, TDisplay>).Key.Equals(this.Key);
            else if (obj is TKey)
                return this.Key.Equals(obj);
            else
                return base.Equals(obj);
        }
    }
}
