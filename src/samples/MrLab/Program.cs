using System;
using System.Threading.Tasks;
using MyInfluxDbClient;

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
            var points = new InfluxPoints()
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "1").AddTag("cur", "SEK").AddField("amt", 150.0).AddField("fee", 50.0)) //TimeStamp is assigned in Influx
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "1").AddTag("cur", "USD").AddField("amt", 10.0).AddField("fee", 2.5).AddTimeStamp())
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "2").AddTag("cur", "USD").AddField("amt", 10.0).AddField("fee", 2.5).AddTimeStamp(DateTime.Now)) //Is converted to nanoseconds since Epoch (UTC)
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "1").AddTag("cur", "EUR").AddField("amt", 9.5).AddField("fee", 5.0).AddTimeStamp(TimeStampResolutions.Nanoseconds))
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "2").AddTag("cur", "SEK").AddField("amt", 225.5).AddField("fee", 50.0).AddTimeStamp(TimeStampResolutions.Microseconds))
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "3").AddTag("cur", "SEK").AddField("amt", 110.0).AddField("fee", 30.0).AddTimeStamp(TimeStampResolutions.Milliseconds))
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "3").AddTag("cur", "SEK").AddField("amt", 2350.0).AddField("fee", 10.0).AddTimeStamp(TimeStampResolutions.Seconds))
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "2").AddTag("cur", "EUR").AddField("amt", 51.8).AddField("fee", 5.0).AddTimeStamp(TimeStampResolutions.Minutes))
                .Add(new InfluxPoint("ticketsold").AddTag("seller", "1").AddTag("cur", "USD").AddField("amt", 1.0).AddField("fee", 1.0).AddTimeStamp(TimeStampResolutions.Hours));

            using (var client = new InfluxDbClient("http://192.168.1.176:9086"))
            {
                //configure write options on client (if you want)
                client.DefaultWriteOptions
                    .SetRetentionPolicy("test")
                    .SetTimeStampPrecision(TimeStampPrecision.Minutes)
                    .SetConsistency(Consistency.Quorum);

                await client.CreateDbAsync("mydb");
                await client.WriteAsync("mydb", points);

                //specific options can be passed
                var writeOptions = new WriteOptions()
                    .SetConsistency(Consistency.Any);

                await client.WriteAsync("mydb", points, writeOptions);
            }
        }

        static async Task Stream(int batches)
        {
            var measurements = new InfluxPoints();
            var rnd = new Random();

            using (var client = new InfluxDbClient("http://192.168.1.176:9086"))
            {
                var c = 0;
                for (var b = 0; b < batches; b++)
                {
                    var batchsize = rnd.Next(1, 10);
                    for (var i = 0; i < batchsize; i++)
                    {
                        decimal amount = rnd.Next(1, 3000);
                        var measurement = new InfluxPoint("ticketsold")
                            .AddTag("id", ++c)
                            .AddTag("seller", rnd.Next(1, 10))
                            .AddTag("curr", "SEK")
                            .AddField("amount", amount)
                            .AddField("fee", rnd.Next(0, 50))
                            .AddField("vat", amount * 0.25M)
                            .AddTimeStamp(TimeStampResolutions.Minutes);

                        measurements.Add(measurement);
                        await Task.Delay(rnd.Next(250, 1000));
                    }

                    await client.WriteAsync("mydb", measurements);
                    measurements.Clear();

                    await Task.Delay(rnd.Next(250, 1000));
                }
            }
        }
    }
}
