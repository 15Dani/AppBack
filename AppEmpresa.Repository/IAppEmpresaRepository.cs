using AppEmpresa.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEmpresa.Repository
{
   public interface IAppEmpresaRepository
    {
     
        void Adicionar<T>(T entity) where T : class;
        void Atualizar<T>(T entity) where T : class;
        void Deletar<T>(T entity) where T : class;

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Enterprise>> ObterTodosEnterprisePorNomeAsync(string nome);
        Task<IEnumerable<Enterprise>> ObterTodosEnterpriseAsync();
        Task<Enterprise> ObterEnterprisePorIdAsync(int id);
     
    }
}