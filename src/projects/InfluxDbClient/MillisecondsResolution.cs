namespace InfluxDbClient
{
    public class MillisecondsResolution : TimeStampResolution
    {
        protected override long OnApply(TimeStamp timeStamp)
        {
            return timeStamp.Ticks - timeStamp.Ticks % TicksPerMillisecond;
        }
    }
}