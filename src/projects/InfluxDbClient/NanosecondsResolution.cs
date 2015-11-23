namespace InfluxDbClient
{
    public class NanosecondsResolution : TimeStampResolution
    {
        protected override long OnApply(TimeStamp timeStamp)
        {
            return timeStamp.Ticks;
        }
    }
}