using Microsoft.EntityFrameworkCore;
using ProductManagement.Interfaces;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductManagementDbContext _context;

        public ProductRepository(ProductManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy tất cả sản phẩm
        /// </summary>
        /// <returns></returns>
        /// Created by: TMSANG (21/06/2020)
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Product.OrderByDescending(product => product.ModifiedDate).ToList();
        }

        /// <summary>
        /// Lấy 1 sản phẩm theo Id
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns></returns>
        /// Created by: TMSANG (21/06/2020)
        public Product GetProductById(int id)
        {
            return _context.Product.Find(id);
        }

        /// <summary>
        /// Thêm 1 sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm</param>
        /// <returns></returns>
        /// Created by: TMSANG (21/06/2020)
        public bool InsertProduct(Product product)
        {
            if (product == null)
                throw new System.ArgumentNullException(nameof(product));
            _context.Product.Add(product);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Kiểm tra sản phẩm có Id đã tồn tại hay chưa
        /// </summary>
        /// <param name="id">Id của sản phẩm cần kiểm tra</param>
        /// <returns></returns>
        /// Created by: TMSANG (21/06/2020)
        public bool IsExistedProduct(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        /// <summary>
        /// Cập nhật 1 sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cập nhật</param>
        /// <returns></returns>
        /// Created by: TMSANG (21/06/2020)
        public bool UpdateProduct(Product product)
        {
            if (product == null)
                throw new System.ArgumentNullException(nameof(product));
            if (product.Id <= 0)
                return false;
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Xóa 1 sản phẩm theo Id
        /// </summary>
        /// <param name="id">Id của sản phẩm muốn xóa</param>
        /// <returns></returns>
        /// Created by: TMSANG (21/06/2020)
        public bool DeleteProductById(int id)
        {
            var product = _context.Product.Find(id);
            if (product == null)
                return false;
            _context.Product.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}
