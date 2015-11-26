namespace MyInfluxDbClient
{
    public class MicrosecondsResolution : TimeStampResolution
    {
        protected override long OnApply(TimeStamp timeStamp)
        {
            return timeStamp.Ticks - timeStamp.Ticks % TicksPerMicrosecond;
        }
    }
}