using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.Models {
    public class AnalysisResumeModel {
        public DateTime LastUpdate {
            get {
                if ( Analysis != null && Analysis.Count > 0 ) {
                    return Analysis.OrderBy( x => x.CreationDate )
                        .FirstOrDefault().CreationDate.ToUniversalTime();
                }
                else {
                    return DateTime.MinValue;
                }
            }
        }

        public UserModel LastUpdateBy {
            get {
                if ( Analysis != null && Analysis.Count > 0 ) {
                    return Analysis.OrderBy( x => x.CreationDate ).First().Author;
                }
                else {
                    return null;
                }
            }
        }

        public int AnalysisCount {
            get { return Analysis.Count; }
        }

        public List<AnalysisModel> Analysis { get; set; }
    }
}
