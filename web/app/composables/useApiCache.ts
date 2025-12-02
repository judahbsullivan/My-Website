/**
 * Composable for managing API cache keys consistently across the application
 * Following Nuxt best practices for data caching and de-duplication
 */
export const useApiCache = () => {
  return {
    // Projects cache keys
    projects: {
      all: 'projects:all',
      featured: 'projects:featured',
      detail: (slug: string) => `project:${slug}`,
    },
    // Blog cache keys
    blog: {
      all: 'blog:all',
      latest: 'blog:latest',
      detail: (slug: string) => `blog:${slug}`,
    },
  }
}

