using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public class NeighbourStrategyFactory
    {
        Dictionary<string, System.Type> strategies;

        public NeighbourStrategyFactory()
        {
            LoadTypesIFindNeighbourStrategy();
        }

        private void LoadTypesIFindNeighbourStrategy()
        {
            strategies = new Dictionary<string, System.Type>();
            System.Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in typesInThisAssembly)
            {
                if (type.GetInterface(typeof(IFindNeighbourStrategy).ToString()) != null)
                {
                    strategies.Add(type.Name.ToLower(), type);
                }
            }
        }

        internal IFindNeighbourStrategy CreateInstance(string nameOfStrategy)
        {
            System.Type t = GetTypeToCreate(nameOfStrategy);
            if (t == null)
            {
                t = GetTypeToCreate("more");
            }
            return Activator.CreateInstance(t) as IFindNeighbourStrategy;
        }

        private System.Type GetTypeToCreate(string nameOfStrategy)
        {
            foreach (var possibleStrategy in strategies)
            {
                if (possibleStrategy.Key.Contains(nameOfStrategy))
                {
                    return possibleStrategy.Value;
                }
            }
            return null;
        }
    }
}
