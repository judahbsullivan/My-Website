<div align="center">

<!-- Animated Header -->
<img src="https://capsule-render.vercel.app/api?type=waving&color=gradient&customColorList=6,11,20&height=180&section=header&text=MyPortfolio&fontSize=42&fontColor=fff&animation=twinkling&fontAlignY=32&desc=Interactive%20Developer%20Portfolio%20%26%20Playground&descAlignY=52&descSize=18" width="100%"/>

<!-- Typing Animation -->
<a href="https://git.io/typing-svg"><img src="https://readme-typing-svg.demolab.com?font=Fira+Code&weight=600&size=22&pause=1000&color=6366F1&center=true&vCenter=true&multiline=true&repeat=false&width=600&height=100&lines=Full-Stack+%E2%80%A2+Interactive+%E2%80%A2+Modern;Built+with+Nuxt+4+%2B+.NET+8+%2B+Supabase" alt="Typing SVG" /></a>

<!-- Badges -->
<p>
  <img src="https://img.shields.io/badge/Nuxt-4.0-00DC82?style=for-the-badge&logo=nuxtdotjs&logoColor=white" />
  <img src="https://img.shields.io/badge/Vue-3.5-4FC08D?style=for-the-badge&logo=vuedotjs&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Supabase-PostgreSQL-3FCF8E?style=for-the-badge&logo=supabase&logoColor=white" />
</p>

<p>
  <img src="https://img.shields.io/badge/GSAP-Animations-88CE02?style=flat-square&logo=greensock&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-Auth-000000?style=flat-square&logo=jsonwebtokens&logoColor=white" />
  <img src="https://img.shields.io/badge/TailwindCSS-4.0-06B6D4?style=flat-square&logo=tailwindcss&logoColor=white" />
  <img src="https://img.shields.io/badge/TypeScript-5.6-3178C6?style=flat-square&logo=typescript&logoColor=white" />
</p>

---

**A full-stack, interactive developer portfolio that's more than just a static site.**  
**It's a playground where users can explore, interact, and experience.**

