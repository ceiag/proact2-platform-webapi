namespace Proact.Services.Entities {
    public class MobileAppsInfo : TrackableEntity, IEntity, IChangeHistoryTrackingEntity {
        public int AndroidLastBuildRequired { get; set; }
        public int iOSLastBuildRequired { get; set; }
        public string AndroidStoreUrl { get; set; }
        public string iOSStoreUrl { get; set; }
    }
}
