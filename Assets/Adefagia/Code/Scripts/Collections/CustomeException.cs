using System;

namespace Adefagia.Collections
{
    public class CustomeException
    {
        
    }

    public class DictionaryDuplicate : Exception
    {
        public DictionaryDuplicate()
        {
            
        }
    }

    public class HasDeployException : Exception
    {
        public HasDeployException() : base("Robot Has Deployed before")
        {
        }
    }
}