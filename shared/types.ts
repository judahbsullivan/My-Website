// ═══════════════════════════════════════════════════════════════
// AUTH TYPES
// ═══════════════════════════════════════════════════════════════

export interface User {
  id: string
  email: string
  displayName: string
}

export interface AuthResponse {
  token: string
  user: User
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  displayName: string
}

export interface AuthState {
  user: User | null
  token: string | null
  isAuthenticated: boolean
}

// ═══════════════════════════════════════════════════════════════
// PROJECT TYPES
// ═══════════════════════════════════════════════════════════════

export interface Project {
  id: number
  slug: string
  title: string
  category: string
  year: string
  description: string
  imageUrl?: string
  isFeatured: boolean
}

export interface ProjectDetail extends Project {
  role: string
  overview?: string
  liveUrl?: string
  githubUrl?: string
  techStack: string[]
  features: ProjectFeature[]
  galleryImages: string[]
}

export interface ProjectFeature {
  title: string
  description: string
}

// ═══════════════════════════════════════════════════════════════
// BLOG TYPES
// ═══════════════════════════════════════════════════════════════

export interface BlogPost {
  id: number
  slug: string
  title: string
  category: string
  excerpt: string
  imageUrl?: string
  readTimeMinutes: number
  isFeatured: boolean
  publishedAt?: string
}

export interface BlogPostDetail extends BlogPost {
  content: string
  tags: string[]
  author?: Author
}

export interface Author {
  id: string
  displayName: string
  email: string
}

// ═══════════════════════════════════════════════════════════════
// CONTACT TYPES
// ═══════════════════════════════════════════════════════════════

export interface ContactRequest {
  name: string
  email: string
  subject?: string
  message: string
}