[Live Demo](#) · [Documentation](./docs/README.md) · [Roadmap](./docs/ROADMAP.md) · [Report Bug](../../issues)

</div>

---

## ✨ What Makes This Different?

<table>
<tr>
<td width="50%">

### 🎭 Not Just a Portfolio
This isn't your typical "here's my work" website. It's an **interactive playground** where visitors can:
- Test animation snippets in real-time
- Explore projects with immersive transitions
- Experience smooth, scroll-triggered animations
- Copy and modify code directly

</td>
<td width="50%">

### 🔐 Full-Stack Power
Behind the scenes, a robust backend handles:
- JWT-based authentication
- Content management via API
- User sessions & subscriptions
- PostgreSQL database with Supabase

</td>
</tr>
</table>

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         FRONTEND                                 │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                    Nuxt 4 / Vue 3                        │    │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐   │    │
│  │  │  Pages   │ │Components│ │Composables│ │ Plugins  │   │    │
│  │  └──────────┘ └──────────┘ └──────────┘ └──────────┘   │    │
│  │         ↓           ↓            ↓            ↓         │    │
│  │  ┌─────────────────────────────────────────────────┐   │    │
│  │  │     GSAP • ScrollTrigger • SplitText • Pinia    │   │    │
│  │  └─────────────────────────────────────────────────┘   │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              │                                   │
│                         HTTP/REST                                │
│                              ↓                                   │
├─────────────────────────────────────────────────────────────────┤
│                          BACKEND                                 │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                  .NET 10 Web API                         │    │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐   │    │
│  │  │Controllers│ │ Services │ │  Models  │ │   Auth   │   │    │
│  │  └──────────┘ └──────────┘ └──────────┘ └──────────┘   │    │
│  │         ↓           ↓            ↓            ↓         │    │
│  │  ┌─────────────────────────────────────────────────┐   │    │
│  │  │   Entity Framework Core • JWT • ASP.NET Identity │   │    │
│  │  └─────────────────────────────────────────────────┘   │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              │                                   │
│                         TCP/SSL                                  │
│                              ↓                                   │
├─────────────────────────────────────────────────────────────────┤
│                         DATABASE                                 │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │              Supabase (PostgreSQL)                       │    │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐   │    │
│  │  │  Users   │ │ Projects │ │  Blogs   │ │ Snippets │   │    │
│  │  └──────────┘ └──────────┘ └──────────┘ └──────────┘   │    │
│  └─────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🚀 Quick Start

### Prerequisites

- [Node.js](https://nodejs.org/) v20+ or [Bun](https://bun.sh/)
- [.NET SDK](https://dotnet.microsoft.com/download) 10.0+
- [Supabase](https://supabase.com/) account (or local PostgreSQL)

### 1️⃣ Clone & Setup

```bash
git clone https://github.com/yourusername/myPortfolio.git
cd myPortfolio
```

### 2️⃣ Configure Environment

Create a `.env` file in the root:

```env
# Database (Supabase)
DB_HOST=db.your-project.supabase.co
DB_PORT=5432
DB_NAME=postgres
DB_USER=postgres
DB_PASSWORD=your_password

# JWT Configuration
JWT_KEY=your_super_secret_key_at_least_32_characters
JWT_ISSUER=MyPortfolio
JWT_AUDIENCE=MyPortfolioUsers
JWT_EXPIRE_MINUTES=60
```

### 3️⃣ Start the Backend

```bash
cd api
dotnet ef database update  # Apply migrations
dotnet run                 # Start API on http://localhost:5266
```

### 4️⃣ Start the Frontend

```bash
cd web
bun install    # or npm install
bun dev        # Start on http://localhost:3000
```

---

## 📁 Project Structure

```
myPortfolio/
├── 📂 api/                      # .NET Backend
│   ├── 📂 Controllers/          # API endpoints
│   │   └── AuthController.cs    # Authentication routes
│   ├── 📂 Data/                 # Database context
│   │   └── AppDbContext.cs      # EF Core setup
│   ├── 📂 Models/               # Data models
│   │   ├── AppUser.cs           # User entity
│   │   └── JwtSettings.cs       # JWT configuration
│   ├── 📂 Services/             # Business logic
│   │   └── TokenService.cs      # JWT generation
│   ├── 📂 Migrations/           # Database migrations
│   └── Program.cs               # App configuration
│
├── 📂 web/                      # Nuxt Frontend
│   ├── 📂 app/
│   │   ├── 📂 pages/            # Route pages
│   │   │   ├── index.vue        # Home page
│   │   │   └── auth.vue         # Login/Register
│   │   ├── 📂 composables/      # Vue composables
│   │   │   └── useAuth.ts       # Auth state management
│   │   └── app.vue              # Root component
│   ├── 📂 plugins/              # Nuxt plugins
│   │   └── auth.client.ts       # Auth initialization
│   ├── 📂 styles/               # Global styles
│   └── nuxt.config.ts           # Nuxt configuration
│
├── 📂 shared/                   # Shared code
│   └── types.ts                 # TypeScript types
│
├── 📂 docs/                     # Documentation
│   ├── README.md                # Docs home
│   ├── ROADMAP.md               # Project roadmap
│   └── CHANGELOG.md             # Version history
│
└── .env                         # Environment variables
```

---

## 🔐 Authentication Flow

```mermaid
sequenceDiagram
    participant U as User
    participant N as Nuxt Frontend
    participant A as .NET API
    participant D as Supabase DB

    U->>N: Enter credentials
    N->>A: POST /api/auth/register
    A->>D: Create user (Identity)
    D-->>A: User created
    A->>A: Generate JWT
    A-->>N: { token, user }
    N->>N: Store in localStorage
    N-->>U: Redirect to dashboard
    
    Note over U,D: Subsequent requests include JWT
    
    U->>N: Access protected route
    N->>A: GET /api/protected (+ Bearer token)
    A->>A: Validate JWT
    A-->>N: Protected data
    N-->>U: Display content
```

---

## 🎨 Animation Playground

<table>
<tr>
<td align="center">
<img src="https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExcGt6Z2Z6ZnR5Y2k2aGx6ZWJyMnJsY2JrN2FtdmZqZXN6cGNuYmNxdiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/3oKIPnAiaMCws8nOsE/giphy.gif" width="200"/>
<br/>
<b>Split Text</b>
</td>
<td align="center">
<img src="https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExcm9oZHN5N3J6bWR0Y2E5YWp0N2RrYnVtaGU4Z2h1dGNrZHBhZW9haCZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/xT9IgzoKnwFNmISR8I/giphy.gif" width="200"/>
<br/>
<b>Scroll Triggers</b>
</td>
<td align="center">
<img src="https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExaXl0ZXhtbmRrNTR0NHd0N2ZvbGJtYnJwcGx4aW1qcXZ2eXNlZGtuZCZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/3o7TKTDn976rzVgky4/giphy.gif" width="200"/>
<br/>
<b>Interactive UI</b>
</td>
</tr>
</table>

The playground lets users:
- 🧪 **Test** animation snippets in real-time
- 📋 **Copy** code with one click
- 🎛️ **Modify** parameters and see instant results
- 💾 **Save** favorite snippets (authenticated users)

---

## 🛠️ Tech Stack

<table>
<tr>
<th>Layer</th>
<th>Technology</th>
<th>Purpose</th>
</tr>
<tr>
<td><b>Frontend</b></td>
<td>
<img src="https://img.shields.io/badge/-Nuxt_4-00DC82?style=flat-square&logo=nuxtdotjs&logoColor=white"/>
<img src="https://img.shields.io/badge/-Vue_3-4FC08D?style=flat-square&logo=vuedotjs&logoColor=white"/>
<img src="https://img.shields.io/badge/-TypeScript-3178C6?style=flat-square&logo=typescript&logoColor=white"/>
</td>
<td>SSR, routing, reactivity</td>
</tr>
<tr>
<td><b>Styling</b></td>
<td>
<img src="https://img.shields.io/badge/-Tailwind_4-06B6D4?style=flat-square&logo=tailwindcss&logoColor=white"/>
<img src="https://img.shields.io/badge/-Nuxt_UI-00DC82?style=flat-square&logo=nuxtdotjs&logoColor=white"/>
</td>
<td>Utility-first CSS, components</td>
</tr>
<tr>
<td><b>Animation</b></td>
<td>
<img src="https://img.shields.io/badge/-GSAP-88CE02?style=flat-square&logo=greensock&logoColor=white"/>
<img src="https://img.shields.io/badge/-ScrollTrigger-88CE02?style=flat-square"/>
<img src="https://img.shields.io/badge/-SplitText-88CE02?style=flat-square"/>
</td>
<td>Advanced animations</td>
</tr>
<tr>
<td><b>Backend</b></td>
<td>
<img src="https://img.shields.io/badge/-.NET_10-512BD4?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/-ASP.NET_Core-512BD4?style=flat-square"/>
<img src="https://img.shields.io/badge/-EF_Core-512BD4?style=flat-square"/>
</td>
<td>REST API, ORM, Identity</td>
</tr>
<tr>
<td><b>Auth</b></td>
<td>
<img src="https://img.shields.io/badge/-JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white"/>
<img src="https://img.shields.io/badge/-ASP.NET_Identity-512BD4?style=flat-square"/>
</td>
<td>Secure authentication</td>
</tr>
<tr>
<td><b>Database</b></td>
<td>
<img src="https://img.shields.io/badge/-Supabase-3FCF8E?style=flat-square&logo=supabase&logoColor=white"/>
<img src="https://img.shields.io/badge/-PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white"/>
</td>
<td>Cloud-hosted database</td>
</tr>
</table>

---

## 📚 API Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/auth/register` | Create new account | ❌ |
| `POST` | `/api/auth/login` | Login & get JWT | ❌ |
| `GET` | `/api/projects` | List all projects | ❌ |
| `GET` | `/api/projects/:slug` | Get project by slug | ❌ |
| `GET` | `/api/blogs` | List all blog posts | ❌ |
| `GET` | `/api/playground` | List animation snippets | ❌ |
| `POST` | `/api/playground` | Create snippet | ✅ |

---

## 🤝 Contributing

Contributions are welcome! Please read the [contributing guidelines](./docs/CONTRIBUTING.md) first.

```bash
# Fork the repo
# Create your feature branch
git checkout -b feature/amazing-feature

# Commit your changes
git commit -m 'Add some amazing feature'

# Push to the branch
git push origin feature/amazing-feature

# Open a Pull Request
```

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=gradient&customColorList=6,11,20&height=100&section=footer" width="100%"/>

**Built with ❤️ by [Judah Sullivan](https://github.com/judahsullivan)**

<p>
<a href="https://github.com/judahsullivan"><img src="https://img.shields.io/badge/-GitHub-181717?style=for-the-badge&logo=github&logoColor=white"/></a>
<a href="https://linkedin.com/in/judahsullivan"><img src="https://img.shields.io/badge/-LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white"/></a>
<a href="https://twitter.com/judahsullivan"><img src="https://img.shields.io/badge/-Twitter-1DA1F2?style=for-the-badge&logo=twitter&logoColor=white"/></a>
</p>

</div>
