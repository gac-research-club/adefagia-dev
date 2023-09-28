using System;

namespace Adefagia.Store
{
    public struct ItemAndAmountSpec
    {
        public string id;
        public string name;
        public int amount;

        public ItemAndAmountSpec(string id, int amount, string name)
        {
            this.id = id;
            this.amount = amount;
            this.name = name;
        }

        public override string ToString()
        {
            return $"{id}:{name}";
        }
    }
}
