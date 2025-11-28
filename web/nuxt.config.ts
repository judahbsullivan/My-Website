import tailwindcss from "@tailwindcss/vite";

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },

  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5266',
    },
  },
  css: ["./styles/globals.css"],
  vite: {
    plugins: [
      tailwindcss(),
    ],
  },
  gsap: {
    clubPlugins: {
      splitText: true,
      scrollSmoother: true,
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
  ]
})
