using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppEmpresa.Domain
{
    public class Enterprise
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Nome Obrigatório!")]
        [StringLength(100, ErrorMessage = " O Limite para Inserir o Nome é de 100 Caracteres")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required(ErrorMessage = "Campo Cnpj Obrigatório!")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Campo Data é Obrigatório! !")]
        public DateTime DataCadastro { get; set; }

        [Required(ErrorMessage = "A Inscrição Estadual deve ser Preenchido!")]
        public string InscricaoEstadual { get; set; }




    }

}

    
    

