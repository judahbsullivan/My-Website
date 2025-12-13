<template>
  <header 
    ref="headerRef"
    class="flex w-full h-16 fixed top-0 left-0 right-0 z-40 glass opacity-0"
  >
    <div class="flex px-4 pt-5 w-full h-full items-center justify-between">
      <NuxtLink 
        ref="logoRef"
        to="/" 
        class="font-semibold text-lg opacity-0 -translate-y-4"
      >
        Judah Sullivan
      </NuxtLink>
      <nav ref="navRef" class="hidden md:flex items-center gap-6 min-w-0">
        <NuxtLink 
          v-for="(link, index) in navLinks" 
          :key="link.to"
          :to="link.to"
          :ref="el => { if (el) navLinkRefs[index] = el as any }"
          class="opacity-0 -translate-y-4 hover:text-primary transition-colors"
        >
          {{ link.label }}
        </NuxtLink>
      </nav>

      <div ref="actionsRef" class="space-x-5 opacity-0 -translate-y-4">
        <ThemeToggle />
        <!-- <UiButton to="/auth" class="shrink-0">Login</UiButton> -->
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { useNuxtApp } from '#app'

// Refs for animation
const headerRef = ref<HTMLElement | null>(null)
const logoRef = ref<any>(null)
const navRef = ref<HTMLElement | null>(null)
const navLinkRefs = ref<any[]>([])
const actionsRef = ref<HTMLElement | null>(null)

const { isIntroLoaderComplete } = useIntroLoader()

const navLinks = [
  { to: '/about', label: 'About' },
  { to: '/projects', label: 'Projects' },
  { to: '/blog', label: 'Blog' },
  { to: '/playground', label: 'PlayGround' },
  { to: '/contact', label: 'Contact' },
]

const route = useRoute()

// Check if link is active - ensure consistent SSR/client rendering
// Use computed to ensure reactivity and consistent rendering during hydration
const currentPath = computed(() => route.path)
const isActive = (path: string) => {
  const pathValue = currentPath.value
  if (path === '/') return pathValue === '/'
  return pathValue.startsWith(path)
}

// Animate navbar entrance after intro loader completes
function animateNavbarEntrance() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap || !headerRef.value) return
  
  const header = headerRef.value
  const logo = logoRef.value?.$el || logoRef.value
  const actions = actionsRef.value
  const links = navLinkRefs.value.map(ref => ref?.$el || ref).filter(Boolean)
  
  // Create timeline for navbar entrance
  const tl = gsap.timeline()
  
  // 1. Fade in header background
  tl.to(header, {
    opacity: 1,
    duration: 0.4,
    ease: 'power2.out'
  })
  
  // 2. Animate logo with bounce
  if (logo) {
    tl.to(logo, {
      opacity: 1,
      y: 0,
      duration: 0.5,
      ease: 'back.out(1.4)'
    }, '-=0.2')
  }
  
  // 3. Stagger nav links
  if (links.length > 0) {
    tl.to(links, {
      opacity: 1,
      y: 0,
      duration: 0.4,
      stagger: {
        amount: 0.3,
        from: 'start'
      },
      ease: 'back.out(1.2)'
    }, '-=0.3')
  }
  
  // 4. Animate actions (theme toggle, etc.)
  if (actions) {
    tl.to(actions, {
      opacity: 1,
      y: 0,
      duration: 0.4,
      ease: 'back.out(1.2)'
    }, '-=0.2')
  }
}

// Watch for intro loader completion
watch(isIntroLoaderComplete, (complete) => {
  if (complete) {
    nextTick(() => {
      // Small delay to ensure smooth transition after loader fades
      setTimeout(() => {
        animateNavbarEntrance()
      }, 100)
    })
  }
  }, { immediate: true })

</script>
