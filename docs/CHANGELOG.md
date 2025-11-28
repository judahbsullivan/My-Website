# 📝 Changelog

<div align="center">

![Changelog](https://img.shields.io/badge/Changelog-Semantic_Versioning-blue?style=for-the-badge)

All notable changes to this project will be documented in this file.

</div>

---

## [Unreleased]

### 🚧 In Progress
- Projects listing and detail pages
- Blog system with Nuxt Content
- User dashboard

---

## [0.1.0] - 2024-11-28

### 🎉 Initial Release - Foundation Complete

This release establishes the core foundation of the MyPortfolio project with full-stack authentication and basic project structure.

---

### ✨ Added

#### Backend (.NET API)

- **Project Setup**
  - Initialized .NET 10 Web API project
  - Configured project structure with Controllers, Models, Services, Data folders
  - Set up `Program.cs` with middleware pipeline

- **Database**
  - Integrated Entity Framework Core with PostgreSQL
  - Configured Supabase connection with SSL
  - Created `AppDbContext` with ASP.NET Identity
  - Applied initial migration creating Identity tables:
    - `AspNetUsers`
    - `AspNetRoles`
    - `AspNetUserClaims`
    - `AspNetRoleClaims`
    - `AspNetUserLogins`
    - `AspNetUserRoles`
    - `AspNetUserTokens`

- **Authentication**
  - Implemented JWT-based authentication
  - Created `TokenService` for JWT generation
  - Built `AuthController` with endpoints:
    - `POST /api/auth/register` - User registration
    - `POST /api/auth/login` - User login
  - Extended `IdentityUser` with custom `AppUser` model
  - Added `DisplayName` field to user profile

- **Configuration**
  - Environment-based configuration via `.env` file
  - DotNetEnv for loading environment variables
  - CORS configuration for frontend development
  - Swagger/OpenAPI for API documentation

#### Frontend (Nuxt 4)

- **Project Setup**
  - Initialized Nuxt 4 project with Vue 3
  - Configured TypeScript with strict mode
  - Set up ESLint for code quality

- **Styling**
  - Integrated Tailwind CSS 4 with Vite plugin
  - Added Nuxt UI component library
  - Created global styles structure

- **Animation**
  - Integrated GSAP animation library
  - Configured GSAP Club plugins:
    - SplitText for text animations
    - ScrollSmoother for smooth scrolling
  - Created animated landing page with:
    - Overlay reveal animation
    - Character-by-character text animation
    - Line-by-line paragraph reveal

- **Authentication**
  - Created `useAuth` composable with:
    - Reactive `user` and `token` state
    - `isAuthenticated` computed property
    - `register()` function
    - `login()` function
    - `logout()` function
    - `authFetch()` helper for authenticated requests
    - `getAuthHeader()` utility
  - Built auth client plugin for initialization
  - LocalStorage persistence for auth state

- **Pages**
  - Created `pages/index.vue` with animated hero
  - Built `pages/auth.vue` with:
    - Login/Register toggle
    - Form validation
    - Error message display
    - Success notifications
    - Logged-in user display
    - Logout functionality

- **Configuration**
  - Runtime config for API base URL
  - Module integrations:
    - `@nuxt/content`
    - `@nuxt/image`
    - `@nuxt/ui`
    - `@nuxt/hints`
    - `@nuxt/scripts`
    - `@hypernym/nuxt-gsap`

#### Shared

- **Types**
  - Created `shared/types.ts` with TypeScript interfaces:
    - `User`
    - `AuthResponse`
    - `LoginRequest`
    - `RegisterRequest`
    - `AuthState`

#### Documentation

- Created comprehensive `README.md` with:
  - Project overview
  - Architecture diagram
  - Quick start guide
  - Tech stack details
  - API endpoints reference
- Created `docs/` folder with:
  - `docs/README.md` - Documentation home
  - `docs/ROADMAP.md` - Project roadmap
  - `docs/CHANGELOG.md` - This file

---

### 🔧 Technical Details

#### Dependencies Added

**Backend (api.csproj)**
```xml
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Npgsql.EntityFrameworkCore.PostgreSQL
- Microsoft.AspNetCore.Authentication.JwtBearer
- System.IdentityModel.Tokens.Jwt
- DotNetEnv
- Swashbuckle.AspNetCore
```

**Frontend (package.json)**
```json
- nuxt: ^4.2.1
- vue: ^3.5.25
- @nuxt/ui: 4.2.1
- @nuxt/content: 3.8.2
- @nuxt/image: 2.0.0
- tailwindcss: ^4.1.17
- gsap: ^3.13.0
- @hypernym/nuxt-gsap: ^2.4.3
```

#### API Response Formats

**Successful Authentication**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": "uuid",
    "email": "user@example.com",
    "displayName": "User Name"
  }
}
```

**Error Response**
```json
{
  "message": "Human-readable error message",
  "errors": ["Array", "of", "specific", "errors"]
}
```

---

### 📊 Stats

| Metric | Count |
|--------|-------|
| Files Created | 15+ |
| API Endpoints | 2 |
| Database Tables | 7 |
| Vue Components | 3 |
| Composables | 1 |
| Plugins | 1 |

---

## Version History

| Version | Date | Highlights |
|---------|------|------------|
| 0.1.0 | 2024-11-28 | 🎉 Initial foundation release |

---

<div align="center">

**[← Back to Docs](./README.md)** · **[Main README](../README.md)**

</div>

