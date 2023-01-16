using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.EntitiesMapper {
    public static class AnalysisEntityMapper {
        public static List<AnalysisModel> Map( List<Analysis> analysisList, Guid requestUserId ) {
            var analysisModel = new List<AnalysisModel>();

            foreach ( var analysis in analysisList ) {
                analysisModel.Add( Map( analysis, requestUserId ) );
            }

            return analysisModel;
        }

        public static AnalysisModel Map( Analysis analysis, Guid requestUserId ) {
            return new AnalysisModel( Map( analysis.AnalysisResults ) ) {
                AnalysisId = analysis.Id,
                Author = UserEntityMapper.Map( analysis.User ),
                CreationDate = analysis.Created.ToUniversalTime(),
                State = analysis.State,
                IsMine = requestUserId == analysis.UserId
            };
        }

        public static List<AnalysisResultModel> Map( List<AnalysisResult> analysisResults ) {
            var reviewResultModels = new List<AnalysisResultModel>();

            foreach ( var analysisResult in analysisResults ) {
                reviewResultModels.Add( Map( analysisResult ) );
            }

            return reviewResultModels;
        }

        public static AnalysisResultModel Map( AnalysisResult analysisResult ) {
            return new AnalysisResultModel() {
                CategoryName = analysisResult.LexiconLabel.LexiconCategory.Name,
                ResultLabel = analysisResult.LexiconLabel.Label,
                Order = analysisResult.LexiconLabel.LexiconCategory.Order,
                LabelId = analysisResult.LexiconLabelId,
                CategoryId = analysisResult.LexiconLabel.LexiconCategory.Id
            };
        }

        public static AnalysisResumeModel ToAnalysisResume( 
            List<Analysis> analysis, Guid requesterUserId ) {
            var analysisResume = new AnalysisResumeModel() {
                Analysis = Map( analysis, requesterUserId )
                    .Where( x => x.State == AnalysisState.Current )
                    .OrderByDescending( x => x.CreationDate )
                    .ToList()
            };

            return analysisResume;
        }

        public static AnalysisResumeModel ToAnalysisResumeOnlyMine( 
            List<Analysis> analysis, Guid authorUserId ) {
            var analysisResume = ToAnalysisResume( analysis, authorUserId );

            foreach ( var analysisItem in analysisResume.Analysis ) {
                if ( analysisItem.Author.UserId != authorUserId ) {
                    analysisItem.ResultsVisible = false;
                }
            }

            return analysisResume;
        }
    }
}
