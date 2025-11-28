# 📚 MyPortfolio Documentation

<div align="center">

![Documentation](https://img.shields.io/badge/Docs-Comprehensive-blue?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-In_Progress-yellow?style=for-the-badge)

</div>

---

## 📖 Table of Contents

| Document | Description |
|----------|-------------|
| [🗺️ Roadmap](./ROADMAP.md) | Project milestones and future plans |
| [📝 Changelog](./CHANGELOG.md) | Version history and updates |
| [🔧 API Reference](./API.md) | Complete API documentation |
| [🎨 Frontend Guide](./FRONTEND.md) | Nuxt/Vue development guide |
| [🔐 Authentication](./AUTH.md) | Auth flow and security |

---

## 🚀 Getting Started

### Development Environment

#### Backend (.NET API)

```bash
cd api

# Restore packages
dotnet restore

# Apply database migrations
dotnet ef database update

# Run in development mode
dotnet watch run
```

The API will be available at `http://localhost:5266`

#### Frontend (Nuxt)

```bash
cd web

# Install dependencies
bun install  # or npm install

# Run development server
bun dev  # or npm run dev
```

The frontend will be available at `http://localhost:3000`

---

## 🔑 Environment Variables

### Root `.env` File

```env
# ═══════════════════════════════════════════════════════════════
# DATABASE CONFIGURATION (Supabase PostgreSQL)
# ═══════════════════════════════════════════════════════════════
DB_HOST=db.your-project.supabase.co
DB_PORT=5432
DB_NAME=postgres
DB_USER=postgres
DB_PASSWORD=your_secure_password

# ═══════════════════════════════════════════════════════════════
# JWT AUTHENTICATION
# ═══════════════════════════════════════════════════════════════
JWT_KEY=your_super_secret_key_minimum_32_characters_long
JWT_ISSUER=MyPortfolio
JWT_AUDIENCE=MyPortfolioUsers
JWT_EXPIRE_MINUTES=60
```

### Frontend Environment (Optional)

Create `web/.env` for frontend-specific config:

```env
NUXT_PUBLIC_API_BASE=http://localhost:5266
```

---

## 🏗️ Architecture Deep Dive

### Authentication Flow

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│              │     │              │     │              │
│    Client    │────▶│   Nuxt App   │────▶│   .NET API   │
│   Browser    │     │   (SSR/CSR)  │     │   (REST)     │
│              │◀────│              │◀────│              │
└──────────────┘     └──────────────┘     └──────────────┘
                            │                     │
                            │                     │
                     ┌──────▼──────┐       ┌──────▼──────┐
                     │             │       │             │
                     │ localStorage│       │  Supabase   │
                     │   (Token)   │       │ PostgreSQL  │
                     │             │       │             │
                     └─────────────┘       └─────────────┘
```

### Data Flow

1. **Registration**: User data → API → Database → JWT returned
2. **Login**: Credentials → API validation → JWT returned
3. **Protected Routes**: JWT in header → API validates → Data returned
4. **Token Refresh**: (Future) Refresh token flow

---

## 📊 Database Schema

### Users (ASP.NET Identity)

| Column | Type | Description |
|--------|------|-------------|
| `Id` | `text` | Primary key (GUID) |
| `Email` | `varchar(256)` | User email |
| `UserName` | `varchar(256)` | Username (same as email) |
| `DisplayName` | `text` | User's display name |
| `PasswordHash` | `text` | Hashed password |
| `EmailConfirmed` | `boolean` | Email verification status |

### Future Tables

- `Projects` - Portfolio projects
- `Blogs` - Blog posts with MDX content
- `Snippets` - Animation playground snippets
- `Subscriptions` - User subscription tiers

---

## 🔒 Security Considerations

### Password Requirements

ASP.NET Identity enforces:
- ✅ Minimum 6 characters
- ✅ At least one uppercase letter
- ✅ At least one lowercase letter
- ✅ At least one digit
- ✅ At least one special character

### JWT Security

- Tokens expire after 60 minutes (configurable)
- Signed with HMAC-SHA256
- Contains: `sub` (user ID), `email`, `jti` (unique ID), `exp` (expiry)

### CORS Configuration

Development CORS allows:
- Origin: `http://localhost:3000`
- Methods: All
- Headers: All

---

## 🧪 Testing

### API Testing with curl

```bash
# Register
curl -X POST http://localhost:5266/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email": "test@example.com", "password": "Test123!", "displayName": "Test"}'

# Login
curl -X POST http://localhost:5266/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "test@example.com", "password": "Test123!"}'
```

### Swagger UI

Visit `http://localhost:5266/swagger` for interactive API testing.

---

## 📞 Support

- 📧 Email: your@email.com
- 🐛 Issues: [GitHub Issues](../../issues)
- 💬 Discussions: [GitHub Discussions](../../discussions)

---

<div align="center">

**[← Back to Main README](../README.md)**

</div>

