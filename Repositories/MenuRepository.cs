using brunchie_backend.DataBase;
using brunchie_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace brunchie_backend.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly AppDbContext _context;
        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddItems(IEnumerable<MenuItemAddDto> items)
        {

            try
            {
                var menuItems = items.Select(mItem => new MenuItem
                {
                    VendorId = mItem.VendorId,
                    Name = mItem.Name,
                    Description = mItem.Description,
                    Price = mItem.Price,
                    IsAvailable = mItem.IsAvailable,
                    ImageUrl = mItem.ImageUrl

                }).ToList();

                await _context.MenuItem.AddRangeAsync(menuItems);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new ApplicationException("An error occurred while updating the database. Please check the data and try again.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding menu items.", ex);
            }
        }

        public async Task<IEnumerable<MenuItemRepDto>> GetMenu (string VendorId)
        {

            
                var MenuItems = await _context.MenuItem
                               .Where(m => m.VendorId == VendorId)
                               .Select(m => new MenuItemRepDto
                               {
                                   Name = m.Name,
                                   Description = m.Description,
                                   Price = m.Price,
                                   ImageUrl = m.ImageUrl,
                                   IsAvailable = m.IsAvailable
                               }).ToListAsync();

                return MenuItems;

            
        }

        public async Task<IEnumerable<int>> RemoveItems(IEnumerable<int> ItemId)
        {

            List<int> failed = new List<int>();
            foreach (var Id in ItemId)
            {

                try
                {
                    var menuItem = await _context.MenuItem.FindAsync(Id);
                    
                    _context.MenuItem.Remove(menuItem);
                    await _context.SaveChangesAsync();
                    
                }
                catch (Exception ex)
                {
                    failed.Add(Id);
                }
            }

            return failed;
        }



            



    }

}









