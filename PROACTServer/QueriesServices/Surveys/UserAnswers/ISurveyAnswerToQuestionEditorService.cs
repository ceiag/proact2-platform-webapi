using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface ISurveyAnswerToQuestionEditorService : IDataEditorService {
        public SurveyCompiledModel GetCompiledSurveyFromPatient( Guid assegnationId );
        public void SetCompiledSurveyFromPatient( Guid assegnationId, SurveyCompileRequest compileRequest );
    }
}
