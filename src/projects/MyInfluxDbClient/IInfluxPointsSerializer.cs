namespace MyInfluxDbClient
{
    public interface IInfluxPointsSerializer
    {
        byte[] Serialize(InfluxPoints points);
    }
}