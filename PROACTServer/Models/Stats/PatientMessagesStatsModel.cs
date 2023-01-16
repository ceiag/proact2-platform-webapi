namespace Proact.Services.Models.Stats {
    public class PatientMessagesStatsModel : UserMessagesStatsModel {
        public PatientMessagesStatsModel( int numberOfUsers ) : base( numberOfUsers ) {
        }

        public int TopicsTextOnly { get; set; }
        public int TopicsWithVideo { get; set; }
        public int TopicsWithAudio { get; set; }
        public int TopicsWithImage { get; set; }
        public int UnrepliedTextOnly { get; set; }
        public int UnrepliedWithVideo { get; set; }
        public int UnrepliedWithAudio { get; set; }
        public int UnrepliedWithImage { get; set; }
        public double AvgTopicsWithVideoDuration { get; set; }
        public double AvgTopicsWithAudioDuration { get; set; }
        public double AvgTopicsTextLength { get; set; }
        public int ScopeNotSpecified { get; set; }
        public int InfoScope { get; set; }
        public int HealthScope { get; set; }
        public int MoodVeryBad { get; set; }
        public int MoodBad { get; set; }
        public int MoodGood { get; set; }
        public int MoodVeryGood { get; set; }
        public int MoodNotSpecified { get; set; }

        public double AvgTopicsTextOnly {
            get => (double)TopicsTextOnly / (double)_numberOfUsers;
        }

        public double AvgTopicsWithVideo {
            get => (double)TopicsWithVideo / (double)_numberOfUsers;
        }

        public double AvgTopicsWithAudio {
            get => (double)TopicsWithAudio / (double)_numberOfUsers;
        }

        public double AvgTopicsWithImage {
            get => (double)TopicsWithImage / (double)_numberOfUsers;
        }

        public double AvgUnrepliedTextOnly {
            get => (double)UnrepliedTextOnly / (double)_numberOfUsers;
        }

        public double AvgUnrepliedWithVideo {
            get => (double)UnrepliedWithVideo / (double)_numberOfUsers;
        }
        
        public double AvgUnrepliedWithAudio {
            get => (double)UnrepliedWithAudio / (double)_numberOfUsers;
        }
        
        public double AvgUnrepliedWithImage {
            get => (double)UnrepliedWithImage / (double)_numberOfUsers;
        }
        
        public int TotalTopics {
            get => TopicsTextOnly + TopicsWithVideo + TopicsWithAudio + TopicsWithImage;
        }

        public int TotalMessages {
            get => TotalTopics + TotalReplies;
        }
        public int TotalUnreplied {
            get => UnrepliedTextOnly + UnrepliedWithVideo + UnrepliedWithAudio + UnrepliedWithImage;
        }
    }
}
