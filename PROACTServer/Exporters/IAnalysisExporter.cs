using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models.Exportations;
using System.Collections.Generic;

namespace Proact.Services.Exporters;

public interface IAnalysisExporter {
    public AnalysisExportResult Export( Lexicon lexicon, IEnumerable<Analysis> analysis );
}
