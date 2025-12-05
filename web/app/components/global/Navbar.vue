<template>
  <header class="flex w-full h-16 fixed top-0 left-0 right-0 z-40 glass ">
    <div class="flex px-4 pt-5 w-full h-full items-center justify-between">
      <NuxtLink to="/">
        Judah Sullivan
      </NuxtLink>
      <nav class="hidden md:flex items-center gap-6 min-w-0">
        <NuxtLink to="/about">About</NuxtLink>
        <NuxtLink to="/projects">Projects</NuxtLink>
        <NuxtLink to="/blog">Blog</NuxtLink>
        <NuxtLink to="/playground">PlayGround</NuxtLink>
        <NuxtLink to="/contact">Contact</NuxtLink>
      </nav>

      <div class="space-x-5">
        <ThemeToggle />
        <!-- <UiButton to="/auth" class="shrink-0">Login</UiButton> -->
      </div>
    </div>


    <MobileMenu :is-open="isMobileMenuOpen" @close="isMobileMenuOpen = false" />
  </header>
</template>

<script setup lang="ts">
const route = useRoute()
const isMobileMenuOpen = ref(false)

const navLinks = [
  { to: '/', label: 'Home' },
  { to: '/about', label: 'About' },
  { to: '/projects', label: 'Projects' },
  { to: '/blog', label: 'Blog' },
]

// Check if link is active - ensure consistent SSR/client rendering
// Use computed to ensure reactivity and consistent rendering during hydration
const currentPath = computed(() => route.path)
const isActive = (path: string) => {
  const pathValue = currentPath.value
  if (path === '/') return pathValue === '/'
  return pathValue.startsWith(path)
}

// Close mobile menu on route change
watch(() => route.path, () => {
  isMobileMenuOpen.value = false
})
</script>
