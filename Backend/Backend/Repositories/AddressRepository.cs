using Backend.Data;
using Backend.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AddressRepository (ApplicationDbContext context) {
    public async Task<int> CreateAddressAsync(Address address) {
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();
        return address.Id;
    }

    public async Task<Address> GetAddressAsync(int addressId) {
        Address address = await context.Addresses.Where(address => address.Id == addressId).FirstOrDefaultAsync();
        if (address == null) {
            throw new Exception("Address not found");
        }
        return address;
    }

    // TODO - Make this better... Limit what is actually editable. Also, compare it first, if there's any change, only then edit... Could backfire however
    public async Task UpdateAddressesAsync(Address address) {
        context.Addresses.Attach(address);
        context.Entry(address).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}