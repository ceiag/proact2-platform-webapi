using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class MessageAnalysisQueriesService : IMessageAnalysisQueriesService {
        private readonly ProactDatabaseContext _database;

        public MessageAnalysisQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Analysis Get( Guid analysisId ) {
            return _database.Analysis
                .Include( x => x.AnalysisResults )
                    .ThenInclude( x => x.LexiconLabel )
                    .ThenInclude( x => x.LexiconCategory )
                .Include( x => x.Message )
                    .ThenInclude( x => x.Author )
                    .ThenInclude( x => x.Institute )
                .FirstOrDefault( x => x.Id == analysisId );
        }

        public IEnumerable<Analysis> GetAllAnalysisFromPatient( Guid userId ) {
            return _database.Analysis
                .AsNoTracking()
                .Include( x => x.AnalysisResults )
                    .ThenInclude( x => x.LexiconLabel )
                    .ThenInclude( x => x.LexiconCategory.Lexicon )
                .Include( x => x.Message )
                    .ThenInclude( x => x.Author )
                .Include( x => x.User )
                .Where( x => x.State == AnalysisState.Current )
                .Where( x => x.Message.OriginalMessageId == Guid.Empty )
                .Where( x => x.Message.AuthorId == userId );
        }

        public Analysis Create( Guid authorUserId, AnalysisCreationRequest request ) {
            var analysis = new Analysis() {
                Id = Guid.NewGuid(),
                MessageId = request.MessageId,
                UserId = authorUserId,
            };

            foreach ( var analysisResultValue in request.AnalysisResults ) {
                analysis.AnalysisResults.Add( new AnalysisResult() {
                    Id = Guid.NewGuid(),
                    LexiconLabelId = analysisResultValue.LabelId,
                } );
            }

            return _database.Analysis.Add( analysis ).Entity;
        }

        public Analysis Update( Guid analysisId, Guid authorUserId, AnalysisCreationRequest request ) {
            Deprecate( analysisId );

            return Create( authorUserId, request );
        }

        public void Deprecate( Guid analysisId ) {
            Get( analysisId ).State = AnalysisState.Deprecated;
        }

        public void Delete( Guid analysisId ) {
            Get( analysisId ).State = AnalysisState.Deleted;
        }
    }
}
