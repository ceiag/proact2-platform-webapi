using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class LexiconLabelQueriesService : ILexiconLabelQueriesService {
        private readonly ProactDatabaseContext _database;

        public LexiconLabelQueriesService( ProactDatabaseContext database ) { 
            _database = database;
        }

        public LexiconLabel Create( LexiconCategory lexiconCategory, LexiconLabelCreationRequest request ) {
            return _database.LexiconLabels.Add( new LexiconLabel() {
                LexiconCategory = lexiconCategory,
                LexiconCategoryId = lexiconCategory.Id,
                GroupName = request.GroupName,
                Label = request.Label,
            } ).Entity;
        }

        public LexiconLabel Get( Guid lexiconLabelId ) {
            return _database.LexiconLabels.FirstOrDefault( x => x.Id == lexiconLabelId );
        }

        public LexiconLabel Update( Guid labelId, LexiconLabelUpdateRequest request ) {
            var label = Get( labelId );
            label.Label = request.Label;
            label.GroupName = request.GroupName;

            return label;
        }

        public List<LexiconLabel> GetAll( Guid lexiconCategoryId ) {
            return _database.LexiconLabels.Where( x => x.LexiconCategoryId == lexiconCategoryId ).ToList();
        }

        public LexiconLabel GetByName( Guid lexiconCategoryId, string label ) {
            return _database.LexiconLabels.FirstOrDefault(
                x => x.LexiconCategoryId == lexiconCategoryId && x.Label == label );
        }

        public void Delete( Guid labelId ) {
            _database.LexiconLabels.Remove( Get( labelId ) );
        }
    }
}
