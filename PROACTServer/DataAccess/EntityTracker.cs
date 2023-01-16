using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public class EntityTracker {
        public void SetTrackInfo( 
            Guid userId, IEnumerable<IEntity> entities, EntityState state ) {
            
            foreach ( var entity in entities ) {    
                var trackableEntity = entity as TrackableEntity;
                
                if ( trackableEntity != null ) {
                    var currentDateTime = DateTime.UtcNow;

                    trackableEntity.LastModified = currentDateTime;
                    trackableEntity.ModifierId = userId;

                    if ( state == EntityState.Added ) {
                        trackableEntity.Created = currentDateTime;
                        trackableEntity.CreatorId = userId;
                    }
                }
            }
        }
    }
}
