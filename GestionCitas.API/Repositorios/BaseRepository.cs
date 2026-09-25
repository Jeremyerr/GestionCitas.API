using GestionCitas.API.Data;
using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCitas.API.Repositorios
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
    {
        // Cambiamos ApplicationDbContext por AppDbContext
        protected readonly AppDbContext _dbcontext;
        protected DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
            _dbSet = _dbcontext.Set<TEntity>();
        }

        public async Task<bool> AnyById(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            if (!await AnyById(id))
            {
                return false;
            }

            var entity = await GetById(id);
            _dbSet.Remove(entity);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<PaginacionResult<TEntity>> GetAsync(int skip, int take)
        {
            var result = await _dbSet.Skip(skip).Take(take).ToListAsync();
            var total = await _dbSet.CountAsync();

            return new PaginacionResult<TEntity>
            {
                Result = result,
                Total = total
            };
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var queriable = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                queriable = queriable.Include(include);
            }
            return await queriable.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}