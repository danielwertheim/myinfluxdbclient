using System.Text;

namespace MyInfluxDbClient.Protocols
{
    public class LineProtocolInfluxPointsSerializer : IInfluxPointsSerializer
    {
        public byte[] Serialize(InfluxPoints points)
        {
            var sb = new StringBuilder();
            var writer = new StringBuilderLineProtocolWriter(sb);
            writer.Write(points);

            var buff = LineProtocolFormat.Encoding.GetBytes(sb.ToString());
            sb.Clear();

            return buff;
        }
    }
}