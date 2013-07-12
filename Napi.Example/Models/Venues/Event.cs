namespace Napi.Example.Models.Venues
{
    public class Event : IModel
    {
        public string DisplayName { get; set; }

        public override string ToString ()
        {
            return DisplayName;
        }
    }
}