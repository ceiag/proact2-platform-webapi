using Proact.Services.Entities.MessageAnalysis;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class AnalysisModel {
        private List<AnalysisResultModel> _analysisResults;

        public AnalysisModel( List<AnalysisResultModel> results ) {
            _analysisResults = results;
        }

        public Guid AnalysisId { get; set; }
        public UserModel Author { get; set; }
        public bool IsMine { get; set; }
        public DateTime CreationDate { get; set; }
        public AnalysisState State { get; set; }
        public bool ResultsVisible { get; set; } = true;
        public List<AnalysisResultModel> Results {
            get {
                if ( ResultsVisible ) {
                    return _analysisResults;
                }
                else {
                    return new List<AnalysisResultModel>(); 
                }
            }
        }

        public List<AnalisysResultGroupByCategoryModel> ResultsGroupByCategories {
            get {
                if ( ResultsVisible ) {
                    return new AnalysisResultsGroupByCategoriesModel( _analysisResults ).Analysis;
                }
                else {
                    return new List<AnalisysResultGroupByCategoryModel>();
                }
            }
        }
    }
}
