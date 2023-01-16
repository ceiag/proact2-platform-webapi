using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models.Exportations;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.Exporters;

public class CsvFormatAnalysisExporter : IAnalysisExporter {
    public AnalysisExportResult Export( Lexicon lexicon, IEnumerable<Analysis> analysis ) {
        string csvResult = "MessageId;Emotion;MessageScope;Message Created At;" +
                           "Analysis Created At;Author Name;";

        foreach ( var category in lexicon
            .Categories
            .OrderBy( x => x.Order ) ) {
            csvResult += $"{category.Name};";
        }

        csvResult += "\n";

        foreach ( var analysisItem in analysis ) {
            csvResult +=
                $"{analysisItem.Message.Id};" +
                $"{analysisItem.Message.Emotion};" +
                $"{analysisItem.Message.MessageScope};" +
                $"{analysisItem.Message.Created};" +
                $"{analysisItem.Created};" +
                $"{analysisItem.User.Name};";

            foreach ( var category in lexicon.Categories ) {
                var analysisResultsInsideCategory = analysisItem
                    .AnalysisResults
                    .Where( x => x.LexiconLabel.LexiconCategory.Name == category.Name );

                int i = 0;
                foreach ( var analisysResult in analysisResultsInsideCategory ) {
                    csvResult += $"{analisysResult.LexiconLabel.Label}";

                    if ( i < analysisResultsInsideCategory.Count() - 1 )
                        csvResult += ", ";

                    ++i;
                }

                csvResult += ";";
            }

            csvResult += "\n";
        }

        return new AnalysisExportResult( "csv", csvResult );
    }
}
