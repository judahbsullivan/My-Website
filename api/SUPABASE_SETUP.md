# Supabase C# Setup Guide

This document provides a complete guide for installing and configuring the official Supabase C# library in this .NET project, following the [official Supabase C# documentation](https://supabase.com/docs/reference/csharp/installing).

## ✅ Verification

### .NET Version
- **Required**: .NET 6.0 or later
- **Current**: .NET 10.0 ✅
- **Status**: Compatible

### Supabase Package
- **Package Name**: `Supabase` (official package from NuGet)
- **Current Version**: 1.1.1
- **Installation**: Already installed in `api.csproj`

```xml
<PackageReference Include="Supabase" Version="1.1.1" />
```

## 📁 Project Structure

```
api/
├── Services/
│   ├── SupabaseService.cs          # Main Supabase service wrapper
│   └── Examples/
│       └── SupabaseCrudExamples.cs # CRUD operation examples
├── Models/
│   ├── Supabase/                   # Production Supabase models
│   │   ├── ProjectSupabase.cs
│   │   ├── BlogPostSupabase.cs
│   │   └── ...
│   └── Examples/
│       └── UserExample.cs          # Example model with BaseModel
├── Examples/
│   └── GetUsersExample.cs          # Working example script
└── Program.cs                       # Application entry point
```

## 🔧 Configuration

### Environment Variables

Set these in your `.env` file (or environment):

```env
# Official docs pattern (anon key)
SUPABASE_URL=https://your-project-id.supabase.co
SUPABASE_KEY=your-anon-key

# OR for server-side APIs (recommended for backend):
SUPABASE_URL=https://your-project-id.supabase.co
SUPABASE_SERVICE_ROLE_KEY=your-service-role-key
```

**Note**: The official docs use `SUPABASE_KEY` (anon key). For server-side APIs, `SUPABASE_SERVICE_ROLE_KEY` is recommended as it bypasses RLS and provides elevated privileges.

### Initialization Pattern

The Supabase client is initialized following the official pattern:

```csharp
// 1. Create client with URL and key
var client = new Supabase.Client(supabaseUrl, supabaseKey, options);

// 2. Initialize asynchronously
await client.InitializeAsync();
```

See `Program.cs` for the complete implementation.

## 📝 Using BaseModel and Table Attributes

All Supabase models must:
1. Inherit from `BaseModel`
2. Use `[Table]` attribute to map to database table
3. Use `[PrimaryKey]` for primary key columns
4. Use `[Column]` for all table columns

### Example Model

```csharp
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("users")]
public class UserExample : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("name")]
    public string? Name { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

See `Models/Examples/UserExample.cs` for a complete example.

## 🚀 CRUD Operations

### Using SupabaseService

Inject `SupabaseService` into your controllers or services:

```csharp
public class MyController : ControllerBase
{
    private readonly SupabaseService _supabase;

    public MyController(SupabaseService supabase)
    {
        _supabase = supabase;
    }
}
```

### SELECT (Read)

```csharp
// Get all records
var response = await _supabase
    .From<UserExample>()
    .Get();
var users = response.Models;

// Get with filter
var user = await _supabase
    .From<UserExample>()
    .Where(x => x.Email == "user@example.com")
    .Single();

// Get with ordering and limit
var recent = await _supabase
    .From<UserExample>()
    .Order(x => x.CreatedAt, Constants.Ordering.Descending)
    .Limit(10)
    .Get();
```

### INSERT (Create)

```csharp
var newUser = new UserExample
{
    Email = "new@example.com",
    Name = "New User"
};

var response = await _supabase
    .From<UserExample>()
    .Insert(newUser);

var createdUser = response.Models.FirstOrDefault();
```

### UPDATE

```csharp
var response = await _supabase
    .From<UserExample>()
    .Where(x => x.Id == userId)
    .Set(x => x.Name, "Updated Name")
    .Update();
```

### DELETE

```csharp
var response = await _supabase
    .From<UserExample>()
    .Where(x => x.Id == userId)
    .Delete();
```

### UPSERT

```csharp
var response = await _supabase
    .From<UserExample>()
    .Upsert(user);
```

See `Services/Examples/SupabaseCrudExamples.cs` for complete CRUD examples.

## 📖 Working Example

A complete working example is available in `Examples/GetUsersExample.cs`:

```csharp
var example = new GetUsersExample(supabaseService);
await example.RunExampleAsync();
```

This example:
1. Connects to Supabase
2. Executes a simple query (`client.From<Table>().Get()`)
3. Logs the results cleanly

## 🔗 Official Documentation References

- [Installing](https://supabase.com/docs/reference/csharp/installing)
- [Initializing](https://supabase.com/docs/reference/csharp/initializing)
- [Select (Fetch data)](https://supabase.com/docs/reference/csharp/select)
- [Insert](https://supabase.com/docs/reference/csharp/insert)
- [Update](https://supabase.com/docs/reference/csharp/update)
- [Delete](https://supabase.com/docs/reference/csharp/delete)
- [Using filters](https://supabase.com/docs/reference/csharp/using-filters)
- [Using modifiers](https://supabase.com/docs/reference/csharp/using-modifiers)

## ✅ Best Practices

1. **Always use async/await** - All Supabase operations are asynchronous
2. **Use BaseModel** - All models must inherit from `BaseModel`
3. **Use attributes** - Always use `[Table]`, `[PrimaryKey]`, and `[Column]`
4. **Service Role Key for Backend** - Use `SUPABASE_SERVICE_ROLE_KEY` for server-side APIs
5. **Error Handling** - Always wrap Supabase calls in try-catch blocks
6. **Type Safety** - Use strongly-typed models instead of dynamic queries

## 🎯 Production Ready

This implementation is production-ready with:
- ✅ Strongly typed models
- ✅ Proper dependency injection
- ✅ Async/await patterns
- ✅ Error handling
- ✅ Following official documentation
- ✅ Clean separation of concerns
- ✅ Example code for reference

