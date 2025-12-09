<template>
  <footer 
    ref="footerRef"
    class="px-4 sm:px-6 w-full bg-muted border-t border-border/50 pt-12 opacity-0"
    style="opacity: 0; transform: translateY(24px); visibility: hidden;"
  >
    <section class="flex flex-col">
      <div class="">
        <div class="">
          <UiTitle as="h2" size="4xl" weight="bold" align="left" class="mb-6">
            Let's Connect
          </UiTitle>
          <p class="text-lg sm:text-xl text-muted-foreground max-w-2xl leading-relaxed">
            Have a project in mind? Let's work together to bring your ideas to life.
          </p>
        </div>

        <div class="grid lg:grid-cols-2 gap-6 ">
          <!-- Navigation -->
          <div class="bg-card/50 backdrop-blur-xl h-auto rounded-2xl p-6 flex-col flex gap-5 border border-border/50 shadow-sm">
            <UiTitle as="h3" size="md" weight="semibold" class="">Navigation</UiTitle>
            <div class="space-y-4">
              <div>
                <NuxtLink to="/" class="text-lg font-medium hover:text-primary transition-colors block">Home</NuxtLink>
              </div>
              <div>
                <NuxtLink to="/about" class="text-lg font-medium hover:text-primary transition-colors block">About
                </NuxtLink>
              </div>
              <div>
                <NuxtLink to="/projects" class="text-lg font-medium hover:text-primary transition-colors block">Projects
                </NuxtLink>
              </div>
              <div>
                <NuxtLink to="/blog" class="text-lg font-medium hover:text-primary transition-colors block">Blog
                </NuxtLink>
              </div>
              <div>
                <NuxtLink to="/contact" class="text-lg font-medium hover:text-primary transition-colors block">Contact
                </NuxtLink>
              </div>
            </div>
          </div>

          <!-- Connect -->
          <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm">
            <UiTitle as="h3" size="md" weight="semibold" class="mb-6">Connect</UiTitle>
            <div class="space-y-6">
              <div>
                <p class="text-sm text-muted-foreground mb-2">Email</p>
                <a href="mailto:hello@judahsullivan.com"
                  class="text-lg font-medium hover:text-primary transition-colors">
                  hello@judahsullivan.com
                </a>
              </div>

              <div>
                <p class="text-sm text-muted-foreground ">Social</p>
                <div class="flex gap-3">
                  <UiButton href="https://github.com/judahbsullivan" target="_blank" variant="primary" size="sm"
                    class="w-11 h-11 p-0">
                    <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                      <path
                        d="M12 0C5.37 0 0 5.37 0 12c0 5.31 3.435 9.795 8.205 11.385.6.105.825-.255.825-.57 0-.285-.015-1.23-.015-2.235-3.015.555-3.795-.735-4.035-1.41-.135-.345-.72-1.41-1.23-1.695-.42-.225-1.02-.78-.015-.795.945-.015 1.62.87 1.845 1.23 1.08 1.815 2.805 1.305 3.495.99.105-.78.42-1.305.765-1.605-2.67-.3-5.46-1.335-5.46-5.925 0-1.305.465-2.385 1.23-3.225-.12-.3-.54-1.53.12-3.18 0 0 1.005-.315 3.3 1.23.96-.27 1.98-.405 3-.405s2.04.135 3 .405c2.295-1.56 3.3-1.23 3.3-1.23.66 1.65.24 2.88.12 3.18.765.84 1.23 1.905 1.23 3.225 0 4.605-2.805 5.625-5.475 5.925.435.375.81 1.095.81 2.22 0 1.605-.015 2.895-.015 3.3 0 .315.225.69.825.57A12.02 12.02 0 0024 12c0-6.63-5.37-12-12-12z" />
                    </svg>
                  </UiButton>
                  <UiButton href="https://linkedin.com/in/judahsullivan" target="_blank" variant="primary" size="sm"
                    class="w-11 h-11 p-0">
                    <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                      <path
                        d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433c-1.144 0-2.063-.926-2.063-2.065 0-1.138.92-2.063 2.063-2.063 1.14 0 2.064.925 2.064 2.063 0 1.139-.925 2.065-2.064 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                    </svg>
                  </UiButton>
                  <UiButton href="https://twitter.com/judahsullivan" target="_blank" variant="primary" size="sm"
                    class="w-11 h-11 p-0">
                    <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                      <path
                        d="M18.244 2.25h3.308l-7.227 8.26 8.502 11.24H16.17l-5.214-6.817L4.99 21.75H1.68l7.73-8.835L1.254 2.25H8.08l4.713 6.231zm-1.161 17.52h1.833L7.084 4.126H5.117z" />
                    </svg>
                  </UiButton>
                </div>
              </div>

              <div class="pt-4 border-t border-border/50">
                <div class="flex items-center gap-3">
                  <span class="w-2.5 h-2.5 rounded-full bg-success animate-pulse" />
                  <span class="text-base font-medium">Open to new opportunities</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Bottom Bar -->
        <div class="mt-12 pt-8 border-t border-border/50 flex flex-col sm:flex-row justify-between items-center gap-4">
          <p class="text-xs text-muted-foreground">
            © {{ currentYear }} Judah Sullivan. All rights reserved.
          </p>
        </div>
      </div>
    </section>
  </footer>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'

const currentYear = new Date().getFullYear()
const footerRef = ref<HTMLElement | null>(null)
let ctx: any = null

// Animate footer when scrolled into view
onMounted(() => {
  nextTick(() => {
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    const ScrollTrigger = nuxtApp.$ScrollTrigger as any
    
    if (!gsap || !ScrollTrigger || !footerRef.value) {
      // Fallback: just show it
      if (footerRef.value) {
        footerRef.value.style.visibility = 'visible'
        footerRef.value.style.opacity = '1'
        footerRef.value.style.transform = 'translateY(0px)'
      }
      return
    }
    
    // Use GSAP context for proper cleanup
    ctx = gsap.context(() => {
      // Set initial state with GSAP (keeps it hidden but ready)
      gsap.set(footerRef.value, {
        opacity: 0,
        y: 30,
        visibility: 'hidden'
      })
      
      // GSAP v3 method: scrollTrigger as property in the animation
      gsap.to(footerRef.value, {
        opacity: 1,
        y: 0,
        duration: 0.8,
        ease: 'power3.out',
        onStart: () => {
          if (footerRef.value) {
            footerRef.value.style.visibility = 'visible'
          }
        },
        scrollTrigger: {
          trigger: footerRef.value,
          start: 'top 85%',
          toggleActions: 'play none none none',
          once: true
        }
      })
    }, footerRef.value)
  })
})

onUnmounted(() => {
  // GSAP v3 context cleanup - kills all animations and ScrollTriggers
  if (ctx) {
    ctx.revert()
  }
})
</script>
