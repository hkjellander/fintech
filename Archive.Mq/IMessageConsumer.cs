using System;
namespace Archive.Mq
{
    public interface IMessageConsumer
    {
        // Setup everything needed for consuming messages from the MQ and
        // then return. Messages should be consumed after triggering events,
        // after this method has returned.
        void ConsumeMessages();
    }
}
