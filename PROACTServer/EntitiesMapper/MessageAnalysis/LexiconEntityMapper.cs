using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services.EntitiesMapper {
    public static class LexiconEntityMapper {
        public static List<LexiconModel> MapHideCategories( List<Lexicon> lexicons ) {
            var lexiconsModel = new List<LexiconModel>();

            foreach ( Lexicon lexicon in lexicons ) {
                lexiconsModel.Add( MapHideCategories( lexicon ) );
            }

            return lexiconsModel;
        }

        public static LexiconModel MapHideCategories( Lexicon lexicon ) {
            if ( lexicon == null ) 
                return null;

            lexicon.Categories.Clear();
            return Map( lexicon );
        }

        public static LexiconModel Map( Lexicon lexicon ) {
            return new LexiconModel() {
                Id = lexicon.Id,
                Name = lexicon.Name,
                Description = lexicon.Description,
                State = lexicon.State,
                Created = lexicon.Created,
                Categories = Map( lexicon.Categories )
            };
        }

        public static LexiconCategoryModel Map( LexiconCategory lexiconCategory ) {
            return new LexiconCategoryModel() {
                Id = lexiconCategory.Id,
                Name = lexiconCategory.Name,
                Order = lexiconCategory.Order,
                MultipleSelection = lexiconCategory.MultipleSelection,
                Labels = Map( lexiconCategory.Labels )
            };
        }

        public static List<LexiconCategoryModel> Map( List<LexiconCategory> lexiconCategories ) {
            var lexiconCategoryModels = new List<LexiconCategoryModel>();

            foreach ( LexiconCategory lexiconCategory in lexiconCategories ) {
                lexiconCategoryModels.Add( Map( lexiconCategory ) );
            }

            return lexiconCategoryModels;
        }

        public static LexiconLabelModel Map( LexiconLabel lexiconLabel ) {
            return new LexiconLabelModel() {
                Label = lexiconLabel.Label,
                GroupName = lexiconLabel.GroupName,
                Id = lexiconLabel.Id,
            };
        }

        public static List<LexiconLabelModel> Map( List<LexiconLabel> lexiconLabels ) {
            var lexiconLabelModels = new List<LexiconLabelModel>();

            foreach ( LexiconLabel lexiconLabel in lexiconLabels ) {
                lexiconLabelModels.Add( Map( lexiconLabel ) );
            }

            return lexiconLabelModels;
        }
    }
}
