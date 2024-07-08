namespace MasterData;

// public class Repository<T> : IRepository<T> where T : class
// {
//     private readonly DbContext _context;
//     private readonly DbSet<T> _dbSet;
//
//     public Repository(DbContext context)
//     {
//         _context = context;
//         _dbSet = context.Set<T>();
//     }
//
//     public IEnumerable<T> GetAll()
//     {
//         return _dbSet.ToList();
//     }
//
//     public T GetById(int id)
//     {
//         return _dbSet.Find(id);
//     }
//
//     public void Add(T entity)
//     {
//         _dbSet.Add(entity);
//         _context.SaveChanges();
//     }
//
//     public void Update(T entity)
//     {
//         _dbSet.Attach(entity);
//         _context.SaveChanges();
//     }
//
//     public void Delete(int id)
//     {
//         var entity = _dbSet.Find(id);
//         if (entity != null)
//         {
//             _dbSet.Remove(entity);
//             _context.SaveChanges();
//         }
//     }
// }
