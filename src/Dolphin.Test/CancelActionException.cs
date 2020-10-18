using System;
using System.Runtime.Serialization;

namespace Dolphin.NewEventBus
{
    [Serializable]
    internal class CancelActionException : Exception
    {
        public CancelActionException()
        {
        }

        public CancelActionException(string message) : base(message)
        {
        }

        public CancelActionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CancelActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}