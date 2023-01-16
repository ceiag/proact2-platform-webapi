using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.EntitiesMapper {
    public static class MessagesAttachmentMapper {
        public static MessageAttachmentModel Map( MessageAttachment messageAttachmentEntity ) {
            if ( messageAttachmentEntity != null ) {
                var messageAttachmentModel = new MessageAttachmentModel() {
                    AttachmentType = messageAttachmentEntity.AttachmentType,
                    ContentStatus = messageAttachmentEntity.ContentStatus,
                    DurationInMilliseconds = (int)messageAttachmentEntity.DurationInMilliseconds,
                    ThumbnailUrl = messageAttachmentEntity.ThumbnailUrl,
                    UploadedTime = messageAttachmentEntity.UploadedTime,
                    Url = messageAttachmentEntity.ContentUrl,
                    AssetId = messageAttachmentEntity.AssetId
                };

                return messageAttachmentModel;
            }
            else {
                return null;
            }
        }
    }
}
