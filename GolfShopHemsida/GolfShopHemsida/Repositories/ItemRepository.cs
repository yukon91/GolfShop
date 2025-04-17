
using GolfShopHemsida.Data;
using GolfShopHemsida.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfShopHemsida.Repositories;

public class ItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddItemAsync(Item item)
    {
        item.ItemId = Guid.NewGuid().ToString(); // Auto-generate ID
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _context.Items.OrderByDescending(i => i.CreatedAt).ToListAsync();
    }

    public async Task<Item> GetItemByIdAsync(string id)
    {
        return await _context.Items.FindAsync(id);
    }

    public async Task UpdateItemAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteItemAsync(string id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
