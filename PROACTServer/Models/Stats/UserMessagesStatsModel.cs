namespace Proact.Services.Models.Stats {
    public abstract class UserMessagesStatsModel {
        protected readonly int _numberOfUsers;

        public UserMessagesStatsModel( int numberOfUsers ) {
            _numberOfUsers = numberOfUsers;
        }

        public int RepliesTextOnly { get; set; }
        public int RepliesWithVideo { get; set; }
        public int RepliesWithAudio { get; set; }
        public int RepliesWithImage { get; set; }
        public double AvgRepliesWithVideoDuration { get; set; }
        public double AvgRepliesWithAudioDuration { get; set; }
        public double AvgRepliesTextLength { get; set; }

        public double AvgRepliesTextOnly {
            get => (double)RepliesTextOnly / (double)_numberOfUsers;
        }
        
        public double AvgRepliesWithVideo {
            get => (double)RepliesWithVideo / (double)_numberOfUsers;
        }
        
        public double AvgRepliesWithAudio {
            get => (double)RepliesWithAudio / (double)_numberOfUsers;
        }

        public double AvgRepliesWithImage {
            get => (double)RepliesWithImage / (double)_numberOfUsers;
        }
        
        public int TotalReplies {
            get => RepliesTextOnly + RepliesWithVideo + RepliesWithAudio + RepliesWithImage;
        }
        
        public int NumberOfUsers {
            get => _numberOfUsers;
        }
    }
}
