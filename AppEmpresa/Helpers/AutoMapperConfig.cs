using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEmpresa.Helpers
{
    [Obsolete]
    public class AutoMapperConfig
    { 
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });

        }
    }
}

