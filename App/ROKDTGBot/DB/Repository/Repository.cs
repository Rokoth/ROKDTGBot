using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;
using ROTGBot.Db.Context;
using System.Linq.Expressions;

namespace ROTGBot.Db.Repository
{
    /// <summary>
    /// Repository - wrapper for db works
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class, IEntity 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<Repository<T>>>();
        }

        /// <summary>
        /// Метод добавления модели в базу
        /// </summary>
        /// <param name="entity">модель</param>
        /// <param name="withSave">с сохраннеием</param>
        /// <param name="token">токен</param>
        /// <returns>модель</returns>
        public async Task<T> AddAsync(T entity, bool withSave, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                var item = context.Set<T>().Add(entity).Entity;
                await Task.CompletedTask;
                return item;
            }, "AddAsync", withSave);
        }

        /// <summary>
        /// Метод удаления из базы
        /// </summary>
        /// <param name="entity">модель</param>
        /// <param name="withSave">с сохраннеием</param>
        /// <param name="token">токен</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync(T entity, bool withSave, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                entity.IsDeleted = true;
                var item = context.Set<T>().Update(entity).Entity;                
                await Task.CompletedTask;
                return item;
            }, "DeleteAsync", withSave);
        }

        /// <summary>
        /// Метод получения отфильтрованного списка моделей с постраничной отдачей
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="token">токен</param>
        /// <returns>список моделей</returns>
        public async Task<List<T>> GetAsync(Filter<T> filter, CancellationToken token)
        {
            return await GetAsyncInternal(filter, false, "GetAsync");
        }

        
        private async Task<List<T>> GetAsyncInternal(Filter<T> filter, bool withDeleted, string methodName)
        {
            return await ExecuteAsync(async (context) =>
            {
                Expression<Func<T, bool>> defaultSelector = s => true;

                var all = context.Set<T>().Where(filter.Selector ?? defaultSelector);

                if (!withDeleted) all = all.Where(s => !s.IsDeleted);
                if (!string.IsNullOrEmpty(filter.Sort))
                {
                    all = all.OrderBy(filter.Sort);
                }
                var count = await all.CountAsync();
                List<T> result;
                if (filter.Size.HasValue)
                {
                    result = await all
                        .Skip(filter.Size.Value * filter.Page ?? 0)
                        .Take(filter.Size.Value)
                        .ToListAsync();                    
                }
                else
                {
                    result = await all.ToListAsync();
                }
                return result;
            }, methodName, false);
        }

        /// <summary>
        /// Метод получения модели по id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public async Task<T> GetAsync(Guid id, CancellationToken token)
        {
            var result =  await ExecuteAsync(async (context) => {
                return await context.Set<T>()
                    .Where(s => !s.IsDeleted && s.Id == id).FirstOrDefaultAsync();
            }, "GetAsync", false);

            return result ?? throw new RepositoryException("Не удалось получить Entity по Id");
        }



        /// <summary>
        /// Метод получения модели по id (в тч удаленные)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public async Task<T> GetAsyncDeleted(Guid id, CancellationToken token)
        {
            var result = await ExecuteAsync(async (context) => {
                return await context.Set<T>()
                    .Where(s => s.Id == id).FirstOrDefaultAsync();
            }, "GetAsync", false);

            return result ?? throw new RepositoryException("Не удалось получить Entity по Id");
        }

        /// <summary>
        /// Метод обновления записи в базе
        /// </summary>
        /// <param name="entity">модель</param>
        /// <param name="withSave">с сохраннеием</param>
        /// <param name="token">токен</param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T entity, bool withSave, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {                
                var item = context.Set<T>().Update(entity).Entity;
                await Task.CompletedTask;
                return item;
            }, "UpdateAsync", withSave);
        }

        /// <summary>
        /// Обертка выполнения запросов (обработка ошибок)
        /// </summary>
        /// <typeparam name="TEx"></typeparam>
        /// <param name="action"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private async Task<TEx> ExecuteAsync<TEx>(Func<DbPgContext, Task<TEx>> action, string method, bool withSave)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<DbPgContext>();
                var result = await action(context);
                if (withSave) await context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе {method} Repository: {ex.Message} {ex.StackTrace}");               
                throw new RepositoryException($"Ошибка в методе {method} Repository: {ex.Message}");
            }
        }

        public async Task SaveChangesAsync()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            await context.SaveChangesAsync();
        }
    }
}
