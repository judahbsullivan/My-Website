# Testing Supabase Migration

## Steps to Test

1. **Restart API Server**
   ```bash
   # Stop current server (Ctrl+C)
   # Then restart:
   cd api
   dotnet watch run
   ```

2. **Verify Environment Variables**
   Make sure your `.env` file (in project root) has:
   ```
   SUPABASE_URL=https://csfuzhtznhkdzytfotet.supabase.co
   SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   SUPABASE_SERVICE_ROLE_KEY=<your-service-role-key>
   ```

3. **Test Endpoints**
   ```bash
   # Test featured projects
   curl http://localhost:5266/api/projects/featured
   
   # Test all projects
   curl http://localhost:5266/api/projects
   
   # Test single project (replace with actual slug)
   curl http://localhost:5266/api/projects/your-project-slug
   ```

## Expected Results

- **Success**: Should return JSON array of projects (200 status)
- **Error**: Check console logs for:
  - Supabase connection errors
  - RLS policy errors (if using anon key instead of service_role)
  - Query syntax errors

## Common Issues

1. **"SUPABASE_URL missing"** - Check `.env` file location and variable names
2. **"RLS policy violation"** - Use `SUPABASE_SERVICE_ROLE_KEY` instead of anon key for server-side
3. **"Table not found"** - Verify table names match exactly (case-sensitive)
4. **Query syntax errors** - Check Supabase .NET v1.1.1 documentation

## Next Steps After Testing

Once `/api/projects/featured` works:
- Migrate `BlogController`
- Migrate `PlaygroundController`  
- Migrate `ContactController`
- Set up Supabase Auth
