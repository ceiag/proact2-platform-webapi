using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ILexiconCategoriesQueriesService : IQueriesService {
        public LexiconCategory Get( Guid categoryId );
        public List<LexiconCategory> GetAll( Lexicon lexicon );
        public LexiconCategory GetByName( Lexicon lexicon, string name );
        public void Add( Lexicon lexicon, LexiconCategoryAdditionRequest request );
        public LexiconCategory Update( Guid categoryId, LexiconCategoryUpdateRequest request );
        public void Delete( Guid categoryId );
        public void SetOrdering( LexiconCategorySetOrderingRequest request );
    }
}
