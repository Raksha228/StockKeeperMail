using StockKeeperMail.Database.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StockKeeperMail.Database.Data
{
    /// <summary>
    /// Автоматически заполняет локальную SQL Server LocalDB демонстрационными данными.
    /// </summary>
    public static class DatabaseSeeder
    {
        public static void EnsureSeeded(InventoryManagementContext context)
        {
            if (!context.Roles.Any()) context.Roles.AddRange(CreateRoles());
            if (!context.Categories.Any()) context.Categories.AddRange(CreateCategories());
            if (!context.Suppliers.Any()) context.Suppliers.AddRange(CreateSuppliers());
            if (!context.Warehouses.Any()) context.Warehouses.AddRange(CreateWarehouses());
            if (!context.Locations.Any()) context.Locations.AddRange(CreateLocations());
            if (!context.Staffs.Any()) context.Staffs.AddRange(CreateStaff());
            if (!context.Products.Any()) context.Products.AddRange(CreateProducts());
            if (!context.Customers.Any()) context.Customers.AddRange(CreateCustomers());
            if (!context.Orders.Any()) context.Orders.AddRange(CreateOrders());
            if (!context.OrderDetails.Any()) context.OrderDetails.AddRange(CreateOrderDetails());
            if (!context.ProductLocations.Any()) context.ProductLocations.AddRange(CreateProductLocations());
            if (!context.PurchaseReceipts.Any()) context.PurchaseReceipts.AddRange(CreatePurchaseReceipts());
            if (!context.Defectives.Any()) context.Defectives.AddRange(CreateDefectives());
            if (!context.Logs.Any()) context.Logs.AddRange(CreateLogs());
            if (!context.InternalMessages.Any()) context.InternalMessages.AddRange(CreateMessages());

            context.SaveChanges();

            EnsureOrderDeliveryData(context);

            context.SaveChanges();
        }

        private static void EnsureOrderDeliveryData(InventoryManagementContext context)
        {
            var customerAddresses = context.Customers
                .ToDictionary(c => c.CustomerID, c => c.CustomerAddress ?? string.Empty);

            var orders = context.Orders
                .OrderBy(o => o.OrderDate)
                .ThenBy(o => o.OrderID)
                .ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];

                if (string.IsNullOrWhiteSpace(order.ExternalOrderNumber))
                {
                    order.ExternalOrderNumber = $"EXT-{order.OrderDate:yyyyMMdd}-{i + 1:D4}";
                }

                if (string.IsNullOrWhiteSpace(order.DeliveryAddress)
                    && customerAddresses.TryGetValue(order.CustomerID, out string customerAddress))
                {
                    order.DeliveryAddress = customerAddress;
                }

                if (!string.IsNullOrWhiteSpace(order.ExternalOrderNumber))
                {
                    order.IsOnlineOrder = true;
                }
            }
        }

        private static List<Role> CreateRoles()
        {
            return new List<Role>
            {
                new Role
                {
                    RoleID = Guid.Parse("f59e8d9d-e9ea-5351-8f5f-ce0dfa519065"),
                    RoleName = "Админ",
                    RoleStatus = "Active",
                    RoleDescription = "Полный доступ ко всем разделам системы",
                    OrdersView = true,
                    OrdersAdd = true,
                    OrdersEdit = true,
                    OrdersDelete = true,
                    CustomersView = true,
                    CustomersAdd = true,
                    CustomersEdit = true,
                    CustomersDelete = true,
                    ProductsView = true,
                    ProductsAdd = true,
                    ProductsEdit = true,
                    ProductsDelete = true,
                    StoragesView = true,
                    StoragesAdd = true,
                    StoragesEdit = true,
                    StoragesDelete = true,
                    DefectivesView = true,
                    DefectivesAdd = true,
                    DefectivesEdit = true,
                    DefectivesDelete = true,
                    CategoriesView = true,
                    CategoriesAdd = true,
                    CategoriesEdit = true,
                    CategoriesDelete = true,
                    LocationsView = true,
                    LocationsAdd = true,
                    LocationsEdit = true,
                    LocationsDelete = true,
                    SuppliersView = true,
                    SuppliersAdd = true,
                    SuppliersEdit = true,
                    SuppliersDelete = true,
                    RolesView = true,
                    RolesAdd = true,
                    RolesEdit = true,
                    RolesDelete = true,
                    StaffsView = true,
                    StaffsAdd = true,
                    StaffsEdit = true,
                    StaffsDelete = true,
                    LogsView = true,
                    LogsAdd = true,
                    LogsEdit = true,
                    LogsDelete = true,
                },
                new Role
                {
                    RoleID = Guid.Parse("840facc0-3e2b-50f9-a911-bd6432c57085"),
                    RoleName = "Администратор",
                    RoleStatus = "Active",
                    RoleDescription = "Управление продажами, товарами, поставщиками и персоналом без полного системного доступа",
                    OrdersView = true,
                    OrdersAdd = true,
                    OrdersEdit = true,
                    OrdersDelete = false,
                    CustomersView = true,
                    CustomersAdd = true,
                    CustomersEdit = true,
                    CustomersDelete = false,
                    ProductsView = true,
                    ProductsAdd = true,
                    ProductsEdit = true,
                    ProductsDelete = false,
                    StoragesView = true,
                    StoragesAdd = true,
                    StoragesEdit = true,
                    StoragesDelete = false,
                    DefectivesView = true,
                    DefectivesAdd = true,
                    DefectivesEdit = true,
                    DefectivesDelete = false,
                    CategoriesView = true,
                    CategoriesAdd = true,
                    CategoriesEdit = true,
                    CategoriesDelete = false,
                    LocationsView = true,
                    LocationsAdd = true,
                    LocationsEdit = true,
                    LocationsDelete = false,
                    SuppliersView = true,
                    SuppliersAdd = true,
                    SuppliersEdit = true,
                    SuppliersDelete = false,
                    RolesView = true,
                    RolesAdd = false,
                    RolesEdit = false,
                    RolesDelete = false,
                    StaffsView = true,
                    StaffsAdd = true,
                    StaffsEdit = true,
                    StaffsDelete = false,
                    LogsView = true,
                    LogsAdd = false,
                    LogsEdit = false,
                    LogsDelete = false,
                },
                new Role
                {
                    RoleID = Guid.Parse("5aa02ddb-1499-5ebf-b56e-50ef9162a4cf"),
                    RoleName = "Продавец",
                    RoleStatus = "Active",
                    RoleDescription = "Оформление заказов, работа с клиентами и просмотр складских остатков",
                    OrdersView = true,
                    OrdersAdd = true,
                    OrdersEdit = true,
                    OrdersDelete = false,
                    CustomersView = true,
                    CustomersAdd = true,
                    CustomersEdit = true,
                    CustomersDelete = false,
                    ProductsView = true,
                    ProductsAdd = false,
                    ProductsEdit = false,
                    ProductsDelete = false,
                    StoragesView = true,
                    StoragesAdd = false,
                    StoragesEdit = false,
                    StoragesDelete = false,
                    DefectivesView = false,
                    DefectivesAdd = false,
                    DefectivesEdit = false,
                    DefectivesDelete = false,
                    CategoriesView = true,
                    CategoriesAdd = false,
                    CategoriesEdit = false,
                    CategoriesDelete = false,
                    LocationsView = true,
                    LocationsAdd = false,
                    LocationsEdit = false,
                    LocationsDelete = false,
                    SuppliersView = true,
                    SuppliersAdd = false,
                    SuppliersEdit = false,
                    SuppliersDelete = false,
                    RolesView = false,
                    RolesAdd = false,
                    RolesEdit = false,
                    RolesDelete = false,
                    StaffsView = false,
                    StaffsAdd = false,
                    StaffsEdit = false,
                    StaffsDelete = false,
                    LogsView = false,
                    LogsAdd = false,
                    LogsEdit = false,
                    LogsDelete = false,
                },
            };
        }

        private static List<Category> CreateCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    CategoryName = "Орто",
                    CategoryStatus = "Active",
                    CategoryDescription = "Ортопедические изделия, компрессионный трикотаж, средства реабилитации, инвалидные коляски и туторы",
                },
                new Category
                {
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    CategoryName = "Уро",
                    CategoryStatus = "Active",
                    CategoryDescription = "Урологические изделия: катетеры для самокатетеризации, мочеприемники и урологические наборы",
                },
            };
        }

        private static List<Supplier> CreateSuppliers()
        {
            return new List<Supplier>
            {
                new Supplier
                {
                    SupplierID = Guid.Parse("ddde6b86-cb9d-59cb-91a8-50f74303af87"),
                    SupplierName = "ООО «КРЕЙТ»",
                    SupplierAddress = "195220, г. Санкт-Петербург, ул. Бутлерова, д. 11, к. 4",
                    SupplierPhone = "+7 (800) 700-68-50",
                    SupplierEmail = "kreit@kreitspb.ru",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("b1114d18-d8cc-5ca6-8baa-389457a3b244"),
                    SupplierName = "ООО «Тривес»",
                    SupplierAddress = "196624, Россия, г. Санкт-Петербург",
                    SupplierPhone = "+7 (812) 329-72-97",
                    SupplierEmail = "sale@trives-spb.ru",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("18c86884-7b4a-58be-b987-b4c15eedfb5f"),
                    SupplierName = "ООО «ЭКОТЕН»",
                    SupplierAddress = "197110, г. Санкт-Петербург, Константиновский пр., д. 18А, пом. 11Н",
                    SupplierPhone = "+7 (812) 325-09-04",
                    SupplierEmail = "e-mail@ecoten.ru",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("44ffbaad-8af0-5f8b-b7b6-c4177e292cda"),
                    SupplierName = "ООО «Малтри»",
                    SupplierAddress = "190020, г. Санкт-Петербург",
                    SupplierPhone = "+7 (812) 336-39-99",
                    SupplierEmail = "info@maltri.ru",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("2b60a221-2ddc-5445-a501-5189e4d2c20f"),
                    SupplierName = "ТД «ОптоМед»",
                    SupplierAddress = "г. Москва, ул. Кантемировская, д. 58",
                    SupplierPhone = "+7 (495) 979-17-75",
                    SupplierEmail = "info@ortopt.ru",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("889fe7ad-9860-531c-9d38-d0f203f8d65a"),
                    SupplierName = "ООО «Интернет Решения» (Ozon)",
                    SupplierAddress = "123112, г. Москва, Пресненская наб., д. 10",
                    SupplierPhone = "",
                    SupplierEmail = "",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("4b9c4f43-d5f3-5a18-b446-0da7aa56798e"),
                    SupplierName = "ООО «РВБ» (Wildberries)",
                    SupplierAddress = "Россия, онлайн-площадка",
                    SupplierPhone = "",
                    SupplierEmail = "",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    SupplierName = "ООО «Аванта Медика»",
                    SupplierAddress = "119331, г. Москва, пр-кт Вернадского, д. 29",
                    SupplierPhone = "+7 (499) 130-30-57",
                    SupplierEmail = "info@avantamedika.ru",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    SupplierName = "Mystoma.ru",
                    SupplierAddress = "г. Москва",
                    SupplierPhone = "+7 (495) 509-44-49",
                    SupplierEmail = "mystoma@gmail.com",
                    SupplierStatus = "Active",
                },
                new Supplier
                {
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    SupplierName = "ООО «Лик-Мед» (МеДеРи)",
                    SupplierAddress = "г. Санкт-Петербург, ул. Константина Заслонова, д. 11",
                    SupplierPhone = "+7 (812) 600-19-93",
                    SupplierEmail = "likmed-sales@bk.ru",
                    SupplierStatus = "Active",
                },
            };
        }

        private static List<Warehouse> CreateWarehouses()
        {
            return new List<Warehouse>
            {
                new Warehouse
                {
                    WarehouseID = Guid.Parse("84900bc7-be71-5036-87ca-245626fb8e3c"),
                    WarehouseName = "Магазин «ОртоУро» — Основной",
                    WarehouseAddress = "Ростовская обл., г. Волгодонск, б-р Великой Победы, 11, офис 10",
                    WarehousePhone = "+7-8639-90-11-10",
                    WarehouseEmail = "shop1@ortouro.local",
                    WarehouseVat = 20.00m,
                },
                new Warehouse
                {
                    WarehouseID = Guid.Parse("0935fa03-15a5-50ce-885e-f6833571d7ed"),
                    WarehouseName = "Магазин «ОртоУро» — Восточный",
                    WarehouseAddress = "Ростовская обл., г. Волгодонск, ул. Энтузиастов, 28",
                    WarehousePhone = "+7-8639-90-11-11",
                    WarehouseEmail = "shop2@ortouro.local",
                    WarehouseVat = 20.00m,
                },
                new Warehouse
                {
                    WarehouseID = Guid.Parse("36083aa4-a7a7-5850-b983-12ca08baa150"),
                    WarehouseName = "Склад «ОртоУро» — Центральный",
                    WarehouseAddress = "Ростовская обл., г. Волгодонск, ул. 8-я Заводская, 12",
                    WarehousePhone = "+7-8639-90-11-12",
                    WarehouseEmail = "warehouse@ortouro.local",
                    WarehouseVat = 20.00m,
                },
            };
        }

        private static List<Location> CreateLocations()
        {
            return new List<Location>
            {
                new Location
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    LocationName = "Основной магазин — витрина 1",
                },
                new Location
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    LocationName = "Основной магазин — витрина 2",
                },
                new Location
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    LocationName = "Основной магазин — стеллаж A1",
                },
                new Location
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    LocationName = "Основной магазин — стеллаж A2",
                },
                new Location
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    LocationName = "Основной магазин — подсобка",
                },
                new Location
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    LocationName = "Восточный магазин — витрина 1",
                },
                new Location
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    LocationName = "Восточный магазин — витрина 2",
                },
                new Location
                {
                    LocationID = Guid.Parse("560de5b9-d5fd-5f38-8189-d70fe93f9181"),
                    LocationName = "Восточный магазин — стеллаж B1",
                },
                new Location
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    LocationName = "Центральный склад — ячейка A1",
                },
                new Location
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    LocationName = "Центральный склад — ячейка A2",
                },
                new Location
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    LocationName = "Центральный склад — ячейка B1",
                },
                new Location
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    LocationName = "Центральный склад — приемка",
                },
            };
        }

        private static List<Staff> CreateStaff()
        {
            return new List<Staff>
            {
                new Staff
                {
                    StaffID = Guid.Parse("4be43b93-b4cb-5c93-b1b6-434251fd7e7d"),
                    RoleID = Guid.Parse("f59e8d9d-e9ea-5351-8f5f-ce0dfa519065"),
                    StaffFirstName = "Алексей",
                    StaffLastName = "Кузнецов",
                    StaffAddress = "Ростовская обл., г. Волгодонск, б-р Великой Победы, 11, офис 10",
                    StaffPhone = "+7-928-600-11-01",
                    StaffEmail = "admin@ortouro.local",
                    StaffUsername = "admin",
                    StaffPassword = "admin123",
                },
                new Staff
                {
                    StaffID = Guid.Parse("38841fb3-3d9e-530e-87ac-405843ba1e82"),
                    RoleID = Guid.Parse("840facc0-3e2b-50f9-a911-bd6432c57085"),
                    StaffFirstName = "Марина",
                    StaffLastName = "Егорова",
                    StaffAddress = "Ростовская обл., г. Волгодонск, б-р Великой Победы, 11, офис 10",
                    StaffPhone = "+7-928-600-11-02",
                    StaffEmail = "administrator@ortouro.local",
                    StaffUsername = "administrator",
                    StaffPassword = "admin456",
                },
                new Staff
                {
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    RoleID = Guid.Parse("5aa02ddb-1499-5ebf-b56e-50ef9162a4cf"),
                    StaffFirstName = "Ирина",
                    StaffLastName = "Соколова",
                    StaffAddress = "Ростовская обл., г. Волгодонск, ул. Энтузиастов, 18",
                    StaffPhone = "+7-928-600-11-03",
                    StaffEmail = "seller1@ortouro.local",
                    StaffUsername = "seller1",
                    StaffPassword = "seller123",
                },
                new Staff
                {
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    RoleID = Guid.Parse("5aa02ddb-1499-5ebf-b56e-50ef9162a4cf"),
                    StaffFirstName = "Павел",
                    StaffLastName = "Логинов",
                    StaffAddress = "Ростовская обл., г. Волгодонск, ул. Морская, 34",
                    StaffPhone = "+7-928-600-11-04",
                    StaffEmail = "seller2@ortouro.local",
                    StaffUsername = "seller2",
                    StaffPassword = "seller123",
                },
                new Staff
                {
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    RoleID = Guid.Parse("5aa02ddb-1499-5ebf-b56e-50ef9162a4cf"),
                    StaffFirstName = "Елена",
                    StaffLastName = "Громова",
                    StaffAddress = "Ростовская обл., г. Волгодонск, ул. Ленина, 82",
                    StaffPhone = "+7-928-600-11-05",
                    StaffEmail = "seller3@ortouro.local",
                    StaffUsername = "seller3",
                    StaffPassword = "seller123",
                },
            };
        }

        private static List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    SupplierID = Guid.Parse("ddde6b86-cb9d-59cb-91a8-50f74303af87"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж на брюшную стенку",
                    ProductSKU = "ORTO-001",
                    ProductUnit = "шт.",
                    ProductPrice = 1290.00m,
                    ProductQuantity = 24,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    SupplierID = Guid.Parse("ddde6b86-cb9d-59cb-91a8-50f74303af87"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж послеродовой",
                    ProductSKU = "ORTO-002",
                    ProductUnit = "шт.",
                    ProductPrice = 1490.00m,
                    ProductQuantity = 18,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("2e7ca414-836f-5174-8c7c-4455fb580cd4"),
                    SupplierID = Guid.Parse("b1114d18-d8cc-5ca6-8baa-389457a3b244"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж послеоперационный на грудную клетку женский",
                    ProductSKU = "ORTO-003",
                    ProductUnit = "шт.",
                    ProductPrice = 2390.00m,
                    ProductQuantity = 9,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    SupplierID = Guid.Parse("b1114d18-d8cc-5ca6-8baa-389457a3b244"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж дородовой",
                    ProductSKU = "ORTO-004",
                    ProductUnit = "шт.",
                    ProductPrice = 1790.00m,
                    ProductQuantity = 14,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    SupplierID = Guid.Parse("18c86884-7b4a-58be-b987-b4c15eedfb5f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Корсет ортопедический",
                    ProductSKU = "ORTO-005",
                    ProductUnit = "шт.",
                    ProductPrice = 3190.00m,
                    ProductQuantity = 11,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    SupplierID = Guid.Parse("18c86884-7b4a-58be-b987-b4c15eedfb5f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж послеоперационный на брюшную стенку",
                    ProductSKU = "ORTO-006",
                    ProductUnit = "шт.",
                    ProductPrice = 1990.00m,
                    ProductQuantity = 16,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("550a6126-9886-5858-b6f6-58deebd3a657"),
                    SupplierID = Guid.Parse("44ffbaad-8af0-5f8b-b7b6-c4177e292cda"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Корсет ортопедический поясничный усиленный",
                    ProductSKU = "ORTO-007",
                    ProductUnit = "шт.",
                    ProductPrice = 4890.00m,
                    ProductQuantity = 8,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    SupplierID = Guid.Parse("44ffbaad-8af0-5f8b-b7b6-c4177e292cda"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж фиксирующий для руки",
                    ProductSKU = "ORTO-008",
                    ProductUnit = "шт.",
                    ProductPrice = 1190.00m,
                    ProductQuantity = 21,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("2a4f1de8-b2eb-555e-a7d4-6e8c6fbd1d70"),
                    SupplierID = Guid.Parse("2b60a221-2ddc-5445-a501-5189e4d2c20f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж для руки",
                    ProductSKU = "ORTO-009",
                    ProductUnit = "шт.",
                    ProductPrice = 990.00m,
                    ProductQuantity = 25,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("d2a0bcd3-2cac-5be6-a87d-db963ffa37c5"),
                    SupplierID = Guid.Parse("2b60a221-2ddc-5445-a501-5189e4d2c20f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж компрессионный на лучезапястный сустав",
                    ProductSKU = "ORTO-010",
                    ProductUnit = "шт.",
                    ProductPrice = 890.00m,
                    ProductQuantity = 13,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    SupplierID = Guid.Parse("2b60a221-2ddc-5445-a501-5189e4d2c20f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж на лучезапястный сустав",
                    ProductSKU = "ORTO-011",
                    ProductUnit = "шт.",
                    ProductPrice = 790.00m,
                    ProductQuantity = 20,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("bb6b7911-678d-52c8-82a7-c5720b42c33b"),
                    SupplierID = Guid.Parse("ddde6b86-cb9d-59cb-91a8-50f74303af87"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж на бедро",
                    ProductSKU = "ORTO-012",
                    ProductUnit = "шт.",
                    ProductPrice = 1290.00m,
                    ProductQuantity = 7,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("32515653-1be6-534a-9670-2a4333191e00"),
                    SupplierID = Guid.Parse("b1114d18-d8cc-5ca6-8baa-389457a3b244"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж на голень и голеностопный сустав",
                    ProductSKU = "ORTO-013",
                    ProductUnit = "шт.",
                    ProductPrice = 1590.00m,
                    ProductQuantity = 15,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("d64b3134-3e6b-5699-aa1c-26cdb6281567"),
                    SupplierID = Guid.Parse("18c86884-7b4a-58be-b987-b4c15eedfb5f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Бандаж шейный детский",
                    ProductSKU = "ORTO-014",
                    ProductUnit = "шт.",
                    ProductPrice = 1090.00m,
                    ProductQuantity = 12,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    SupplierID = Guid.Parse("44ffbaad-8af0-5f8b-b7b6-c4177e292cda"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Гольфы компрессионные мужские",
                    ProductSKU = "ORTO-015",
                    ProductUnit = "пара",
                    ProductPrice = 1690.00m,
                    ProductQuantity = 26,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("7989d799-effa-5214-9205-b48e3d4ce254"),
                    SupplierID = Guid.Parse("44ffbaad-8af0-5f8b-b7b6-c4177e292cda"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Гольфы компрессионные женские",
                    ProductSKU = "ORTO-016",
                    ProductUnit = "пара",
                    ProductPrice = 1690.00m,
                    ProductQuantity = 22,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("49837b1c-aeac-5661-8d07-a97adc41d851"),
                    SupplierID = Guid.Parse("2b60a221-2ddc-5445-a501-5189e4d2c20f"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Компрессионный трикотаж эластичный",
                    ProductSKU = "ORTO-017",
                    ProductUnit = "шт.",
                    ProductPrice = 2190.00m,
                    ProductQuantity = 10,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("241ce5fb-54ae-554f-b6f7-6edfa6a4290e"),
                    SupplierID = Guid.Parse("889fe7ad-9860-531c-9d38-d0f203f8d65a"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Инвалидная коляска складная",
                    ProductSKU = "ORTO-018",
                    ProductUnit = "шт.",
                    ProductPrice = 28900.00m,
                    ProductQuantity = 3,
                    ProductAvailability = "Unavailable",
                },
                new Product
                {
                    ProductID = Guid.Parse("f29e8709-5006-5eb5-b95a-bb656d0db629"),
                    SupplierID = Guid.Parse("4b9c4f43-d5f3-5a18-b446-0da7aa56798e"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Инвалидная коляска комнатная",
                    ProductSKU = "ORTO-019",
                    ProductUnit = "шт.",
                    ProductPrice = 31200.00m,
                    ProductQuantity = 1,
                    ProductAvailability = "Unavailable",
                },
                new Product
                {
                    ProductID = Guid.Parse("888a2181-f7c0-5886-8f08-fea73f0c0b12"),
                    SupplierID = Guid.Parse("b1114d18-d8cc-5ca6-8baa-389457a3b244"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Тутор на коленный сустав",
                    ProductSKU = "ORTO-020",
                    ProductUnit = "шт.",
                    ProductPrice = 3490.00m,
                    ProductQuantity = 6,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("1296c8f3-4c34-58bd-8fa5-58e65c09dc6b"),
                    SupplierID = Guid.Parse("ddde6b86-cb9d-59cb-91a8-50f74303af87"),
                    CategoryID = Guid.Parse("ffe9e06e-ec2e-5dc2-856b-ccc6b53f3334"),
                    ProductName = "Тутор на голеностопный сустав",
                    ProductSKU = "ORTO-021",
                    ProductUnit = "шт.",
                    ProductPrice = 2990.00m,
                    ProductQuantity = 0,
                    ProductAvailability = "Out Of Stock",
                },
                new Product
                {
                    ProductID = Guid.Parse("069fefd7-707c-59e7-a809-b4e2bcda0482"),
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Набор-мочеприемник Нелатон Coloplast",
                    ProductSKU = "URO-001",
                    ProductUnit = "уп.",
                    ProductPrice = 780.00m,
                    ProductQuantity = 30,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("4ddeb0e5-975c-5777-82c6-602057f15e41"),
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Набор-мочеприемник Нелатон B. Braun",
                    ProductSKU = "URO-002",
                    ProductUnit = "уп.",
                    ProductPrice = 760.00m,
                    ProductQuantity = 28,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Набор-мочеприемник Нелатон Integral",
                    ProductSKU = "URO-003",
                    ProductUnit = "уп.",
                    ProductPrice = 690.00m,
                    ProductQuantity = 18,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("6f81d746-9038-5d87-b0ab-1c40cac51de0"),
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер лубрицированный Coloplast",
                    ProductSKU = "URO-004",
                    ProductUnit = "шт.",
                    ProductPrice = 390.00m,
                    ProductQuantity = 45,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер лубрицированный Coloplast EasiCath",
                    ProductSKU = "URO-005",
                    ProductUnit = "шт.",
                    ProductPrice = 430.00m,
                    ProductQuantity = 52,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("97e97f84-88e5-5221-a6f1-434d1566bbcb"),
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер лубрицированный B. Braun Actreen Lite",
                    ProductSKU = "URO-006",
                    ProductUnit = "шт.",
                    ProductPrice = 410.00m,
                    ProductQuantity = 38,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("3377495b-f6e6-5b19-aacd-dd4ddce972b4"),
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер лубрицированный Mederen",
                    ProductSKU = "URO-007",
                    ProductUnit = "шт.",
                    ProductPrice = 350.00m,
                    ProductQuantity = 33,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("65b879b6-a61c-5043-af71-62a7958a4713"),
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер лубрицированный Apexmed",
                    ProductSKU = "URO-008",
                    ProductUnit = "шт.",
                    ProductPrice = 320.00m,
                    ProductQuantity = 40,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Набор-мочеприемник Coloplast EasiCath Set",
                    ProductSKU = "URO-009",
                    ProductUnit = "уп.",
                    ProductPrice = 890.00m,
                    ProductQuantity = 17,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("d37e1ce4-6ae2-5d76-85ad-67959bcf03a1"),
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Набор-мочеприемник B. Braun Actreen Glys Set",
                    ProductSKU = "URO-010",
                    ProductUnit = "уп.",
                    ProductPrice = 870.00m,
                    ProductQuantity = 4,
                    ProductAvailability = "Unavailable",
                },
                new Product
                {
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Мочеприемник ножной Coloplast Conveen Security+",
                    ProductSKU = "URO-011",
                    ProductUnit = "шт.",
                    ProductPrice = 590.00m,
                    ProductQuantity = 24,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("701b3743-5c98-5e45-8a81-ee8bf769ea15"),
                    SupplierID = Guid.Parse("1d578c20-acc5-5dcb-beb3-5035767303b9"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Мочеприемник прикроватный Coloplast Conveen Standard 1500 мл",
                    ProductSKU = "URO-012",
                    ProductUnit = "шт.",
                    ProductPrice = 640.00m,
                    ProductQuantity = 19,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Мочеприемник прикроватный Coloplast Conveen Basic 2000 мл",
                    ProductSKU = "URO-013",
                    ProductUnit = "шт.",
                    ProductPrice = 680.00m,
                    ProductQuantity = 21,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("53af2937-b05b-5753-a125-cf664bb7ba8b"),
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер Нелатона Coloplast",
                    ProductSKU = "URO-014",
                    ProductUnit = "шт.",
                    ProductPrice = 270.00m,
                    ProductQuantity = 36,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("eacd521b-981a-5879-9c4e-4bc19faa7d6e"),
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер Нелатона B. Braun",
                    ProductSKU = "URO-015",
                    ProductUnit = "шт.",
                    ProductPrice = 260.00m,
                    ProductQuantity = 31,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("3611640a-7c2c-5211-93bd-93e7ee9b55fc"),
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер Нелатона Integral",
                    ProductSKU = "URO-016",
                    ProductUnit = "шт.",
                    ProductPrice = 240.00m,
                    ProductQuantity = 20,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("d0daaeb5-7169-5bf2-adfb-9ef7149a0fd7"),
                    SupplierID = Guid.Parse("1f1a9e0f-4fc5-582c-a9f6-bdab1e0fb5dc"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Катетер Нелатона Apexmed",
                    ProductSKU = "URO-017",
                    ProductUnit = "шт.",
                    ProductPrice = 230.00m,
                    ProductQuantity = 18,
                    ProductAvailability = "Available",
                },
                new Product
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    SupplierID = Guid.Parse("f3b159d2-6c3d-51ee-9caa-62f81b47b631"),
                    CategoryID = Guid.Parse("9c2e7ea7-3c90-5ad0-9a29-ac7f09bc1819"),
                    ProductName = "Мочеприемник ножной B. Braun",
                    ProductSKU = "URO-018",
                    ProductUnit = "шт.",
                    ProductPrice = 560.00m,
                    ProductQuantity = 16,
                    ProductAvailability = "Available",
                },
            };
        }

        private static List<Customer> CreateCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    CustomerID = Guid.Parse("0e7a535f-0698-5033-a5cf-aa1db46219e3"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Иван",
                    CustomerLastname = "Петров",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Энтузиастов, д. 10",
                    CustomerPhone = "+7-928-700-10-20",
                    CustomerEmail = "client1@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("27d0ede2-b2eb-5a55-90c4-fd47a01167cb"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Мария",
                    CustomerLastname = "Иванова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Ленина, д. 11",
                    CustomerPhone = "+7-928-700-11-21",
                    CustomerEmail = "client2@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("50280baa-ec35-5d94-b01d-97fe3894f479"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Ольга",
                    CustomerLastname = "Смирнова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Морская, д. 12",
                    CustomerPhone = "+7-928-700-12-22",
                    CustomerEmail = "client3@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Дмитрий",
                    CustomerLastname = "Ковалев",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, пр-кт Строителей, д. 13",
                    CustomerPhone = "+7-928-700-13-23",
                    CustomerEmail = "client4@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("311d186c-77bd-58d1-803d-a3baa5832f13"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Екатерина",
                    CustomerLastname = "Федорова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Молодежная, д. 14",
                    CustomerPhone = "+7-928-700-14-24",
                    CustomerEmail = "client5@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("60a755b5-84ff-5ebc-a6da-9a6572f09944"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Сергей",
                    CustomerLastname = "Кузьмин",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Гагарина, д. 15",
                    CustomerPhone = "+7-928-700-15-25",
                    CustomerEmail = "client6@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("d46acdf7-49f5-5ebe-8efe-05c042126b5d"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Наталья",
                    CustomerLastname = "Савельева",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Дружбы, д. 16",
                    CustomerPhone = "+7-928-700-16-26",
                    CustomerEmail = "client7@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("b18d48ec-4de1-50cf-91a0-af72c3a8ead9"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Андрей",
                    CustomerLastname = "Морозов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Маршала Кошевого, д. 17",
                    CustomerPhone = "+7-928-700-17-27",
                    CustomerEmail = "client8@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("07aa6c51-3c43-5210-a64e-58cc831fd859"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Татьяна",
                    CustomerLastname = "Герасимова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Черникова, д. 18",
                    CustomerPhone = "+7-928-700-18-28",
                    CustomerEmail = "client9@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("b9497a2f-b7b0-59ee-b4c2-20b52401a922"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Александр",
                    CustomerLastname = "Орлов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. 30 лет Победы, д. 19",
                    CustomerPhone = "+7-928-700-19-29",
                    CustomerEmail = "client10@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("3f9a0889-9fe6-5607-b1d3-105119a109b8"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Светлана",
                    CustomerLastname = "Комарова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Энтузиастов, д. 20",
                    CustomerPhone = "+7-928-700-20-30",
                    CustomerEmail = "client11@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("2c520ea9-d080-5154-a5a8-366849a022af"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Николай",
                    CustomerLastname = "Гусев",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Ленина, д. 21",
                    CustomerPhone = "+7-928-700-21-31",
                    CustomerEmail = "client12@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Юлия",
                    CustomerLastname = "Захарова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Морская, д. 22",
                    CustomerPhone = "+7-928-700-22-32",
                    CustomerEmail = "client13@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("f92735c9-c5d1-541b-a4cb-03b2c9872cae"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Максим",
                    CustomerLastname = "Титов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, пр-кт Строителей, д. 23",
                    CustomerPhone = "+7-928-700-23-33",
                    CustomerEmail = "client14@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("edb05489-090e-57db-8c0d-f9c3c9d68797"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Виктория",
                    CustomerLastname = "Мельникова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Молодежная, д. 24",
                    CustomerPhone = "+7-928-700-24-34",
                    CustomerEmail = "client15@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("30e8fc0e-b850-5899-b644-6eca25b03a5a"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Роман",
                    CustomerLastname = "Абрамов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Гагарина, д. 25",
                    CustomerPhone = "+7-928-700-25-35",
                    CustomerEmail = "client16@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("cf7f6c03-fd3b-5a1d-ba4d-26f155e40cfc"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Людмила",
                    CustomerLastname = "Осипова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Дружбы, д. 26",
                    CustomerPhone = "+7-928-700-26-36",
                    CustomerEmail = "client17@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("0c69b0d1-b441-53c4-bc1e-f5c305d8da33"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Кирилл",
                    CustomerLastname = "Белов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Маршала Кошевого, д. 27",
                    CustomerPhone = "+7-928-700-27-37",
                    CustomerEmail = "client18@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("42b3b7f2-e52a-5a61-9c2a-73fd128d1b95"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Анна",
                    CustomerLastname = "Романова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Черникова, д. 28",
                    CustomerPhone = "+7-928-700-28-38",
                    CustomerEmail = "client19@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("bdd2217a-e62b-57c0-8b50-abdc93035b42"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Владимир",
                    CustomerLastname = "Ефимов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. 30 лет Победы, д. 29",
                    CustomerPhone = "+7-928-700-29-39",
                    CustomerEmail = "client20@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("8f5896b5-e6f4-52e0-8e20-2e4fbceb08cd"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Ирина",
                    CustomerLastname = "Калинина",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Энтузиастов, д. 30",
                    CustomerPhone = "+7-928-700-30-40",
                    CustomerEmail = "client21@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("388d2cf0-81c8-5583-9bde-a54d0291d5a0"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    CustomerFirstname = "Олег",
                    CustomerLastname = "Носов",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Ленина, д. 31",
                    CustomerPhone = "+7-928-700-31-41",
                    CustomerEmail = "client22@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("1ccacb1d-cc3d-51b3-ac13-a36c4a813862"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    CustomerFirstname = "Полина",
                    CustomerLastname = "Семенова",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, ул. Морская, д. 32",
                    CustomerPhone = "+7-928-700-32-42",
                    CustomerEmail = "client23@mail.local",
                },
                new Customer
                {
                    CustomerID = Guid.Parse("def615ca-9cc9-5446-a597-11ca0d40fadc"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    CustomerFirstname = "Михаил",
                    CustomerLastname = "Гордеев",
                    CustomerAddress = "Ростовская обл., г. Волгодонск, пр-кт Строителей, д. 33",
                    CustomerPhone = "+7-928-700-33-43",
                    CustomerEmail = "client24@mail.local",
                },
            };
        }

        private static List<Order> CreateOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    OrderID = Guid.Parse("dd69d17a-304c-55eb-96b9-657919e4d42b"),
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    OrderDate = DateTime.Parse("2026-02-10T11:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 4890.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("085f2e11-08b0-5e5e-810d-b2f97ce90d7d"),
                    CustomerID = Guid.Parse("b18d48ec-4de1-50cf-91a0-af72c3a8ead9"),
                    OrderDate = DateTime.Parse("2026-02-10T10:37:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 2800.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("cb85e1e0-e85e-5c28-b74c-0b69e2fcc21e"),
                    CustomerID = Guid.Parse("b9497a2f-b7b0-59ee-b4c2-20b52401a922"),
                    OrderDate = DateTime.Parse("2026-02-12T10:52:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3780.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("d4e616da-1a08-5ca7-b1a4-51cfac7b6b32"),
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    OrderDate = DateTime.Parse("2026-02-13T15:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3380.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("e3fa598a-133a-5a4b-9719-fc769bd72aec"),
                    CustomerID = Guid.Parse("30e8fc0e-b850-5899-b644-6eca25b03a5a"),
                    OrderDate = DateTime.Parse("2026-02-16T11:37:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 1280.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("f59eb226-dd08-5c29-a1d4-e3d4bbcbd024"),
                    CustomerID = Guid.Parse("bdd2217a-e62b-57c0-8b50-abdc93035b42"),
                    OrderDate = DateTime.Parse("2026-02-16T10:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 2390.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("8abe3c9f-6349-5319-a920-5dd7befdd5ff"),
                    CustomerID = Guid.Parse("388d2cf0-81c8-5583-9bde-a54d0291d5a0"),
                    OrderDate = DateTime.Parse("2026-02-17T17:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 5090.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("9ae638b5-4144-55c9-9398-ec8884ca183f"),
                    CustomerID = Guid.Parse("0e7a535f-0698-5033-a5cf-aa1db46219e3"),
                    OrderDate = DateTime.Parse("2026-02-23T16:22:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3770.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("6d3e031a-dd8c-5b09-ac74-ebdfa7ed22d2"),
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    OrderDate = DateTime.Parse("2026-02-24T15:17:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 4090.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("514eb9fa-62e4-5653-bd8f-52b6d7b27b8b"),
                    CustomerID = Guid.Parse("d46acdf7-49f5-5ebe-8efe-05c042126b5d"),
                    OrderDate = DateTime.Parse("2026-02-25T16:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 9570.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("78c1978a-0877-5383-95ef-4bdc7d5a7c58"),
                    CustomerID = Guid.Parse("3f9a0889-9fe6-5607-b1d3-105119a109b8"),
                    OrderDate = DateTime.Parse("2026-02-25T10:56:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 2700.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("ad0959a7-9a24-528f-90df-cb6aaa31f205"),
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    OrderDate = DateTime.Parse("2026-02-26T17:37:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 1290.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("c17d7386-9d94-5545-9609-a9d10fd99d70"),
                    CustomerID = Guid.Parse("30e8fc0e-b850-5899-b644-6eca25b03a5a"),
                    OrderDate = DateTime.Parse("2026-02-27T14:52:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3470.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("0044cde7-12b4-50d8-a01c-18d85712ff01"),
                    CustomerID = Guid.Parse("42b3b7f2-e52a-5a61-9c2a-73fd128d1b95"),
                    OrderDate = DateTime.Parse("2026-03-04T14:56:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 6200.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("7a1e5c56-a69e-5984-a208-1ae0cf31c918"),
                    CustomerID = Guid.Parse("388d2cf0-81c8-5583-9bde-a54d0291d5a0"),
                    OrderDate = DateTime.Parse("2026-03-06T11:26:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3300.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("a900ab77-c454-5ac7-9a4f-6b14cbdaf6db"),
                    CustomerID = Guid.Parse("0e7a535f-0698-5033-a5cf-aa1db46219e3"),
                    OrderDate = DateTime.Parse("2026-03-09T12:22:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3160.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("6b820c4c-54ef-5e30-b810-aef3ce84092f"),
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    OrderDate = DateTime.Parse("2026-03-10T11:17:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 31200.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("b612091f-6703-5518-8a78-da6e932b72c1"),
                    CustomerID = Guid.Parse("d46acdf7-49f5-5ebe-8efe-05c042126b5d"),
                    OrderDate = DateTime.Parse("2026-03-11T10:11:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 5360.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("5f1227a4-0ead-5c1c-95f3-d3306123c044"),
                    CustomerID = Guid.Parse("b9497a2f-b7b0-59ee-b4c2-20b52401a922"),
                    OrderDate = DateTime.Parse("2026-03-12T12:47:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 5270.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("deaad6fb-e671-54f8-88ef-e93787a866b5"),
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    OrderDate = DateTime.Parse("2026-03-13T10:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 15460.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("4edc157b-d35d-5ab1-855d-c8e506b1cc1e"),
                    CustomerID = Guid.Parse("30e8fc0e-b850-5899-b644-6eca25b03a5a"),
                    OrderDate = DateTime.Parse("2026-03-16T14:34:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 2600.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("3b6f0a35-7b0c-5618-99f0-858fc9082c9c"),
                    CustomerID = Guid.Parse("42b3b7f2-e52a-5a61-9c2a-73fd128d1b95"),
                    OrderDate = DateTime.Parse("2026-03-17T10:11:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 5860.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("d50a3336-2466-542b-8587-a54640a7dab7"),
                    CustomerID = Guid.Parse("1ccacb1d-cc3d-51b3-ac13-a36c4a813862"),
                    OrderDate = DateTime.Parse("2026-03-17T17:22:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 2800.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("1a4c2ff4-61e2-5d0d-b6de-749650f8b960"),
                    CustomerID = Guid.Parse("0e7a535f-0698-5033-a5cf-aa1db46219e3"),
                    OrderDate = DateTime.Parse("2026-03-18T10:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 6660.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("2e43059f-c7ca-569e-b508-cd2082892fa9"),
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    OrderDate = DateTime.Parse("2026-03-19T12:11:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 8860.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("f87ed5ce-6c07-5e50-86aa-bdf03b94f712"),
                    CustomerID = Guid.Parse("d46acdf7-49f5-5ebe-8efe-05c042126b5d"),
                    OrderDate = DateTime.Parse("2026-03-20T15:22:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 960.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("1bd943e9-4a96-5b89-91b1-68d355f07426"),
                    CustomerID = Guid.Parse("b9497a2f-b7b0-59ee-b4c2-20b52401a922"),
                    OrderDate = DateTime.Parse("2026-03-23T14:34:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 690.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("b0f203b8-7938-585b-bfe3-592ffc110b90"),
                    CustomerID = Guid.Parse("f92735c9-c5d1-541b-a4cb-03b2c9872cae"),
                    OrderDate = DateTime.Parse("2026-03-23T11:11:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3210.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("2052972c-8c60-5d8a-8b9e-af180d3fd5a5"),
                    CustomerID = Guid.Parse("30e8fc0e-b850-5899-b644-6eca25b03a5a"),
                    OrderDate = DateTime.Parse("2026-03-24T14:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 37070.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("88d99e92-c60e-58c6-a315-ab7bd0d877d8"),
                    CustomerID = Guid.Parse("bdd2217a-e62b-57c0-8b50-abdc93035b42"),
                    OrderDate = DateTime.Parse("2026-03-24T13:47:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 4450.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("7fbb4654-9416-50ab-a124-4fc8e99d1b6a"),
                    CustomerID = Guid.Parse("388d2cf0-81c8-5583-9bde-a54d0291d5a0"),
                    OrderDate = DateTime.Parse("2026-03-25T10:11:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 5370.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("d3675840-9fb1-5633-8ea9-d17c1d463a72"),
                    CustomerID = Guid.Parse("27d0ede2-b2eb-5a55-90c4-fd47a01167cb"),
                    OrderDate = DateTime.Parse("2026-03-25T11:11:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 5130.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("58a7f5a0-a3dc-5a37-ac32-1cda02e2e39e"),
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    OrderDate = DateTime.Parse("2026-03-26T10:26:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 3580.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("48b110d4-a678-5117-8241-06bb1abe027c"),
                    CustomerID = Guid.Parse("d46acdf7-49f5-5ebe-8efe-05c042126b5d"),
                    OrderDate = DateTime.Parse("2026-03-27T16:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 6500.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("21c72de8-c537-59b5-b831-363f790d443a"),
                    CustomerID = Guid.Parse("3f9a0889-9fe6-5607-b1d3-105119a109b8"),
                    OrderDate = DateTime.Parse("2026-03-27T13:22:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Shipped",
                    OrderTotal = 680.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("aaf3e39b-b1c8-59ef-8190-5a1b20231d41"),
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    OrderDate = DateTime.Parse("2026-03-30T17:26:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 640.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("3f63cc7c-050d-54b7-b4f6-02251b1f78d0"),
                    CustomerID = Guid.Parse("30e8fc0e-b850-5899-b644-6eca25b03a5a"),
                    OrderDate = DateTime.Parse("2026-03-31T15:22:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Shipped",
                    OrderTotal = 39160.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("e9f24d80-36cb-5ff1-a201-2079f4c9ac0a"),
                    CustomerID = Guid.Parse("42b3b7f2-e52a-5a61-9c2a-73fd128d1b95"),
                    OrderDate = DateTime.Parse("2026-04-01T14:07:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 1580.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("46badf24-c263-5b5b-b9dc-8bed89abfa73"),
                    CustomerID = Guid.Parse("388d2cf0-81c8-5583-9bde-a54d0291d5a0"),
                    OrderDate = DateTime.Parse("2026-04-02T16:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 1090.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("dfaad59c-c44e-5f50-968d-de66ec7bd188"),
                    CustomerID = Guid.Parse("0e7a535f-0698-5033-a5cf-aa1db46219e3"),
                    OrderDate = DateTime.Parse("2026-04-03T13:26:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 1770.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("4a0e8617-7dd9-5417-a185-63d6515eb85a"),
                    CustomerID = Guid.Parse("311d186c-77bd-58d1-803d-a3baa5832f13"),
                    OrderDate = DateTime.Parse("2026-04-03T11:47:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Delivered",
                    OrderTotal = 4070.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("9eae0b19-88d8-5593-ad9b-a6c620022a91"),
                    CustomerID = Guid.Parse("d46acdf7-49f5-5ebe-8efe-05c042126b5d"),
                    OrderDate = DateTime.Parse("2026-04-06T16:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 7380.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("489aaff9-3428-545d-aef4-ff5035b36abe"),
                    CustomerID = Guid.Parse("3f9a0889-9fe6-5607-b1d3-105119a109b8"),
                    OrderDate = DateTime.Parse("2026-04-06T15:34:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Shipped",
                    OrderTotal = 1150.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("45f964d1-74f7-55d7-938b-9cfe7b30324f"),
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    OrderDate = DateTime.Parse("2026-04-08T12:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 8970.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("783aa2b0-c7df-5954-8816-72572b155488"),
                    CustomerID = Guid.Parse("cf7f6c03-fd3b-5a1d-ba4d-26f155e40cfc"),
                    OrderDate = DateTime.Parse("2026-04-08T13:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 430.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("52e85847-a270-5931-8b54-7d1e7dd1df93"),
                    CustomerID = Guid.Parse("42b3b7f2-e52a-5a61-9c2a-73fd128d1b95"),
                    OrderDate = DateTime.Parse("2026-04-09T11:34:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Shipped",
                    OrderTotal = 1750.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("9e2ddb34-3246-57c9-8f2a-b7a8501d2460"),
                    CustomerID = Guid.Parse("1ccacb1d-cc3d-51b3-ac13-a36c4a813862"),
                    OrderDate = DateTime.Parse("2026-04-09T13:13:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 6330.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("9ffeeade-bc9c-5dad-b9dd-36a64f7ba470"),
                    CustomerID = Guid.Parse("0e7a535f-0698-5033-a5cf-aa1db46219e3"),
                    OrderDate = DateTime.Parse("2026-04-10T11:34:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Shipped",
                    OrderTotal = 6570.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("be68bdc0-8832-5e2e-b650-0cda1e0a7c35"),
                    CustomerID = Guid.Parse("0f7e1994-39d7-5629-b423-dd98ea1eb8f6"),
                    OrderDate = DateTime.Parse("2026-04-13T17:41:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Shipped",
                    OrderTotal = 35260.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("bc36e9f1-f417-5a0c-a7d4-8b271ee71146"),
                    CustomerID = Guid.Parse("b18d48ec-4de1-50cf-91a0-af72c3a8ead9"),
                    OrderDate = DateTime.Parse("2026-04-13T17:44:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Processing",
                    OrderTotal = 890.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("894db47b-ded7-5a83-b269-8c3531cb9b0f"),
                    CustomerID = Guid.Parse("b9497a2f-b7b0-59ee-b4c2-20b52401a922"),
                    OrderDate = DateTime.Parse("2026-04-14T11:13:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "Processing",
                    OrderTotal = 3190.00m,
                },
                new Order
                {
                    OrderID = Guid.Parse("2e883132-fb6e-5e5c-bad4-c31542f90acd"),
                    CustomerID = Guid.Parse("be71a251-86d5-57cf-a179-974a07d23db1"),
                    OrderDate = DateTime.Parse("2026-04-15T17:29:00", CultureInfo.InvariantCulture),
                    DeliveryStatus = "In Transit",
                    OrderTotal = 680.00m,
                },
            };
        }

        private static List<OrderDetail> CreateOrderDetails()
        {
            return new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductID = Guid.Parse("550a6126-9886-5858-b6f6-58deebd3a657"),
                    OrderID = Guid.Parse("dd69d17a-304c-55eb-96b9-657919e4d42b"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 4890.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    OrderID = Guid.Parse("085f2e11-08b0-5e5e-810d-b2f97ce90d7d"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 2800.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    OrderID = Guid.Parse("cb85e1e0-e85e-5c28-b74c-0b69e2fcc21e"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 3190.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    OrderID = Guid.Parse("cb85e1e0-e85e-5c28-b74c-0b69e2fcc21e"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 590.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    OrderID = Guid.Parse("d4e616da-1a08-5ca7-b1a4-51cfac7b6b32"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 3380.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("701b3743-5c98-5e45-8a81-ee8bf769ea15"),
                    OrderID = Guid.Parse("e3fa598a-133a-5a4b-9719-fc769bd72aec"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1280.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("2e7ca414-836f-5174-8c7c-4455fb580cd4"),
                    OrderID = Guid.Parse("f59eb226-dd08-5c29-a1d4-e3d4bbcbd024"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 2390.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("2a4f1de8-b2eb-555e-a7d4-6e8c6fbd1d70"),
                    OrderID = Guid.Parse("8abe3c9f-6349-5319-a920-5dd7befdd5ff"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 990.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("97e97f84-88e5-5221-a6f1-434d1566bbcb"),
                    OrderID = Guid.Parse("8abe3c9f-6349-5319-a920-5dd7befdd5ff"),
                    OrderDetailQuantity = 10,
                    OrderDetailAmount = 4100.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    OrderID = Guid.Parse("9ae638b5-4144-55c9-9398-ec8884ca183f"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 790.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    OrderID = Guid.Parse("9ae638b5-4144-55c9-9398-ec8884ca183f"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 2980.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("eacd521b-981a-5879-9c4e-4bc19faa7d6e"),
                    OrderID = Guid.Parse("6d3e031a-dd8c-5b09-ac74-ebdfa7ed22d2"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 520.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    OrderID = Guid.Parse("6d3e031a-dd8c-5b09-ac74-ebdfa7ed22d2"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 3570.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    OrderID = Guid.Parse("514eb9fa-62e4-5653-bd8f-52b6d7b27b8b"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 9570.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("53af2937-b05b-5753-a125-cf664bb7ba8b"),
                    OrderID = Guid.Parse("78c1978a-0877-5383-95ef-4bdc7d5a7c58"),
                    OrderDetailQuantity = 10,
                    OrderDetailAmount = 2700.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    OrderID = Guid.Parse("ad0959a7-9a24-528f-90df-cb6aaa31f205"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1290.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    OrderID = Guid.Parse("c17d7386-9d94-5545-9609-a9d10fd99d70"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1690.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    OrderID = Guid.Parse("c17d7386-9d94-5545-9609-a9d10fd99d70"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1780.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    OrderID = Guid.Parse("0044cde7-12b4-50d8-a01c-18d85712ff01"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 5970.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("d0daaeb5-7169-5bf2-adfb-9ef7149a0fd7"),
                    OrderID = Guid.Parse("0044cde7-12b4-50d8-a01c-18d85712ff01"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 230.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    OrderID = Guid.Parse("7a1e5c56-a69e-5984-a208-1ae0cf31c918"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1490.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    OrderID = Guid.Parse("7a1e5c56-a69e-5984-a208-1ae0cf31c918"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 690.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    OrderID = Guid.Parse("7a1e5c56-a69e-5984-a208-1ae0cf31c918"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1120.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    OrderID = Guid.Parse("a900ab77-c454-5ac7-9a4f-6b14cbdaf6db"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1120.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    OrderID = Guid.Parse("a900ab77-c454-5ac7-9a4f-6b14cbdaf6db"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 2040.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("f29e8709-5006-5eb5-b95a-bb656d0db629"),
                    OrderID = Guid.Parse("6b820c4c-54ef-5e30-b810-aef3ce84092f"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 31200.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    OrderID = Guid.Parse("b612091f-6703-5518-8a78-da6e932b72c1"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 2980.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    OrderID = Guid.Parse("b612091f-6703-5518-8a78-da6e932b72c1"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 2380.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("7989d799-effa-5214-9205-b48e3d4ce254"),
                    OrderID = Guid.Parse("5f1227a4-0ead-5c1c-95f3-d3306123c044"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1690.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    OrderID = Guid.Parse("5f1227a4-0ead-5c1c-95f3-d3306123c044"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 3580.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    OrderID = Guid.Parse("deaad6fb-e671-54f8-88ef-e93787a866b5"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 790.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("550a6126-9886-5858-b6f6-58deebd3a657"),
                    OrderID = Guid.Parse("deaad6fb-e671-54f8-88ef-e93787a866b5"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 14670.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("eacd521b-981a-5879-9c4e-4bc19faa7d6e"),
                    OrderID = Guid.Parse("4edc157b-d35d-5ab1-855d-c8e506b1cc1e"),
                    OrderDetailQuantity = 10,
                    OrderDetailAmount = 2600.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("7989d799-effa-5214-9205-b48e3d4ce254"),
                    OrderID = Guid.Parse("3b6f0a35-7b0c-5618-99f0-858fc9082c9c"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1690.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("32515653-1be6-534a-9670-2a4333191e00"),
                    OrderID = Guid.Parse("3b6f0a35-7b0c-5618-99f0-858fc9082c9c"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1590.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    OrderID = Guid.Parse("3b6f0a35-7b0c-5618-99f0-858fc9082c9c"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 2580.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    OrderID = Guid.Parse("d50a3336-2466-542b-8587-a54640a7dab7"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 2800.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    OrderID = Guid.Parse("1a4c2ff4-61e2-5d0d-b6de-749650f8b960"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 4470.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("49837b1c-aeac-5661-8d07-a97adc41d851"),
                    OrderID = Guid.Parse("1a4c2ff4-61e2-5d0d-b6de-749650f8b960"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 2190.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    OrderID = Guid.Parse("2e43059f-c7ca-569e-b508-cd2082892fa9"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 5370.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("888a2181-f7c0-5886-8f08-fea73f0c0b12"),
                    OrderID = Guid.Parse("2e43059f-c7ca-569e-b508-cd2082892fa9"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 3490.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("65b879b6-a61c-5043-af71-62a7958a4713"),
                    OrderID = Guid.Parse("f87ed5ce-6c07-5e50-86aa-bdf03b94f712"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 960.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    OrderID = Guid.Parse("1bd943e9-4a96-5b89-91b1-68d355f07426"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 690.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("bb6b7911-678d-52c8-82a7-c5720b42c33b"),
                    OrderID = Guid.Parse("b0f203b8-7938-585b-bfe3-592ffc110b90"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1290.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("701b3743-5c98-5e45-8a81-ee8bf769ea15"),
                    OrderID = Guid.Parse("b0f203b8-7938-585b-bfe3-592ffc110b90"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 1920.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("241ce5fb-54ae-554f-b6f7-6edfa6a4290e"),
                    OrderID = Guid.Parse("2052972c-8c60-5d8a-8b9e-af180d3fd5a5"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 28900.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    OrderID = Guid.Parse("2052972c-8c60-5d8a-8b9e-af180d3fd5a5"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1790.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    OrderID = Guid.Parse("2052972c-8c60-5d8a-8b9e-af180d3fd5a5"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 6380.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    OrderID = Guid.Parse("88d99e92-c60e-58c6-a315-ab7bd0d877d8"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 4450.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    OrderID = Guid.Parse("7fbb4654-9416-50ab-a124-4fc8e99d1b6a"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 790.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    OrderID = Guid.Parse("7fbb4654-9416-50ab-a124-4fc8e99d1b6a"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1780.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    OrderID = Guid.Parse("7fbb4654-9416-50ab-a124-4fc8e99d1b6a"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 2800.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    OrderID = Guid.Parse("d3675840-9fb1-5633-8ea9-d17c1d463a72"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 2980.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    OrderID = Guid.Parse("d3675840-9fb1-5633-8ea9-d17c1d463a72"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 2150.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    OrderID = Guid.Parse("58a7f5a0-a3dc-5a37-ac32-1cda02e2e39e"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 3580.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    OrderID = Guid.Parse("48b110d4-a678-5117-8241-06bb1abe027c"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1990.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("069fefd7-707c-59e7-a809-b4e2bcda0482"),
                    OrderID = Guid.Parse("48b110d4-a678-5117-8241-06bb1abe027c"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1560.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    OrderID = Guid.Parse("48b110d4-a678-5117-8241-06bb1abe027c"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 2950.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    OrderID = Guid.Parse("21c72de8-c537-59b5-b831-363f790d443a"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 680.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("65b879b6-a61c-5043-af71-62a7958a4713"),
                    OrderID = Guid.Parse("aaf3e39b-b1c8-59ef-8190-5a1b20231d41"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 640.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1296c8f3-4c34-58bd-8fa5-58e65c09dc6b"),
                    OrderID = Guid.Parse("3f63cc7c-050d-54b7-b4f6-02251b1f78d0"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 8970.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("241ce5fb-54ae-554f-b6f7-6edfa6a4290e"),
                    OrderID = Guid.Parse("3f63cc7c-050d-54b7-b4f6-02251b1f78d0"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 28900.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    OrderID = Guid.Parse("3f63cc7c-050d-54b7-b4f6-02251b1f78d0"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1290.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    OrderID = Guid.Parse("e9f24d80-36cb-5ff1-a201-2079f4c9ac0a"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1580.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("d64b3134-3e6b-5699-aa1c-26cdb6281567"),
                    OrderID = Guid.Parse("46badf24-c263-5b5b-b9dc-8bed89abfa73"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 1090.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    OrderID = Guid.Parse("dfaad59c-c44e-5f50-968d-de66ec7bd188"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 1770.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    OrderID = Guid.Parse("4a0e8617-7dd9-5417-a185-63d6515eb85a"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 2950.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    OrderID = Guid.Parse("4a0e8617-7dd9-5417-a185-63d6515eb85a"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 1120.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("32515653-1be6-534a-9670-2a4333191e00"),
                    OrderID = Guid.Parse("9eae0b19-88d8-5593-ad9b-a6c620022a91"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 4770.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("d37e1ce4-6ae2-5d76-85ad-67959bcf03a1"),
                    OrderID = Guid.Parse("9eae0b19-88d8-5593-ad9b-a6c620022a91"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 2610.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("d0daaeb5-7169-5bf2-adfb-9ef7149a0fd7"),
                    OrderID = Guid.Parse("489aaff9-3428-545d-aef4-ff5035b36abe"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 1150.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("1296c8f3-4c34-58bd-8fa5-58e65c09dc6b"),
                    OrderID = Guid.Parse("45f964d1-74f7-55d7-938b-9cfe7b30324f"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 8970.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    OrderID = Guid.Parse("783aa2b0-c7df-5954-8816-72572b155488"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 430.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("3377495b-f6e6-5b19-aacd-dd4ddce972b4"),
                    OrderID = Guid.Parse("52e85847-a270-5931-8b54-7d1e7dd1df93"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 1750.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("6f81d746-9038-5d87-b0ab-1c40cac51de0"),
                    OrderID = Guid.Parse("9e2ddb34-3246-57c9-8f2a-b7a8501d2460"),
                    OrderDetailQuantity = 5,
                    OrderDetailAmount = 1950.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("49837b1c-aeac-5661-8d07-a97adc41d851"),
                    OrderID = Guid.Parse("9e2ddb34-3246-57c9-8f2a-b7a8501d2460"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 4380.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("49837b1c-aeac-5661-8d07-a97adc41d851"),
                    OrderID = Guid.Parse("9ffeeade-bc9c-5dad-b9dd-36a64f7ba470"),
                    OrderDetailQuantity = 3,
                    OrderDetailAmount = 6570.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("241ce5fb-54ae-554f-b6f7-6edfa6a4290e"),
                    OrderID = Guid.Parse("be68bdc0-8832-5e2e-b650-0cda1e0a7c35"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 28900.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    OrderID = Guid.Parse("be68bdc0-8832-5e2e-b650-0cda1e0a7c35"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 3980.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    OrderID = Guid.Parse("be68bdc0-8832-5e2e-b650-0cda1e0a7c35"),
                    OrderDetailQuantity = 2,
                    OrderDetailAmount = 2380.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("d2a0bcd3-2cac-5be6-a87d-db963ffa37c5"),
                    OrderID = Guid.Parse("bc36e9f1-f417-5a0c-a7d4-8b271ee71146"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 890.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    OrderID = Guid.Parse("894db47b-ded7-5a83-b269-8c3531cb9b0f"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 3190.00m,
                },
                new OrderDetail
                {
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    OrderID = Guid.Parse("2e883132-fb6e-5e5c-bad4-c31542f90acd"),
                    OrderDetailQuantity = 1,
                    OrderDetailAmount = 680.00m,
                },
            };
        }

        private static List<ProductLocation> CreateProductLocations()
        {
            return new List<ProductLocation>
            {
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    ProductQuantity = 10,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    ProductID = Guid.Parse("649827b8-b97d-5e5e-a9fd-4b2d4ba7dc97"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("2e7ca414-836f-5174-8c7c-4455fb580cd4"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("2e7ca414-836f-5174-8c7c-4455fb580cd4"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("74572931-fb31-5c98-a97a-97ba2a3916c0"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    ProductQuantity = 2,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("c6cf4140-6f94-5b69-a0df-9618b85d9b2e"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("982ce262-b2ce-5167-b85e-1d84783d4ebc"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("550a6126-9886-5858-b6f6-58deebd3a657"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("550a6126-9886-5858-b6f6-58deebd3a657"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("c2d3ccfe-6b55-519a-86e4-12b0d73f39f8"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("2a4f1de8-b2eb-555e-a7d4-6e8c6fbd1d70"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("2a4f1de8-b2eb-555e-a7d4-6e8c6fbd1d70"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("2a4f1de8-b2eb-555e-a7d4-6e8c6fbd1d70"),
                    ProductQuantity = 11,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("d2a0bcd3-2cac-5be6-a87d-db963ffa37c5"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("d2a0bcd3-2cac-5be6-a87d-db963ffa37c5"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("d2a0bcd3-2cac-5be6-a87d-db963ffa37c5"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("ab86be50-e643-5b33-b52b-9491d0ea5680"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("bb6b7911-678d-52c8-82a7-c5720b42c33b"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("bb6b7911-678d-52c8-82a7-c5720b42c33b"),
                    ProductQuantity = 2,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("32515653-1be6-534a-9670-2a4333191e00"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("32515653-1be6-534a-9670-2a4333191e00"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("32515653-1be6-534a-9670-2a4333191e00"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("d64b3134-3e6b-5699-aa1c-26cdb6281567"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("d64b3134-3e6b-5699-aa1c-26cdb6281567"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("d64b3134-3e6b-5699-aa1c-26cdb6281567"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("ce678276-912f-525b-8218-59627be7d149"),
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    ProductQuantity = 11,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    ProductID = Guid.Parse("7989d799-effa-5214-9205-b48e3d4ce254"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9e70a202-0980-5679-87ca-f846d9be7f2c"),
                    ProductID = Guid.Parse("7989d799-effa-5214-9205-b48e3d4ce254"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("3102e073-185b-59dd-bf30-e7bf18f1ef8e"),
                    ProductID = Guid.Parse("7989d799-effa-5214-9205-b48e3d4ce254"),
                    ProductQuantity = 10,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("49837b1c-aeac-5661-8d07-a97adc41d851"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    ProductID = Guid.Parse("49837b1c-aeac-5661-8d07-a97adc41d851"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    ProductID = Guid.Parse("241ce5fb-54ae-554f-b6f7-6edfa6a4290e"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("f29e8709-5006-5eb5-b95a-bb656d0db629"),
                    ProductQuantity = 1,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("a36ec316-987f-597b-aaad-8f3b9905806c"),
                    ProductID = Guid.Parse("888a2181-f7c0-5886-8f08-fea73f0c0b12"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("290c4f1f-5b00-5701-8ff1-407bfc08c780"),
                    ProductID = Guid.Parse("888a2181-f7c0-5886-8f08-fea73f0c0b12"),
                    ProductQuantity = 2,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("bdba8210-6d58-5bea-9828-cc02be235571"),
                    ProductID = Guid.Parse("1296c8f3-4c34-58bd-8fa5-58e65c09dc6b"),
                    ProductQuantity = 0,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("069fefd7-707c-59e7-a809-b4e2bcda0482"),
                    ProductQuantity = 10,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("069fefd7-707c-59e7-a809-b4e2bcda0482"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    ProductID = Guid.Parse("069fefd7-707c-59e7-a809-b4e2bcda0482"),
                    ProductQuantity = 13,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("4ddeb0e5-975c-5777-82c6-602057f15e41"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("4ddeb0e5-975c-5777-82c6-602057f15e41"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("4ddeb0e5-975c-5777-82c6-602057f15e41"),
                    ProductQuantity = 12,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("6f81d746-9038-5d87-b0ab-1c40cac51de0"),
                    ProductQuantity = 15,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("6f81d746-9038-5d87-b0ab-1c40cac51de0"),
                    ProductQuantity = 11,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("6f81d746-9038-5d87-b0ab-1c40cac51de0"),
                    ProductQuantity = 19,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    ProductQuantity = 18,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    ProductQuantity = 13,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("560de5b9-d5fd-5f38-8189-d70fe93f9181"),
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    ProductQuantity = 21,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("97e97f84-88e5-5221-a6f1-434d1566bbcb"),
                    ProductQuantity = 13,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("97e97f84-88e5-5221-a6f1-434d1566bbcb"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("97e97f84-88e5-5221-a6f1-434d1566bbcb"),
                    ProductQuantity = 16,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("3377495b-f6e6-5b19-aacd-dd4ddce972b4"),
                    ProductQuantity = 11,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("3377495b-f6e6-5b19-aacd-dd4ddce972b4"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("3377495b-f6e6-5b19-aacd-dd4ddce972b4"),
                    ProductQuantity = 14,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("560de5b9-d5fd-5f38-8189-d70fe93f9181"),
                    ProductID = Guid.Parse("65b879b6-a61c-5043-af71-62a7958a4713"),
                    ProductQuantity = 14,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("65b879b6-a61c-5043-af71-62a7958a4713"),
                    ProductQuantity = 10,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("65b879b6-a61c-5043-af71-62a7958a4713"),
                    ProductQuantity = 16,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("a7f088ef-23fc-557f-b470-818b2c999819"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("d37e1ce4-6ae2-5d76-85ad-67959bcf03a1"),
                    ProductQuantity = 3,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("d37e1ce4-6ae2-5d76-85ad-67959bcf03a1"),
                    ProductQuantity = 1,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("6fe3308e-c389-5ebb-9a49-ef010db52236"),
                    ProductQuantity = 10,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    ProductID = Guid.Parse("701b3743-5c98-5e45-8a81-ee8bf769ea15"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("560de5b9-d5fd-5f38-8189-d70fe93f9181"),
                    ProductID = Guid.Parse("701b3743-5c98-5e45-8a81-ee8bf769ea15"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("701b3743-5c98-5e45-8a81-ee8bf769ea15"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("53af2937-b05b-5753-a125-cf664bb7ba8b"),
                    ProductQuantity = 12,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("53af2937-b05b-5753-a125-cf664bb7ba8b"),
                    ProductQuantity = 9,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("53af2937-b05b-5753-a125-cf664bb7ba8b"),
                    ProductQuantity = 15,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("eacd521b-981a-5879-9c4e-4bc19faa7d6e"),
                    ProductQuantity = 10,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("eacd521b-981a-5879-9c4e-4bc19faa7d6e"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    ProductID = Guid.Parse("eacd521b-981a-5879-9c4e-4bc19faa7d6e"),
                    ProductQuantity = 14,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("3611640a-7c2c-5211-93bd-93e7ee9b55fc"),
                    ProductQuantity = 7,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    ProductID = Guid.Parse("3611640a-7c2c-5211-93bd-93e7ee9b55fc"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("560de5b9-d5fd-5f38-8189-d70fe93f9181"),
                    ProductID = Guid.Parse("3611640a-7c2c-5211-93bd-93e7ee9b55fc"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("e1298768-f893-5231-8887-d9b8e9286c5d"),
                    ProductID = Guid.Parse("d0daaeb5-7169-5bf2-adfb-9ef7149a0fd7"),
                    ProductQuantity = 6,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("4863ca60-8dbe-597f-9482-b44df88d7930"),
                    ProductID = Guid.Parse("d0daaeb5-7169-5bf2-adfb-9ef7149a0fd7"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("560de5b9-d5fd-5f38-8189-d70fe93f9181"),
                    ProductID = Guid.Parse("d0daaeb5-7169-5bf2-adfb-9ef7149a0fd7"),
                    ProductQuantity = 8,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("803f2419-2cbe-50ed-b08c-dc013f845459"),
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    ProductQuantity = 5,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("9524748f-7232-5038-85b5-0f69fcd785f6"),
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    ProductQuantity = 4,
                },
                new ProductLocation
                {
                    LocationID = Guid.Parse("2e237553-a5e2-58c8-80e1-cd9067d86788"),
                    ProductID = Guid.Parse("1b1502de-1847-50b3-9520-8578915a3887"),
                    ProductQuantity = 7,
                },
            };
        }

        private static List<PurchaseReceipt> CreatePurchaseReceipts()
        {
            return new List<PurchaseReceipt>
            {
                new PurchaseReceipt
                {
                    PurchaseReceiptID = Guid.Parse("13370f16-f1dc-4a7a-84b0-59f1f44d0001"),
                    SupplierID = Guid.Parse("ddde6b86-cb9d-59cb-91a8-50f74303af87"),
                    ProductID = Guid.Parse("7f840b26-94e7-587e-9774-1f6e376bdfb2"),
                    WarehouseID = Guid.Parse("84900bc7-be71-5036-87ca-245626fb8e3c"),
                    DocumentNumber = "ПР-0001",
                    Quantity = 25,
                    UnitPrice = 1450.00m,
                    TotalAmount = 36250.00m,
                    PurchasedAt = DateTime.Parse("2026-05-20T10:00:00", CultureInfo.InvariantCulture),
                    Comment = "Начальный приход товара"
                },
                new PurchaseReceipt
                {
                    PurchaseReceiptID = Guid.Parse("13370f16-f1dc-4a7a-84b0-59f1f44d0002"),
                    SupplierID = Guid.Parse("b1114d18-d8cc-5ca6-8baa-389457a3b244"),
                    ProductID = Guid.Parse("2e7ca414-836f-5174-8c7c-4455fb580cd4"),
                    WarehouseID = Guid.Parse("0935fa03-15a5-50ce-885e-f6833571d7ed"),
                    DocumentNumber = "ПР-0002",
                    Quantity = 15,
                    UnitPrice = 2650.00m,
                    TotalAmount = 39750.00m,
                    PurchasedAt = DateTime.Parse("2026-05-21T12:30:00", CultureInfo.InvariantCulture),
                    Comment = "Пополнение склада"
                }
            };
        }

        private static List<Defective> CreateDefectives()
        {
            return new List<Defective>
            {
                new Defective
                {
                    DefectiveID = Guid.Parse("f0e575c9-3ba8-52d8-a7e8-7fa1c9ebf154"),
                    ProductID = Guid.Parse("241ce5fb-54ae-554f-b6f7-6edfa6a4290e"),
                    Quantity = 1,
                    DateDeclared = DateTime.Parse("2026-03-03T11:17:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("cd8550b1-5ae1-5cdf-ab60-c19f0d46ecb0"),
                    ProductID = Guid.Parse("d2a0bcd3-2cac-5be6-a87d-db963ffa37c5"),
                    Quantity = 2,
                    DateDeclared = DateTime.Parse("2026-02-24T15:26:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("0403acbc-ddcd-5e3e-929b-e150dcfee5bc"),
                    ProductID = Guid.Parse("af8561cb-fd5f-5c69-84a8-d4fc7608ca2b"),
                    Quantity = 1,
                    DateDeclared = DateTime.Parse("2026-03-18T14:41:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("f9ef8521-4f1b-5623-8c82-fdc5ac76116d"),
                    ProductID = Guid.Parse("26ac85a1-197a-5273-80d6-c7a0f829b34d"),
                    Quantity = 4,
                    DateDeclared = DateTime.Parse("2026-04-02T10:34:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("b5eac938-a2a1-52fd-b582-4637aaa9221b"),
                    ProductID = Guid.Parse("913baf1a-6c07-5274-be04-63c1b9ddad02"),
                    Quantity = 2,
                    DateDeclared = DateTime.Parse("2026-04-07T16:29:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("f7a85030-f5c4-5c83-af99-d140415ad9bd"),
                    ProductID = Guid.Parse("888a2181-f7c0-5886-8f08-fea73f0c0b12"),
                    Quantity = 1,
                    DateDeclared = DateTime.Parse("2026-03-27T13:22:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("97692a6c-dfd3-5101-a7f1-76357b7a2e42"),
                    ProductID = Guid.Parse("d64b3134-3e6b-5699-aa1c-26cdb6281567"),
                    Quantity = 1,
                    DateDeclared = DateTime.Parse("2026-02-19T12:47:00", CultureInfo.InvariantCulture),
                },
                new Defective
                {
                    DefectiveID = Guid.Parse("fdb3b6b6-3e4f-515a-941a-f47f3c5ca78e"),
                    ProductID = Guid.Parse("fc9744f3-b31a-51a4-a760-d3d049d8f8f1"),
                    Quantity = 1,
                    DateDeclared = DateTime.Parse("2026-04-10T11:13:00", CultureInfo.InvariantCulture),
                },
            };
        }

        private static List<Log> CreateLogs()
        {
            return new List<Log>
            {
                new Log
                {
                    LogID = Guid.Parse("138c3553-54b4-5f4f-a911-96a733e7cef0"),
                    StaffID = Guid.Parse("4be43b93-b4cb-5c93-b1b6-434251fd7e7d"),
                    LogCategory = "CATEGORIES",
                    ActionType = "CREATE",
                    LogDetails = "Созданы категории Орто и Уро",
                    DateTime = DateTime.Parse("2026-02-10T10:11:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("4d1f83b9-fe2b-5691-9851-42dbc0c912a5"),
                    StaffID = Guid.Parse("4be43b93-b4cb-5c93-b1b6-434251fd7e7d"),
                    LogCategory = "STAFFS",
                    ActionType = "CREATE",
                    LogDetails = "Добавлены роли и стартовый персонал",
                    DateTime = DateTime.Parse("2026-02-10T10:26:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("6de7263c-ab48-5317-a7f0-27bb39c25b4a"),
                    StaffID = Guid.Parse("38841fb3-3d9e-530e-87ac-405843ba1e82"),
                    LogCategory = "SUPPLIERS",
                    ActionType = "CREATE",
                    LogDetails = "Добавлен пул поставщиков по направлениям Орто и Уро",
                    DateTime = DateTime.Parse("2026-02-10T10:37:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("824c4be4-f212-5e8c-8b68-89fa6f6ef34e"),
                    StaffID = Guid.Parse("38841fb3-3d9e-530e-87ac-405843ba1e82"),
                    LogCategory = "PRODUCTS",
                    ActionType = "CREATE",
                    LogDetails = "Первичное заполнение справочника товаров",
                    DateTime = DateTime.Parse("2026-02-10T11:17:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("957838a6-84cd-51c1-b687-c3ca40e63912"),
                    StaffID = Guid.Parse("38841fb3-3d9e-530e-87ac-405843ba1e82"),
                    LogCategory = "STORAGES",
                    ActionType = "ADD_STOCK",
                    LogDetails = "Разнесены остатки по магазинам и складу",
                    DateTime = DateTime.Parse("2026-02-10T11:41:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("90d52b5e-3266-5929-bf27-3e00d96662a5"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ dd69d17a-304c-55eb-96b9-657919e4d42b со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-10T11:41:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("3291c128-d44c-5f19-94db-e74775c16dd5"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 085f2e11-08b0-5e5e-810d-b2f97ce90d7d со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-10T10:37:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("3aa15637-7783-5b8f-aaac-431af1b714d8"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ cb85e1e0-e85e-5c28-b74c-0b69e2fcc21e со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-12T10:52:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("664b2022-60a0-5fe5-ae38-6ebd0121a619"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ d4e616da-1a08-5ca7-b1a4-51cfac7b6b32 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-13T15:41:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("480d5c85-855a-5343-a6a4-9a07814fe2c2"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ e3fa598a-133a-5a4b-9719-fc769bd72aec со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-16T11:37:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("ffcf8b2d-80fe-5404-951c-a0e950633548"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ f59eb226-dd08-5c29-a1d4-e3d4bbcbd024 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-16T10:44:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("290e37de-9b74-54fb-ad83-9e19df0615d5"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 8abe3c9f-6349-5319-a920-5dd7befdd5ff со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-17T17:44:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("4477cd93-5d11-5080-bc29-05496b594105"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 9ae638b5-4144-55c9-9398-ec8884ca183f со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-23T16:22:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("22f76fe1-c7bb-5e44-af22-64f83925b641"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 6d3e031a-dd8c-5b09-ac74-ebdfa7ed22d2 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-24T15:17:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("ca8f011f-8c66-574e-8e3b-63ba70adb4c2"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 514eb9fa-62e4-5653-bd8f-52b6d7b27b8b со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-25T16:41:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("2a4b14b4-82c6-50b2-96c9-a8ec6b6c8d17"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 78c1978a-0877-5383-95ef-4bdc7d5a7c58 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-25T10:56:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("b031b993-b213-5e67-8cc8-e0dd3ebaf751"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ ad0959a7-9a24-528f-90df-cb6aaa31f205 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-26T17:37:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("e3bcca0e-cd0c-5286-8058-da5c8acfa921"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ c17d7386-9d94-5545-9609-a9d10fd99d70 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-02-27T14:52:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("ea1aeec3-137c-585b-b612-5fe15a4331fb"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 0044cde7-12b4-50d8-a01c-18d85712ff01 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-04T14:56:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("7e0975b6-a973-5e29-b3db-067e43ef406c"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 7a1e5c56-a69e-5984-a208-1ae0cf31c918 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-06T11:26:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("2107ae79-5023-5412-86e8-c73cdc93e274"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ a900ab77-c454-5ac7-9a4f-6b14cbdaf6db со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-09T12:22:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("af716295-a79f-5709-9c50-0b97e3ab3ca7"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 6b820c4c-54ef-5e30-b810-aef3ce84092f со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-10T11:17:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("415d2eb2-9660-510b-be62-1e1a4d11260d"),
                    StaffID = Guid.Parse("5fb020d5-fd99-5f34-80cd-e62430420486"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ b612091f-6703-5518-8a78-da6e932b72c1 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-11T10:11:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("bf2dc4aa-34bf-525e-af1d-7c3e9fa1f92f"),
                    StaffID = Guid.Parse("627cf963-adef-5bc7-8abf-dddd621767e4"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ 5f1227a4-0ead-5c1c-95f3-d3306123c044 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-12T12:47:00", CultureInfo.InvariantCulture),
                },
                new Log
                {
                    LogID = Guid.Parse("2dffeb08-3f08-5c6c-8e0f-c7569eee0701"),
                    StaffID = Guid.Parse("c9e58eda-2e44-5446-97c5-e3b8f85b8e66"),
                    LogCategory = "ORDERS",
                    ActionType = "CREATE",
                    LogDetails = "Создан заказ deaad6fb-e671-54f8-88ef-e93787a866b5 со статусом Delivered",
                    DateTime = DateTime.Parse("2026-03-13T10:44:00", CultureInfo.InvariantCulture),
                },
            };
        }

        private static List<InternalMessage> CreateMessages()
        {
            return new List<InternalMessage>
            {
            };
        }
    }
}
