using Proact.Services.Models.Stats;
using System;
using Xunit;

namespace Proact.Services.UnitTests.Stats {
    public class PatientMessagesStatsModel_UTest {
        [Fact]
        public void _CheckStatsCorrectness() {
            var stats = new PatientMessagesStatsModel( 10 ) {
                TopicsTextOnly = 1,
                RepliesTextOnly = 2,
                TopicsWithVideo = 3,
                TopicsWithAudio = 4,
                TopicsWithImage = 5,
                RepliesWithVideo = 6,
                RepliesWithAudio = 7,
                RepliesWithImage = 8,
                AvgTopicsWithVideoDuration = 9,
                AvgTopicsWithAudioDuration = 10,
                AvgRepliesWithVideoDuration = 11,
                AvgRepliesWithAudioDuration = 12,
                AvgTopicsTextLength = 13,
                AvgRepliesTextLength = 14,
                UnrepliedTextOnly = 15,
                UnrepliedWithVideo = 16,
                UnrepliedWithAudio = 17,
                UnrepliedWithImage = 18,
            };

            Assert.Equal( 36, stats.TotalMessages );
            Assert.Equal( 13, stats.TotalTopics );
            Assert.Equal( 23, stats.TotalReplies );
            Assert.Equal( 66, stats.TotalUnreplied );
            Assert.Equal( Math.Round( 0.1f, 1 ), Math.Round( stats.AvgTopicsTextOnly, 1 ) );
            Assert.Equal( Math.Round( 0.2f, 1 ), Math.Round( stats.AvgRepliesTextOnly, 1 ) );
            Assert.Equal( Math.Round( 0.3f, 1 ), Math.Round( stats.AvgTopicsWithVideo, 1 ) );
            Assert.Equal( Math.Round( 0.4f, 1 ), Math.Round( stats.AvgTopicsWithAudio, 1 ) );
            Assert.Equal( Math.Round( 0.5f, 1 ), Math.Round( stats.AvgTopicsWithImage, 1 ) );
            Assert.Equal( Math.Round( 1.5f, 1 ), Math.Round( stats.AvgUnrepliedTextOnly, 1 ) );
            Assert.Equal( Math.Round( 1.6f, 1 ), Math.Round( stats.AvgUnrepliedWithVideo, 1 ) );
            Assert.Equal( Math.Round( 1.7f, 1 ), Math.Round( stats.AvgUnrepliedWithAudio, 1 ) );
            Assert.Equal( Math.Round( 1.8f, 1 ), Math.Round( stats.AvgUnrepliedWithImage, 1 ) );
        }
    }
}
