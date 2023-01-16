using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.Models {
    public class AnalisysResultGroupByCategoryModel {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Order { get; set; }
        public List<AnalysisResultModel> Results { get; set; }

        public AnalisysResultGroupByCategoryModel( AnalysisResultModel analisysResult ) {
            CategoryId = analisysResult.CategoryId;
            CategoryName = analisysResult.CategoryName;
            Order = analisysResult.Order;
            Results = new List<AnalysisResultModel>() { analisysResult };
        }
    }

    public class AnalysisResultsGroupByCategoriesModel {
        private List<AnalisysResultGroupByCategoryModel> _analysis;
        public List<AnalisysResultGroupByCategoryModel> Analysis { 
            get { 
                var orderedList = _analysis.OrderBy( x => x.Order ).ToList();
                return orderedList;
            }
        }

        public AnalysisResultsGroupByCategoriesModel( List<AnalysisResultModel> AnalysisResults ) {
            _analysis = new List<AnalisysResultGroupByCategoryModel>();

            foreach ( var resultItem in AnalysisResults ) {
                if ( IsCategoryPresent( resultItem ) ) {
                    AddCategoryToList( resultItem );
                }
                else {
                    CreateNewCategoryItemAndAddItem( resultItem );
                }
            }

            foreach ( var item in _analysis ) {
                item.Results = item.Results.OrderBy( x => x.Order ).ToList();
            }
        }

        private bool IsCategoryPresent( AnalysisResultModel resultItem ) {
            return _analysis.Any( x => x.CategoryName == resultItem.CategoryName );
        }

        private void AddCategoryToList( AnalysisResultModel resultItem ) {
            var categoyGroup = _analysis.FirstOrDefault( x => x.CategoryName == resultItem.CategoryName );
            categoyGroup.Results.Add( resultItem );
        }

        private void CreateNewCategoryItemAndAddItem( AnalysisResultModel resultItem ) {
            _analysis.Add( new AnalisysResultGroupByCategoryModel( resultItem ) );
        }
    }
}
