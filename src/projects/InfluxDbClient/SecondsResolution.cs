namespace InfluxDbClient
{
    public class SecondsResolution : TimeStampResolution
    {
        protected override long OnApply(TimeStamp timeStamp)
        {
            return timeStamp.Ticks - timeStamp.Ticks % TicksPerSecond;
        }
    }
}