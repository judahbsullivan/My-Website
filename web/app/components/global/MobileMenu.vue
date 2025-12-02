<template>
  <Teleport to="body">
    <!-- Backdrop -->
    <Transition
      enter-active-class="transition-opacity duration-300"
      enter-from-class="opacity-0"
      enter-to-class="opacity-100"
      leave-active-class="transition-opacity duration-300"
      leave-from-class="opacity-100"
      leave-to-class="opacity-0"
    >
    <div
        v-if="isVisible"
      class="fixed inset-0 z-40 bg-black/60 backdrop-blur-sm md:hidden"
      @click="close"
    />
    </Transition>

    <!-- Menu Panel -->
    <div
      v-if="isVisible"
      ref="navPanelRef"
      class="fixed min-h-dvh right-0 top-0 w-full md:w-[35%] z-50 bg-background text-foreground px-6 py-10 border-l border-border flex flex-col justify-between"
    >
      <!-- Close Button -->
      <button
        @click="close"
        class="absolute top-6 right-6 w-10 h-10 rounded-lg bg-muted flex items-center justify-center hover:bg-accent transition-colors z-10"
        type="button"
        aria-label="Close menu"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>

      <div class="flex-1 flex flex-col justify-center">
        <!-- Navigation Heading -->
        <h2 class="menu-heading text-sm uppercase tracking-wider text-muted-foreground mb-8">
          Navigation
        </h2>

        <!-- Navigation Links -->
        <ul class="space-y-8 items-left flex flex-col text-left">
          <li v-for="(link, index) in navLinks" :key="link.to">
            <NuxtLink
              :to="link.to"
              class="nav-link group block uppercase text-foreground text-[clamp(2.75rem,5vw,5rem)] leading-none tracking-tight cursor-pointer"
              :class="{ 'underline': isActive(link.to) }"
              @click="close"
            >
              {{ link.label }}
            </NuxtLink>
          </li>
        </ul>
      </div>

      <!-- Social Links at Bottom -->
      <div class="pb-8">
      <h3 class="menu-heading text-sm uppercase tracking-wider text-muted-foreground mb-6">
        Connect
      </h3>
      <div class="flex items-center gap-6 md:gap-10 text-xl md:text-2xl flex-wrap">
        <a 
          href="https://linkedin.com/in/judahsullivan" 
          target="_blank" 
          rel="noopener noreferrer"
          class="social-link capitalize cursor-pointer"
        >
          Linkedin
        </a>
        <a 
          href="https://profile.indeed.com/?hl=en_US&co=US&from=gnav-homepage" 
          target="_blank" 
          rel="noopener noreferrer"
          class="social-link capitalize cursor-pointer"
        >
          Indeed
        </a>
        <a 
          href="https://youtube.com/judahsullivan" 
          target="_blank" 
          rel="noopener noreferrer"
          class="social-link capitalize cursor-pointer"
        >
          YouTube
        </a>
        <a 
          href="https://github.com/judahbsullivan" 
          target="_blank" 
          rel="noopener noreferrer"
          class="social-link capitalize cursor-pointer"
        >
          Github
        </a>
      </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
interface Props {
  isOpen: boolean
}

const props = defineProps<Props>()
const emit = defineEmits<{
  close: []
}>()

const route = useRoute()
const nuxtApp = useNuxtApp()
const $gsap = nuxtApp.$gsap as typeof gsap
const $SplitText = nuxtApp.$SplitText as any

const navPanelRef = ref<HTMLElement | null>(null)
const timelineRef = ref<gsap.core.Timeline | null>(null)
const splitTextInstances = ref<any[]>([])
const isVisible = ref(false)

const navLinks = [
  { to: '/', label: 'Home' },
  { to: '/about', label: 'About' },
  { to: '/projects', label: 'Projects' },
  { to: '/blog', label: 'Blog' },
  { to: '/contact', label: 'Contact' },
]

const isActive = (path: string) => {
  if (path === '/') return route.path === '/'
  return route.path.startsWith(path)
}

const close = () => {
  emit('close')
}

