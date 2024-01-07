using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos
{
    public interface IItemList
    {
        public interface IItem
        {
            string GetID();
            string GetTag();
        }

        /**
         * Adds an {@link Item} to the store, replacing any existing item with the
         * same {@link Item#id} value.
         */
        void Put(IItem item);

        /**
         * Retrieves the {@link Item} with the given {@link Item#id} value, or
         * null if no such {@link Item} exists in the store.
        */
        IItem Get(string id);

        /**
         * Delete all {@link Item}s with the specified tag.
         */
        void DropAllByTag(string tag);

        /**
         * Returns the number of Items in the store
         */
        int Size();


    }
}
