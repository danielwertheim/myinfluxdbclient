namespace MyInfluxDbClient
{
    public class MinuteResolution : TimeStampResolution
    {
        protected override long OnApply(TimeStamp timeStamp)
        {
            return timeStamp.Ticks - timeStamp.Ticks % TicksPerMinute;
        }
    }
}