// Animate text when menu opens/closes
watch(() => props.isOpen, async (newVal) => {
  if (!$gsap) return

  if (newVal) {
    // Show menu
    isVisible.value = true
    
    // Wait for DOM to be ready
    await nextTick()
    
    const navPanel = navPanelRef.value
    if (!navPanel) return

    // Kill existing timeline if it exists
    if (timelineRef.value) {
      timelineRef.value.kill()
    }

    // Clean up old SplitText instances
    splitTextInstances.value.forEach(instance => {
      if (instance && instance.revert) {
        instance.revert()
      }
    })
    splitTextInstances.value = []

    // Set initial state - panel off-screen to the right
    $gsap.set(navPanel, { x: '100%' })

    // Create timeline with reverse complete callback
    const tl = $gsap.timeline({ 
      paused: true,
      onReverseComplete: () => {
        // Clean up SplitText instances after animation
        splitTextInstances.value.forEach(instance => {
          if (instance && instance.revert) {
            instance.revert()
          }
        })
        splitTextInstances.value = []
        
        // Hide menu after animation completes
        isVisible.value = false
      }
    })
    tl.to(navPanel, {
      x: 0,
      duration: 0.8,
      ease: 'power4.inOut',
    })

    timelineRef.value = tl
    
    // Play timeline
    tl.play()
    
    document.body.style.overflow = 'hidden'

    // Wait for panel to be visible before animating text
    if ($SplitText) {
      setTimeout(() => {
        if (!navPanelRef.value) return

        const headings = navPanelRef.value.querySelectorAll('.menu-heading')
        const navLinks = navPanelRef.value.querySelectorAll('.nav-link')
        const socialLinks = navPanelRef.value.querySelectorAll('.social-link')

      // Animate headings
      headings.forEach((heading) => {
          const split = new $SplitText(heading as Element, {
          type: 'chars, words, lines',
          linesClass: 'line',
          wordsClass: 'word',
          charsClass: 'char',
          mask: 'lines',
          smartWrap: true,
        })

          splitTextInstances.value.push(split)

          $gsap.set(split.chars, { yPercent: 100, opacity: 0 })
          $gsap.to(split.chars, {
          yPercent: 0,
          opacity: 1,
          duration: 1.2,
          stagger: 0.04,
          ease: 'power4.out',
        })
      })

      // Animate nav links
      navLinks.forEach((link) => {
          const split = new $SplitText(link as Element, {
          type: 'chars, words, lines',
          linesClass: 'line',
          wordsClass: 'word',
          charsClass: 'char',
          mask: 'lines',
          smartWrap: true,
        })

          splitTextInstances.value.push(split)

          $gsap.set(split.chars, { yPercent: 100, opacity: 0 })
          $gsap.to(split.chars, {
          yPercent: 0,
          opacity: 1,
          duration: 1.2,
          stagger: 0.04,
          ease: 'power4.out',
        })
      })

      // Animate social links
      socialLinks.forEach((link) => {
          const split = new $SplitText(link as Element, {
          type: 'chars, words, lines',
          linesClass: 'line',
          wordsClass: 'word',
          charsClass: 'char',
          mask: 'lines',
          smartWrap: true,
        })

          splitTextInstances.value.push(split)

          $gsap.set(split.chars, { yPercent: 100, opacity: 0 })
          $gsap.to(split.chars, {
          yPercent: 0,
          opacity: 1,
          duration: 1.2,
          stagger: 0.04,
          ease: 'power4.out',
        })
      })
      }, 100)
    }
  } else {
    // Animate out text first
    if ($SplitText && splitTextInstances.value.length > 0) {
      splitTextInstances.value.forEach(split => {
        if (split && split.chars) {
          $gsap.to(split.chars, {
            yPercent: 100,
            opacity: 0,
            duration: 0.6,
            stagger: 0.02,
            ease: 'power4.in',
          })
        }
      })
    }

    // Then reverse panel timeline
    if (timelineRef.value) {
      timelineRef.value.reverse()
    } else {
      // If no timeline, hide immediately
      isVisible.value = false
    }
    
    document.body.style.overflow = 'auto'
  }
})

// Cleanup
onUnmounted(() => {
  if (timelineRef.value) {
    timelineRef.value.kill()
    timelineRef.value = null
  }
  
  // Clean up SplitText instances
  splitTextInstances.value.forEach(instance => {
    if (instance && instance.revert) {
      instance.revert()
    }
  })
  splitTextInstances.value = []
  
  document.body.style.overflow = 'auto'
})
</script>

<style>
/* Ensure chars are inline-block for animation */
.menu-heading .char,
.nav-link .char,
.social-link .char {
  display: inline-block;
  will-change: transform, opacity;
}

/* Line overflow hidden for mask effect */
.menu-heading .line,
.nav-link .line,
.social-link .line {
  overflow: hidden;
}
</style>
