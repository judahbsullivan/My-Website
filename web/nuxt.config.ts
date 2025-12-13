import tailwindcss from "@tailwindcss/vite";

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },

  colorMode: {
    classSuffix: '',
    preference: 'system',
    fallback: 'dark',
  },

  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5266',
      deployBranch: process.env.NUXT_PUBLIC_DEPLOY_BRANCH || process.env.VERCEL_GIT_COMMIT_REF || process.env.GIT_BRANCH || 'main',
    },
  },
  css: [
    "./styles/globals.css",
    "@fontsource/aileron/400.css",
    "@fontsource/aileron/600.css",
    "@fontsource/aileron/700.css"
  ],
  vite: {
    plugins: [
      tailwindcss(),
    ],
  },
  gsap: {
    clubPlugins: {
      splitText: true,
      scrollSmoother: true,
    },
    extraPlugins: {
      scrollTrigger: true,
      
    }
  },
  typescript: {
    // only add custom compilerOptions if needed
    tsConfig: {
      compilerOptions: {
        strict: true,
        // but avoid redefining `paths` unless you re-merge correctly
      }
    }
  },
  modules: [
    '@nuxt/content',
    '@nuxt/eslint',
    '@nuxt/hints',
    '@nuxt/image',
    '@nuxt/scripts',
    '@nuxt/test-utils',
    '@nuxt/ui',
    '@hypernym/nuxt-gsap'
  ],
  image: {
    quality: 80,
    format: ['webp'],
    // Allow images from API domain and any external URLs used
    domains: [
      'localhost',
      // Add production API domain here if needed
    ],
  },
  app: {
    pageTransition: {
      name: 'page',
      mode: 'out-in',
    },
  }
})