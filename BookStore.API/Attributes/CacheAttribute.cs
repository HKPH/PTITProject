using System;


namespace BookStore.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachedAttribute : Attribute
    {
        public int DurationInSeconds { get; }

        public CachedAttribute(int durationInSeconds = 60)
        {
            DurationInSeconds = durationInSeconds;
        }
    }

}
