using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class LexiconCategoriesQueriesService : ILexiconCategoriesQueriesService {
        public ProactDatabaseContext _database { get; set; }

        public LexiconCategoriesQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public List<LexiconCategory> GetAll( Lexicon lexicon ) {
            return lexicon.Categories;
        }

        public LexiconCategory Get( Guid categoryId ) {
            return _database.LexiconCategories.FirstOrDefault( x => x.Id == categoryId );
        }

        public LexiconCategory Update( Guid categoryId, LexiconCategoryUpdateRequest request ) { 
            var category = Get( categoryId );

            category.Name = request.Name;
            category.MultipleSelection = request.MultipleSelection;

            return category;
        }

        public LexiconCategory GetByName( Lexicon lexicon, string name ) {
            return lexicon.Categories.FirstOrDefault( x => x.Name == name );
        }

        public void Add( Lexicon lexicon, LexiconCategoryAdditionRequest request ) {
            _database.LexiconCategories.Add( new LexiconCategory() {
                Name = request.Name,
                MultipleSelection = request.MultipleSelection,
                LexiconId = lexicon.Id,
            } );
        }

        public void Delete( Guid categoryId ) {
            _database.LexiconCategories.Remove( Get( categoryId ) );
        }

        public void SetOrdering( LexiconCategorySetOrderingRequest request ) {
            for ( int i = 0; i < request.OrderedCategories.Count; ++i ) {
                Get( request.OrderedCategories[i] ).Order = i;
            }
        }
    }
}
