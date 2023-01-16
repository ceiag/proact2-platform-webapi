using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class LexiconQueriesService : ILexiconQueriesService {
        public ProactDatabaseContext _database { get; set; }

        public LexiconQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Lexicon Create( Guid instituteId, LexiconCreationRequest request ) {
            var lexicon = new Lexicon() {
                Name = request.Name,
                Description = request.Description,
                InstituteId = instituteId
            };

            int i = 0;
            foreach ( var category in request.Categories ) {
                var newCategory = new LexiconCategory() {
                    Name = category.Name,
                    MultipleSelection = category.MultipleSelection,
                    Order = i
                };

                ++i;

                foreach ( var label in category.Labels ) {
                    newCategory.Labels.Add( new LexiconLabel() {
                        Label = label.Label,
                        GroupName = label.GroupName
                    } );
                }

                lexicon.Categories.Add( newCategory );
            }

            _database.Lexicons.Add( lexicon );

            return lexicon;
        }

        public Lexicon Get( Guid lexiconId ) {
            return _database.Lexicons
                .Include( x => x.Categories )
                .ThenInclude( x => x.Labels.OrderBy( x => x.Label ) )
                .FirstOrDefault( x => x.Id == lexiconId );
        }

        public List<Lexicon> GetAll( Guid instituteId ) {
            return _database.Lexicons
                .Include( x => x.Categories )
                .ThenInclude( x => x.Labels.OrderBy( x => x.Label ) )
                .OrderByDescending( x => x.Created )
                .Where( x => x.InstituteId == instituteId )
                .ToList();
        }

        public Lexicon GetByName( string name ) {
            return _database.Lexicons
                 .Include( x => x.Categories )
                 .ThenInclude( x => x.Labels.OrderBy( x => x.Label ) )
                 .FirstOrDefault( x => x.Name == name );
        }

        public void PublishLexicon( Guid lexiconId ) {
            Get( lexiconId ).State = LexiconState.PUBLISHED;
        }

        public void Delete( Guid lexiconId ) {
            _database.Lexicons.Remove( Get( lexiconId ) );
        }
    }
}
