using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System.Collections.Generic;

namespace Proact.Services.Tests.Shared {
    public static class LexiconSnapshotCreator {
        public static DatabaseSnapshotProvider AddLexiconWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Institute institute, out Lexicon lexicon ) {

            lexicon = snapshotProvider.ServiceProvider
                .GetQueriesService<ILexiconQueriesService>()
                .Create( institute.Id, GetLexiconWithRandomCreationRequest() );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static LexiconCreationRequest GetLexiconWithRandomCreationRequest() {
            return new LexiconCreationRequest() {
                Name = "lexicon_name",
                Description = "lexicon_description",
                Categories = new List<LexiconCategoryCreationRequest>() {
                        new LexiconCategoryCreationRequest {
                            Name = "symptoms",
                            Description = "symptoms description",
                            MultipleSelection = false,
                            Labels = new List<LexiconLabelCreationRequest>() {
                                new LexiconLabelCreationRequest() {
                                    Label = "headache",
                                    GroupName = "head"
                                },
                                new LexiconLabelCreationRequest() {
                                    Label = "stomac pain",
                                    GroupName = "stomac"
                                },
                                new LexiconLabelCreationRequest() {
                                    Label = "rush",
                                    GroupName = "skin"
                                },
                            }
                        },
                        new LexiconCategoryCreationRequest {
                            Name = "grade",
                            Description = "grade description",
                            MultipleSelection = false,
                            Labels = new List<LexiconLabelCreationRequest>() {
                                new LexiconLabelCreationRequest() {
                                    Label = "grade 1",
                                },
                                new LexiconLabelCreationRequest() {
                                    Label = "grade 2",
                                },
                                new LexiconLabelCreationRequest() {
                                    Label = "grade 3",
                                },
                            }
                        },
                        new LexiconCategoryCreationRequest {
                            Name = "action",
                            Description = "action description",
                            MultipleSelection = false,
                            Labels = new List<LexiconLabelCreationRequest>() {
                                new LexiconLabelCreationRequest() {
                                    Label = "action 1",
                                },
                                new LexiconLabelCreationRequest() {
                                    Label = "action 2",
                                },
                                new LexiconLabelCreationRequest() {
                                    Label = "action 3",
                                },
                            }
                        }
                    }
            };
        }
    }
}
