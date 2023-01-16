using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.EntitiesMapper {
    public static class ProjectPropertiesEntityMapper {
        public static ProjectPropertiesModel Map( ProjectProperties projectProperties ) {
            if ( projectProperties == null ) return null;

            return new ProjectPropertiesModel() {
                Lexicon = LexiconEntityMapper.MapHideCategories( projectProperties.Lexicon ),
                MedicsCanSeeOtherAnalisys = projectProperties.MedicsCanSeeOtherAnalisys,
                MessageCanBeAnalizedAfterMinutes = projectProperties.MessageCanBeAnalizedAfterMinutes,
                MessageCanNotBeDeletedAfterMinutes = projectProperties.MessageCanNotBeDeletedAfterMinutes,
                MessageCanBeRepliedAfterMinutes = projectProperties.MessageCanBeRepliedAfterMinutes,
                IsAnalystConsoleActive = projectProperties.IsAnalystConsoleActive,
                IsSurveysSystemActive = projectProperties.IsSurveysSystemActive,
                IsMessagingActive = projectProperties.IsMessagingActive
            };
        }
    }
}
