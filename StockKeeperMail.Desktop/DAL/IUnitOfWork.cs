using StockKeeperMail.Database.Models;

namespace StockKeeperMail.Desktop.DAL
{
    /// <summary>
    /// Представляет интерфейс IUnitOfWork.
    /// </summary>
    public interface IUnitOfWork
    {
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

        public void Save();
    }
}
