using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services {
    public static class DbLexiconValidityChecker {
        public static ConsistencyRulesHelper IfLexiconNameIsAvailable(
           this ConsistencyRulesHelper rulesHelper, string name ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<ILexiconQueriesService>()
                        .GetByName( name ) == null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new ConflictObjectResult( $"{name} is already taken!");
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconIsValid(
           this ConsistencyRulesHelper rulesHelper, Guid lexiconId, out Lexicon lexicon ) {
            Lexicon lexiconResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    lexiconResult = rulesHelper
                        .GetQueriesService<ILexiconQueriesService>().Get( lexiconId );

                    return lexiconResult != null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"Lexicon with id: {lexiconId} not found!" );
                } );

            lexicon = lexiconResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconCategoryIsValid(
           this ConsistencyRulesHelper rulesHelper, Guid categoryId, out LexiconCategory category ) {
            LexiconCategory categoryResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    categoryResult = rulesHelper
                        .GetQueriesService<ILexiconCategoriesQueriesService>()
                        .Get( categoryId );

                    return categoryResult != null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( "" );
                } );

            category = categoryResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconLabelIsValid(
           this ConsistencyRulesHelper rulesHelper, Guid labelId, out LexiconLabel label ) {
            LexiconLabel labelResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    labelResult = rulesHelper
                        .GetQueriesService<ILexiconLabelQueriesService>()
                        .Get( labelId );

                    return labelResult != null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( "" );
                } );

            label = labelResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconCategoryNameIsAvailable(
           this ConsistencyRulesHelper rulesHelper, Lexicon lexicon, string name ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<ILexiconCategoriesQueriesService>()
                        .GetByName( lexicon, name ) == null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"{name} is already taken!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconIsNotPublished(
           this ConsistencyRulesHelper rulesHelper, Guid lexiconId ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<ILexiconQueriesService>()
                        .Get( lexiconId ).State == LexiconState.DRAW;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"You can not modify a published Lexicon" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconLabelNameIsAvailable(
           this ConsistencyRulesHelper rulesHelper, Guid categoryId, string label ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<ILexiconLabelQueriesService>()
                        .GetByName( categoryId, label ) == null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"{label} is already taken!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconIsInMyInstitute(
            this ConsistencyRulesHelper rulesHelper, Guid myInstituteId, Guid lexiconId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var lexicon = rulesHelper.GetQueriesService<ILexiconQueriesService>().Get( lexiconId );

                    return lexicon.InstituteId == myInstituteId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"lexicon {lexiconId} is not into institute {myInstituteId}" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfLexiconLabelNameIsAvailableForUpdate(
           this ConsistencyRulesHelper rulesHelper, Guid categoryId, Guid labelId, string label ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var labelResult = rulesHelper
                        .GetQueriesService<ILexiconLabelQueriesService>()
                        .GetByName( categoryId, label );

                    if ( labelResult == null ) {
                        return true;
                    }

                    if ( labelResult.Id == labelId && labelResult.Label == label ) {
                        return true;
                    }

                    return false;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"{label} is already taken!" );
                } );

            return validityChecker;
        }
    }
}
