using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Context;
using FaultReportingAPI.DAL.Repository.Abstract.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Net;

namespace FaultReportingAPI.DAL.Repository.Concrete.Base
{
    public class GenericRepository<TEntity>(AppDbContext context) : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context = context;
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        #region List
        public virtual async Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int pageNumber = 1, int pageSize = 10) where TResult : CoreBaseEntity
        {
            DataResult<CoreBaseEntity> response = new();
            try
            {
                IQueryable<TEntity> query = _dbSet;
                if (filter != null)
                    query = query.Where(filter);

                if (orderBy != null)
                    query = orderBy(query);
                // 🔹 Toplam kayıt (pagination öncesi)
                var totalCount = await query.CountAsync();

                // 🔹 Pagination
                query = query.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);
                if (selector != null)
                {
                    var entity = await query.Select(selector).ToListAsync();
                    response.ListData = entity.Cast<TResult>();
                }
                else
                {
                    var entity = await query.ToListAsync();
                    response.ListData = entity.Cast<TResult>();
                }
                response.Count = totalCount;
                response.StatusCode = !response.ListData.Any() ? HttpStatusCode.NotFound : HttpStatusCode.OK;
                response.Message = !response.ListData.Any() ? "Record not found." : "";
                response.Success = true;
            }
            catch (HttpRequestException ex)
            {
                response.StatusCode = ex.StatusCode;
                response.Message = "An error occurred while processing your request.";
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing your request.";
            }
            return response;
        }
        #endregion

        #region Single
        public async Task<DataResult<TEntity>> GetByIdAsync(long Id)
        {
            DataResult<TEntity> response = new();
            try
            {
                var entity = await _dbSet.Where(x => x.Id == Id && !x.IsDeleted/* && x.IsActive*/).FirstOrDefaultAsync();
                response.Data = entity;
                response.StatusCode = entity == null ? HttpStatusCode.NotFound : HttpStatusCode.OK;
                response.Message = entity == null ? "No record found" : "";
                response.Success = entity != null;
            }
            catch (HttpRequestException ex)
            {
                response.StatusCode = ex.StatusCode;
                response.Message = "An error occurred while processing your request.";
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing your request.";
            }
            return response;
        }

        public virtual async Task<DataResult<CoreBaseEntity>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null) where TResult : CoreBaseEntity
        {
            return await BaseGetByAsync(filter, selector);
        }
        protected virtual async Task<DataResult<CoreBaseEntity>> BaseGetByAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null) where TResult : CoreBaseEntity
        {
            DataResult<CoreBaseEntity> response = new();
            try
            {
                IQueryable<TEntity> query = _dbSet;
                if (filter != null)
                    query = query.Where(filter);

                if (selector != null)
                {
                    var entity = await query.Select(selector).ToListAsync();
                    response.Data = entity.Cast<TResult>().FirstOrDefault();
                    response.StatusCode = response.Data == null ? HttpStatusCode.NotFound : HttpStatusCode.OK;
                    response.Message = response.Data == null ? "Record not found." : "";
                }
                else
                {
                    var entity = await query.ToListAsync();
                    response.Data = entity.Cast<TResult>().FirstOrDefault();
                    response.StatusCode = response.Data == null ? HttpStatusCode.NotFound : HttpStatusCode.OK;
                    response.Message = response.Data == null ? "Record not found." : "";
                }
                response.Success = true;
            }
            catch (HttpRequestException ex)
            {
                response.StatusCode = ex.StatusCode;
                response.Message = "An error occurred while processing your request.";
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing your request.";

            }
            return response;
        }

        #endregion

        #region Crud
        public async Task<DataResult<TEntity>> CrudAsync(TEntity entity, EntityStateEnum entityState, Dictionary<string, object>? changedFields = null, long? userId = null)
        {
            var result = new DataResult<TEntity>();
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    {
                        DetachAllNavigations(entity);
                        var entry = _context.Entry(entity);

                        switch (entityState)
                        {
                            case EntityStateEnum.Insert:
                                entity.CreatedById = userId;
                                entry.State = EntityState.Added;
                                break;

                            case EntityStateEnum.Update:
                                entity.UpdatedDate = DateTime.Now;
                                entity.UpdatedById = userId;
                                if (changedFields != null && changedFields.Count != 0)
                                {
                                    entry.State = EntityState.Unchanged;

                                    foreach (var field in changedFields)
                                    {
                                        var prop = entry.Metadata.FindProperty(field.Key) ?? throw new ArgumentException($"Invalid field: {field.Key}");
                                        entry.Property(field.Key).CurrentValue = field.Value;
                                        entry.Property(field.Key).IsModified = true;
                                    }
                                    entry.Property(nameof(entity.UpdatedDate)).IsModified = true;
                                }
                                else
                                {
                                    _dbSet.Update(entity);
                                }
                                break;

                            case EntityStateEnum.Delete:
                                entity.DeletedDate = DateTime.Now;
                                entity.DeletedById = userId;
                                entity.IsDeleted = true;
                                _dbSet.Update(entity);
                                break;
                        }
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    result.StatusCode = HttpStatusCode.Created;
                    result.Success = true;
                    result.Data = entity;
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    result.StatusCode = HttpStatusCode.Conflict;
                    result.Message = "An error occurred while processing your request.";
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    result.StatusCode = HttpStatusCode.InternalServerError;
                    result.Message = "An error occurred while processing your request.";
                }
            });
            return result;
        }
        #endregion
        #region Any
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }
        #endregion

        #region Private Methods
        private void DetachAllNavigations(object entity)
        {
            var entry = _context.Entry(entity);

            foreach (var navigation in entry.Navigations)
            {
                if (navigation.CurrentValue == null)
                    continue;

                if (navigation.Metadata.IsCollection)
                {
                    if (navigation.CurrentValue is IEnumerable enumerable)
                    {
                        foreach (var child in enumerable)
                        {
                            var childEntry = _context.Entry(child);
                            if (childEntry.State != EntityState.Detached)
                            {
                                childEntry.State = EntityState.Detached;
                            }
                        }
                    }
                }
                else
                {
                    var navEntry = _context.Entry(navigation.CurrentValue);
                    if (navEntry.State != EntityState.Detached)
                    {
                        navEntry.State = EntityState.Detached;
                    }
                }
            }
        }
        #endregion

    }
}
