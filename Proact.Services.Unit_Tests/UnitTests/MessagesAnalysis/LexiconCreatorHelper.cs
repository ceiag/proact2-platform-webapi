using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System.Collections.Generic;

namespace Proact.Services.UnitTests.MessageAnalysis {
    public static class LexiconCreatorHelper {
        public static LexiconCreationRequest GetDummyLexiconCreationRequest() {
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

        public static Lexicon CreateDummyLexicon( MockDatabaseUnitTestHelper mockHelper ) {
            var lexiconCreated = mockHelper.ServicesProvider
                .GetQueriesService<ILexiconQueriesService>()
                .Create( GetDummyLexiconCreationRequest() );

            mockHelper.ServicesProvider.SaveChanges();

            return lexiconCreated;
        }

        public static AnalysisCreationRequest GetDummyAnalysisCreationRequest(
            MockDatabaseUnitTestHelper mockHelper, MessageModel message ) {
            var lexicon = CreateDummyLexicon( mockHelper );

            return new AnalysisCreationRequest() {
                MessageId = message.MessageId,
                AnalysisResults = new List<AnalysisResultCreationRequest>() {
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[0].Labels[0].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[1].Labels[0].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[2].Labels[0].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[2].Labels[1].Id,
                        }
                    }
            };
        }
    }
}
