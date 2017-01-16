namespace MyInfluxDbClient
{
    public class Select : Query<Select>
    {
        private Select() : base("select") { }

        public static Select All()
            => new Select().SelectedFields("*");

        public static Select Fields(string fields)
            => new Select().SelectedFields(fields);
    }
}