using WebApi.Entities;

namespace WebApi.Data
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            List<Product> products = Enumerable.Range(1, 10)
                .Select(i => new Product()
                {
                    Name = $"{i} product",
                    Description = $"{i} product description",
                    Price = i * Random.Shared.Next(5, 100),
                })
                .ToList();

            _context.Products.AddRange(products);

            List<Order> orders = Enumerable.Range(1, 5)
                .Select(i => new Order()
                {
                    Status = Enums.OrderStatus.Pending,
                    StatusChangedAt = DateTime.Now,
                })
                .ToList();

            foreach(var order in orders)
            {
                HashSet<Product> usedProducts = new HashSet<Product>();
                int productsCount = Random.Shared.Next(1, 5);
                for (int i = 0; i < productsCount; i++)
                {
                    Product product = null;
                    while (product is null || usedProducts.Contains(product))
                    {
                        product = products[Random.Shared.Next(0, products.Count - 1)];
                    }
                    usedProducts.Add(product);
                    order.ProductsInOrder.Add(new ProductInOrder()
                    {
                        Amount = Random.Shared.Next(1, 10),
                        Product = product,
                        Order = order,
                    });
                }

                order.OrderTotal = order.ProductsInOrder.Sum(p => p.Amount * products[p.ProductId].Price);
            }

            _context.AddRange(orders);

            _context.SaveChanges();
        }
    }
}
