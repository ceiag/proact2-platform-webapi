using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;

namespace Proact.Services.Tests.Shared {
    public static class MessageAnalysisSnapshotCreator {
        public static DatabaseSnapshotProvider AddAnalysisWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider,
            Medic medic, Guid messageId, Lexicon lexicon, out Analysis analysis ) {

            var analysisCreationRequest = new AnalysisCreationRequest() {
                MessageId = messageId,
                AnalysisResults = new List<AnalysisResultCreationRequest>() {
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[0].Labels[1].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[1].Labels[1].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[2].Labels[1].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[2].Labels[2].Id,
                        }
                    }
            };

            analysis = snapshotProvider.ServiceProvider
                .GetQueriesService<IMessageAnalysisQueriesService>()
                .Create( medic.UserId, analysisCreationRequest );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
