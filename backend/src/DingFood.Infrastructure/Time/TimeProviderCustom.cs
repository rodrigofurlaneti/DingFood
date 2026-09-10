using System;

namespace DingFood.Infrastructure.Time
{
    public class TimeProviderCustom : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}