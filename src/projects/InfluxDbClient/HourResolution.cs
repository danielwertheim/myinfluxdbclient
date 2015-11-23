namespace InfluxDbClient
{
    public class HourResolution : TimeStampResolution
    {
        protected override long OnApply(TimeStamp timeStamp)
        {
            return timeStamp.Ticks - timeStamp.Ticks % TicksPerHour;
        }
    }
}