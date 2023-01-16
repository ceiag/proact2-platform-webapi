using AutoMapper;
using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proact.EncryptionAgentService.Configurations {
    public static class AutoMapperConfiguration {

        public static IMapper Mapper {
            get {
                return _mapperConfiguration.CreateMapper();
            }
        }

        private static MapperConfiguration _mapperConfiguration;

        public static void Configure() {
            _mapperConfiguration = new MapperConfiguration( cfg => {
                ConfigureMessagesMapper( cfg );
            } );
        }

        private static void ConfigureMessagesMapper( 
            IMapperConfigurationExpression mapperConfigurationExpression ) {
            mapperConfigurationExpression.CreateMap<MessageData, MessageDataModel>()
                .ForMember( m => m.MessageId, o => o.MapFrom( source => source.Id ) );

            mapperConfigurationExpression.CreateMap<CreateMessageDataRequest, MessageData>()
             .ForMember( m => m.Id, o => o.MapFrom( source => source.MessageId ) );
        }
    }
}
