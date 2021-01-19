using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

namespace aiof.messaging.data
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<IMessage, IEmailMessage>()
                .ForMember(x => x.From, o => o.MapFrom(s => s.From))
                .ForMember(x => x.To, o => o.MapFrom(s => s.To))
                .ForMember(x => x.Subject, o => o.MapFrom(s => s.Subject))
                .ForMember(x => x.Cc, o => o.MapFrom(s => string.Join(",", s.Cc)))
                .ForMember(x => x.Bcc, o => o.MapFrom(s => string.Join(",", s.Bcc)));
        }
    }
}
