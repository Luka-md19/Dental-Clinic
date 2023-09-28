using AutoMapper;
using AutoMapper.QueryableExtensions;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DentalClinicDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(DentalClinicDbContext context , IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
        
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            T entity = _mapper.Map<T>(source);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TResult>(entity);
        }


        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _context.Set<T>().ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var query = _context.Set<T>().AsQueryable();

            // Handle search, if available
            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                query = query.Where(item => EF.Property<string>(item, "Name").Contains(queryParameters.Search));
            }

            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                switch (queryParameters.OrderBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(item => EF.Property<string>(item, "Name"));
                        break;
                    // You can add other cases for other properties, following the same pattern.
                    default:
                        break;
                }
            }
            // Pagination
            var totalCount = await _context.Set<T>().CountAsync();
            var items = await query.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                                   .Take(queryParameters.PageSize)
                                   .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                                   .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = queryParameters.Page,
                PageSize = queryParameters.PageSize
            };
        }


        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            }
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<TResult> GetAsync<TResult>(int? id) 
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
            }
            
            return _mapper.Map<TResult>(result);
        }


        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<TSource>(int id, TSource source)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
            _mapper.Map(source, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
