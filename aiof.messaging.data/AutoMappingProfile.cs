﻿using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using Newtonsoft.Json;

namespace aiof.messaging.data
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<IMessage, IEmailMessage>()
                .ForMember(x => x.PublicKey, o => o.MapFrom(s => s.PublicKey))
                .ForMember(x => x.From, o => o.MapFrom(s => s.From))
                .ForMember(x => x.To, o => o.MapFrom(s => s.To))
                .ForMember(x => x.Subject, o => o.MapFrom(s => s.Subject))
                .ForMember(x => x.Cc, o => o.MapFrom(s => string.Join(",", s.Cc)))
                .ForMember(x => x.Bcc, o => o.MapFrom(s => string.Join(",", s.Bcc)))
                .ForMember(x => x.IsBodyHtml, o => o.MapFrom(s => s.IsBodyHtml))
                .ForMember(x => x.Body, o => o.MapFrom(s => s.Body));

            CreateMap<IMessage, MessageEntity>()
                .ForMember(x => x.PartitionKey, o => o.MapFrom(s => s.Type.ToLowerInvariant()))
                .ForMember(x => x.RowKey, o => o.MapFrom(s => s.PublicKey.ToString()))
                .ForMember(x => x.PublicKey, o => o.MapFrom(s => s.PublicKey))
                .ForMember(x => x.Type, o => o.MapFrom(s => s.Type))
                .ForMember(x => x.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(x => x.Created, o => o.MapFrom(s => s.Created))
                .ForMember(x => x.Raw, o => o.MapFrom(s => JsonConvert.SerializeObject(s)))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<IMessage, MessageDeadLetterEntity>()
                .ForMember(x => x.RowKey, o => o.MapFrom(s => s.PublicKey.ToString().ToLowerInvariant()))
                .ForMember(x => x.PublicKey, o => o.MapFrom(s => s.PublicKey))
                .ForMember(x => x.Type, o => o.MapFrom(s => s.Type))
                .ForMember(x => x.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(x => x.Created, o => o.MapFrom(s => s.Created))
                .ForMember(x => x.IsTest, o => o.MapFrom(s => s.TestConfig != null))
                .ForMember(x => x.Raw, o => o.MapFrom(s => JsonConvert.SerializeObject(s)))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<IEmailMessage, EmailMessageEntity>()
                .ForMember(x => x.From, o => o.MapFrom(s => s.From))
                .ForMember(x => x.To, o => o.MapFrom(s => s.To))
                .ForMember(x => x.Subject, o => o.MapFrom(s => s.Subject))
                .ForMember(x => x.Cc, o => o.MapFrom(s => s.Cc))
                .ForMember(x => x.Bcc, o => o.MapFrom(s => s.Bcc))
                .ForMember(x => x.IsBodyHtml, o => o.MapFrom(s => s.IsBodyHtml))
                .ForMember(x => x.Body, o => o.MapFrom(s => s.Body))
                .ForMember(x => x.Raw, o => o.MapFrom(s => JsonConvert.SerializeObject(s)));
        }
    }
}
