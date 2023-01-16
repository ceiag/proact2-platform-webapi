namespace Proact.Services.Entities {
    public class Protocol : TrackableEntity, IEntity {
        public string Name { get; set; }
        public string InternalCode { get; set; }
        public string InternationalCode { get; set; }
    }
}
