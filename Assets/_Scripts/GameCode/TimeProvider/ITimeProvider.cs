using System;

namespace GameCode.TimeProvider
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }
    
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}