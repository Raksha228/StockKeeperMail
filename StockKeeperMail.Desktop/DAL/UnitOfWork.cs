using StockKeeperMail.Database.Data;
using StockKeeperMail.Database.Models;
using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace StockKeeperMail.Desktop.DAL
{
    /// <summary>
    /// Представляет класс UnitOfWork для работы напрямую с SQL Server LocalDB через Entity Framework Core.
    /// Desktop работает с БД напрямую, без отдельной серверной части.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;

        private readonly InventoryManagementContext _context;
        private IDbContextTransaction _transaction;

        public GenericRepository<Role> RoleRepository { get; }
        public GenericRepository<Category> CategoryRepository { get; }
        public GenericRepository<Warehouse> WarehouseRepository { get; }
        public GenericRepository<Supplier> SupplierRepository { get; }
        public GenericRepository<Staff> StaffRepository { get; }
        public GenericRepository<Product> ProductRepository { get; }
        public GenericRepository<Order> OrderRepository { get; }
        public GenericRepository<OrderDetail> OrderDetailRepository { get; }
        public GenericRepository<Location> LocationRepository { get; }
        public GenericRepository<Customer> CustomerRepository { get; }
        public GenericRepository<Defective> DefectiveRepository { get; }
        public GenericRepository<ProductLocation> ProductLocationRepository { get; }
        public GenericRepository<PurchaseReceipt> PurchaseReceiptRepository { get; }
        public GenericRepository<Log> LogRepository { get; }
        public GenericRepository<InternalMessage> InternalMessageRepository { get; }

        public UnitOfWork()
        {
            _context = new InventoryManagementContext();

            RoleRepository = new GenericRepository<Role>(_context);
            CategoryRepository = new GenericRepository<Category>(_context);
            WarehouseRepository = new GenericRepository<Warehouse>(_context);
            SupplierRepository = new GenericRepository<Supplier>(_context);
            StaffRepository = new GenericRepository<Staff>(_context);
            ProductRepository = new GenericRepository<Product>(_context);
            OrderRepository = new GenericRepository<Order>(_context);
            OrderDetailRepository = new GenericRepository<OrderDetail>(_context);
            LocationRepository = new GenericRepository<Location>(_context);
            CustomerRepository = new GenericRepository<Customer>(_context);
            DefectiveRepository = new GenericRepository<Defective>(_context);
            ProductLocationRepository = new GenericRepository<ProductLocation>(_context);
            PurchaseReceiptRepository = new GenericRepository<PurchaseReceipt>(_context);
            LogRepository = new GenericRepository<Log>(_context);
            InternalMessageRepository = new GenericRepository<InternalMessage>(_context);
        }

        public void Begin()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
