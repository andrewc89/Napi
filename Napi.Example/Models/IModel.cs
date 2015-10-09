using Napi.Models.Interfaces;

namespace Napi.Example.Models
{
    public abstract class IModel : INapiModel<long>
    {
        public long ID { get; set; }
        public bool Active { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}