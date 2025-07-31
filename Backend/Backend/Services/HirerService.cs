using Backend.DTOs.Users.Hirer;
using Backend.Repositories;

namespace Backend.Services;

public class HirerService (AddressRepository addressRepository, UserRepository userRepository) {
    public async Task RegisterUserAsHirerAsync(Guid userId, HirerProfessionalDetailsDto dto) {
        // TODO - Check if he/she isn't already a hirer, fetch the record with userId and check it's values....
        int addressId = await addressRepository.CreateAddressAsync(dto.CompanyAddress);
        await userRepository.RegisterUserAsHirerAsync(userId, dto, addressId);
    }    
}