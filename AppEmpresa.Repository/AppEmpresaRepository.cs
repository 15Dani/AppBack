using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEmpresa.Domain;
using Microsoft.EntityFrameworkCore;

namespace AppEmpresa.Repository
{
    public class AppEmpresaRepository : IAppEmpresaRepository
    {
        private readonly DataContext _context;

        public AppEmpresaRepository(DataContext context)
        {
            _context = context;

        }

        public void Adicionar<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Atualizar<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Deletar<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        //Enterprise por id
        public async Task<Enterprise> ObterEnterprisePorIdAsync(int id)
        {

            return  await _context.Enterprises.FindAsync(id);
           

        }

        //Enterprise retornar todos.
        public async Task<IEnumerable<Enterprise>> ObterTodosEnterpriseAsync()
        {
          
            return await _context.Enterprises.OrderBy(c => c.Id).ToListAsync();
        }

        //Enterprise retorna por nome
        public async Task<IEnumerable<Enterprise>> ObterTodosEnterprisePorNomeAsync(string nome)
        {
            IQueryable<Enterprise> query = _context.Enterprises;

            query = query.AsNoTracking()                        
                        .Where(e => e.Nome.ToLower().Contains(nome.ToLower()))
                        .OrderByDescending(d => d.DataCadastro);

            return await query.ToArrayAsync();

            
        }

    }
  } 

      
       
       



       