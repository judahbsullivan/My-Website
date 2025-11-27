import tailwindcss from "@tailwindcss/vite";

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
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
