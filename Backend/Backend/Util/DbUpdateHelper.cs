using Microsoft.EntityFrameworkCore;

namespace Backend.Util;

public static class DbUpdateHelper {
    public static async Task<bool> UpdateAllFieldsExceptAsync<TEntity>(TEntity entity, DbContext context, params string[] excludedProperties) where TEntity : class
    {
        context.Attach(entity);
        var entry = context.Entry(entity);

        foreach (var property in entry.Properties)
        {
            if (!excludedProperties.Contains(property.Metadata.Name))
            {
                property.IsModified = true;
            }
        }

        if (await context.SaveChangesAsync() == 1) {
            return true;
        }

        return false;
    }
}