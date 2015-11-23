using System;
using System.Threading.Tasks;
using InfluxDbClient;

namespace MrLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Simple().Wait();
            //Stream(batches: 100).Wait();
        }

        static async Task Simple()
        {
            var measurements = new Measurements()
                .Add(new Measurement("ticketsold").AddTag("seller", "1").AddTag("cur", "SEK").AddField("amt", 150.0).AddField("fee", 50.0)) //TimeStamp is assigned in Influx
                .Add(new Measurement("ticketsold").AddTag("seller", "1").AddTag("cur", "USD").AddField("amt", 10.0).AddField("fee", 2.5).AddTimeStamp())
                .Add(new Measurement("ticketsold").AddTag("seller", "2").AddTag("cur", "USD").AddField("amt", 10.0).AddField("fee", 2.5).AddTimeStamp(DateTime.Now)) //Is converted to nanoseconds since Epoch (UTC)
                .Add(new Measurement("ticketsold").AddTag("seller", "1").AddTag("cur", "EUR").AddField("amt", 9.5).AddField("fee", 5.0).AddTimeStamp(TimeStampResolution.Nanoseconds))
                .Add(new Measurement("ticketsold").AddTag("seller", "2").AddTag("cur", "SEK").AddField("amt", 225.5).AddField("fee", 50.0).AddTimeStamp(TimeStampResolution.Microseconds))
                .Add(new Measurement("ticketsold").AddTag("seller", "3").AddTag("cur", "SEK").AddField("amt", 110.0).AddField("fee", 30.0).AddTimeStamp(TimeStampResolution.Milliseconds))
                .Add(new Measurement("ticketsold").AddTag("seller", "3").AddTag("cur", "SEK").AddField("amt", 2350.0).AddField("fee", 10.0).AddTimeStamp(TimeStampResolution.Seconds))
                .Add(new Measurement("ticketsold").AddTag("seller", "2").AddTag("cur", "EUR").AddField("amt", 51.8).AddField("fee", 5.0).AddTimeStamp(TimeStampResolution.Minutes))
                .Add(new Measurement("ticketsold").AddTag("seller", "1").AddTag("cur", "USD").AddField("amt", 6.75).AddField("fee", 1.0).AddTimeStamp(TimeStampResolution.Hours));

            using (var client = new InfluxDbClient.InfluxDbClient("http://192.168.1.176:9086"))
            {
                var response = await client.WriteMeasurementsAsync("mydb", measurements);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Content);
            }
        }

        static async Task Stream(int batches)
        {
            var measurements = new Measurements();
            var rnd = new Random();

            using (var client = new InfluxDbClient.InfluxDbClient("http://192.168.1.176:9086"))
            {
                var c = 0;
                for (var b = 0; b < batches; b++)
                {
                    var batchsize = rnd.Next(1, 10);
                    for (var i = 0; i < batchsize; i++)
                    {
                        decimal amount = rnd.Next(1, 3000);
                        var measurement = new Measurement("ticketsold")
                            .AddTag("id", ++c)
                            .AddTag("seller", rnd.Next(1, 10))
                            .AddTag("curr", "SEK")
                            .AddField("amount", amount)
                            .AddField("fee", rnd.Next(0, 50))
                            .AddField("vat", amount * 0.25M)
                            .AddTimeStamp(TimeStampResolution.Minutes);

                        measurements.Add(measurement);
                        await Task.Delay(rnd.Next(250, 1000));
                    }

                    var response = await client.WriteMeasurementsAsync("mydb", measurements);
                    Console.WriteLine(response.StatusCode);
                    measurements.Clear();

                    await Task.Delay(rnd.Next(250, 1000));
                }
            }
        }
    }
}
