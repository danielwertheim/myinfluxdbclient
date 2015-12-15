using System;

namespace MyInfluxDbClient
{
    public class DropSeries : SeriesQuery<DropSeries>
    {
        public DropSeries() : base("drop series") {}

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(From) || !string.IsNullOrWhiteSpace(Where);
        }

        public override string Generate()
        {
            if(!IsValid())
                throw new InvalidOperationException($"{nameof(DropSeries)} needs to have either a {nameof(From)} or a {nameof(Where)} statement defined to be able to generate the command.");

            return base.Generate();
        }
    }
}