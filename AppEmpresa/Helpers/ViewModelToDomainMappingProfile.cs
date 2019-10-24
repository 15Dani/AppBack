using AppEmpresa.Domain;
using AppEmpresa.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEmpresa.Helpers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<Enterprise, EnterpriseDTo>()
                 .ReverseMap();

        }
    }
}
