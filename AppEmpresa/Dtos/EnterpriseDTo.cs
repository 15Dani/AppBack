using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEmpresa.Dtos
{
    public class EnterpriseDTo
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Cnpj { get; set; }

        public DateTime DataCadastro { get; set; }

        public string InscricaoEstadual { get; set; }

    }
}
