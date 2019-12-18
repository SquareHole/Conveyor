using System;
using System.Runtime.Serialization;

namespace Talista.Conveyor
{
    [Serializable]
    public class MissingExpectedContextDataException : Exception
    {
        public MissingExpectedContextDataException()
        {
        }

        public MissingExpectedContextDataException(string message) : base(message)
        {
        }

        public MissingExpectedContextDataException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingExpectedContextDataException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}