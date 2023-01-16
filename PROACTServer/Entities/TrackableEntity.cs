using System;

namespace Proact.Services.Entities {
    public abstract class TrackableEntity : IEntity {
        public Guid Id { get; set; }
        public DateTime LastModified { get; set; }
        public Guid CreatorId { get; set; }
        public Guid ModifierId { get; set; }
        private DateTime _created { get; set; }
        public DateTime Created {
            get => _created.ToUniversalTime();
            set => _created = value;
        }

        public bool Modified {
            get {
                return LastModified > Created;
            }
        }
    }
}
