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

export interface CreateProjectRequest {
  slug: string
  title: string
  category: string
  year: string
  role: string
  description: string
  overview?: string
  imageUrl?: string
  liveUrl?: string
  githubUrl?: string
  techStack?: string[]
  features?: ProjectFeature[]
  galleryImages?: string[]
  isFeatured?: boolean
  displayOrder?: number
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
  image?: string
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

export interface CreateBlogPostRequest {
  slug: string
  title: string
  category: string
  excerpt: string
  content: string
  image?: string
  tags?: string[]
  readTimeMinutes?: number
  isFeatured?: boolean
  isPublished?: boolean
  authorId?: string
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

// ═══════════════════════════════════════════════════════════════
// PLAYGROUND TYPES
// ═══════════════════════════════════════════════════════════════

export interface Playground {
  id: number
  slug: string
  title: string
  description: string
  category: string
  codeType: string
  previewImageUrl?: string
  isVueComponent: boolean
  isInteractive: boolean
  viewCount: number
  likeCount: number
  isFeatured: boolean
  requiresSubscription: boolean
  publishedAt?: string
}

export interface PlaygroundDetail extends Playground {
  code: string
  css?: string
  javascript?: string
  dependencies: string[]
  tags: string[]
  isLocked: boolean // Indicates if content is locked due to subscription
  author?: Author
}

export interface CreatePlaygroundRequest {
  slug: string
  title: string
  description: string
  category: string
  code: string
  codeType?: string
  css?: string
  javascript?: string
  dependencies?: string[]
  tags?: string[]
  previewImageUrl?: string
  isVueComponent?: boolean
  isInteractive?: boolean
  isFeatured?: boolean
  isPublished?: boolean
  requiresSubscription?: boolean
  authorId?: string
}
