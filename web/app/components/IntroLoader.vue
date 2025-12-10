<template>
  <!-- Full screen loader - this IS the page content during loading -->
  <div 
    ref="loaderScreenRef"
    :class="[
      'min-h-screen w-full flex items-center justify-center overflow-hidden',
      isDark ? 'bg-gray-950' : 'bg-white'
    ]"
  >
    <!-- Bento Grid Container -->
    <div v-if="showLoader">
      <div 
        ref="bentoContainerRef"
        class="w-full max-w-4xl px-4 sm:px-6 opacity-0"
      >
          <div class="grid grid-cols-2 sm:grid-cols-4 gap-3 sm:gap-4">
          <!-- Main Name Box - Spans 2 columns, 2 rows on desktop, full width on mobile -->
          <div 
            ref="nameBoxRef"
            :class="[
              'col-span-2 sm:row-span-2 rounded-2xl p-4 sm:p-6 flex flex-col justify-center',
              'border backdrop-blur-sm',
              'opacity-0 scale-80 translate-y-8 -rotate-5',
              isDark 
                ? 'bg-card/90 border-border/50' 
                : 'bg-card border-border/40'
            ]"
          >
            <div ref="nameRef" class="text-3xl sm:text-4xl md:text-5xl lg:text-6xl font-bold">
              <div class="overflow-hidden">
                <span class="block">Judah</span>
              </div>
              <div class="overflow-hidden">
                <span class="block">Sullivan</span>
              </div>
            </div>
            <div 
              ref="taglineRef"
              :class="[
                'mt-2 sm:mt-3 text-xs sm:text-sm md:text-base text-muted-foreground',
                'opacity-0 translate-y-2.5'
              ]"
            >
              Full Stack Developer
            </div>
          </div>

          <!-- Role/Tagline Box -->
          <div 
            ref="roleBoxRef"
            :class="[
              'col-span-2 sm:col-span-2 rounded-xl p-3 sm:p-4 flex items-center gap-2 sm:gap-3',
              'border backdrop-blur-sm',
              'opacity-0 scale-80 translate-y-8 -rotate-5',
              isDark
                ? 'bg-primary/15 border-primary/30'
                : 'bg-primary/10 border-primary/20'
            ]"
          >
            <div 
              :class="[
                'w-10 h-10 rounded-lg flex items-center justify-center',
                isDark ? 'bg-primary/30' : 'bg-primary/20'
              ]"
            >
              <Icon name="heroicons:code-bracket-square" class="w-5 h-5 text-primary" />
            </div>
            <div ref="roleTextRef" class="flex-1 opacity-0 translate-y-2.5">
              <div class="text-xs font-medium text-muted-foreground mb-1">Building</div>
              <div class="text-sm font-semibold text-foreground">Modern Web Experiences</div>
            </div>
          </div>

          <!-- Location Box -->
          <div 
            ref="locationBoxRef"
            :class="[
              'col-span-1 rounded-xl p-3 sm:p-4 flex flex-col items-center justify-center gap-1.5 sm:gap-2',
              'border backdrop-blur-sm',
              'opacity-0 scale-80 translate-y-8 -rotate-5',
              isDark
                ? 'bg-secondary/15 border-secondary/30'
                : 'bg-secondary/10 border-secondary/20'
            ]"
          >
            <Icon name="heroicons:map-pin" 
              :class="[
                'w-4 h-4 sm:w-5 sm:h-5',
                isDark ? 'text-blue-400' : 'text-blue-600'
              ]" 
            />
            <div ref="locationTextRef" class="text-[10px] sm:text-xs font-medium text-foreground text-center opacity-0 translate-y-2.5">
              Houston, TX
            </div>
          </div>

          <!-- Status Box -->
          <div 
            ref="statusBoxRef"
            :class="[
              'col-span-1 rounded-xl p-3 sm:p-4 flex flex-col items-center justify-center gap-1.5 sm:gap-2',
              'border backdrop-blur-sm',
              'opacity-0 scale-80 translate-y-8 -rotate-5',
              isDark
                ? 'bg-success/15 border-success/30'
                : 'bg-success/10 border-success/20'
            ]"
          >
            <div 
              :class="[
                'w-2 h-2 rounded-full animate-pulse',
                isDark ? 'bg-success' : 'bg-green-600'
              ]"
            />
            <div ref="statusTextRef" class="text-[10px] sm:text-xs font-medium text-foreground text-center opacity-0 translate-y-2.5">
              Available
            </div>
          </div>

          <!-- Tech Stack Boxes -->
          <div 
            v-for="(tech, index) in techStack"
            :key="index"
            :ref="el => { if (el) techBoxRefs[index] = el as HTMLElement }"
            :class="[
              'col-span-1 rounded-xl p-2.5 sm:p-3 flex flex-col items-center justify-center gap-1.5 sm:gap-2',
              'border backdrop-blur-sm',
              'opacity-0 scale-80 translate-y-8 -rotate-5',
              tech.bgClass,
              tech.borderClass
            ]"
          >
            <Icon :name="tech.icon" :class="['w-5 h-5 sm:w-6 sm:h-6', tech.iconClass]" />
            <div 
              :ref="el => { if (el) techTextRefs[index] = el as HTMLElement }"
              class="text-[10px] sm:text-xs font-medium text-foreground text-center opacity-0 translate-y-2.5"
            >
              {{ tech.name }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick, computed } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../composables/useIntroLoader'

const colorMode = useColorMode()
const { isIntroLoaderComplete } = useIntroLoader()
const showLoader = ref(false)

// Safe getter for color mode value
const isDark = computed(() => {
  if (import.meta.server) return false
  return colorMode.value === 'dark'
})
const loaderScreenRef = ref<HTMLElement | null>(null)
const bentoContainerRef = ref<HTMLElement | null>(null)
const nameBoxRef = ref<HTMLElement | null>(null)
const nameRef = ref<HTMLElement | null>(null)
const taglineRef = ref<HTMLElement | null>(null)
const roleBoxRef = ref<HTMLElement | null>(null)
const roleTextRef = ref<HTMLElement | null>(null)
const locationBoxRef = ref<HTMLElement | null>(null)
const locationTextRef = ref<HTMLElement | null>(null)
const statusBoxRef = ref<HTMLElement | null>(null)
const statusTextRef = ref<HTMLElement | null>(null)
const techBoxRefs = ref<(HTMLElement | null)[]>([])
const techTextRefs = ref<(HTMLElement | null)[]>([])

const techStack = computed(() => {
  const dark = isDark.value
  return [
    {
      name: 'Vue',
      icon: 'logos:vue',
      bgClass: dark ? 'bg-primary/15' : 'bg-primary/10',
      borderClass: dark ? 'border-primary/30' : 'border-primary/20',
      iconClass: 'text-primary'
    },
    {
      name: 'Nuxt',
      icon: 'logos:nuxt-icon',
      bgClass: dark ? 'bg-green-500/15' : 'bg-green-500/10',
      borderClass: dark ? 'border-green-500/30' : 'border-green-500/20',
      iconClass: 'text-green-500'
    },
    {
      name: '.NET',
      icon: 'logos:dotnet',
      bgClass: dark ? 'bg-purple-500/15' : 'bg-purple-500/10',
      borderClass: dark ? 'border-purple-500/30' : 'border-purple-500/20',
      iconClass: 'text-purple-500'
    },
    {
      name: 'TS',
      icon: 'logos:typescript-icon',
      bgClass: dark ? 'bg-blue-500/15' : 'bg-blue-500/10',
      borderClass: dark ? 'border-blue-500/30' : 'border-blue-500/20',
      iconClass: 'text-blue-500'
    }
  ]
})

onMounted(() => {
  if (import.meta.server) {
    showLoader.value = false
    return
  }
  
  // Check if loader was already completed (e.g., on navigation)
  if (isIntroLoaderComplete.value) {
    return
  }
  
  // Show loader on mount (initial load)
  showLoader.value = true
  
  // Wait for DOM to be ready
  nextTick(() => {
    setTimeout(() => {
      startAnimation()
    }, 200)
  })
})

function startAnimation() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  const { setIntroLoaderComplete } = useIntroLoader()
  
  // Fallback: if GSAP is not available, mark as complete immediately
  if (!gsap) {
    showLoader.value = false
    // Small delay to ensure state update
    setTimeout(() => {
      setIntroLoaderComplete(true)
    }, 100)
    return
  }
  
  // Wait for refs to be ready
  nextTick(() => {
    if (!loaderScreenRef.value || !bentoContainerRef.value || !nameBoxRef.value || !nameRef.value) {
      setTimeout(() => startAnimation(), 50)
      return
    }
    
    const loaderScreen = loaderScreenRef.value
    const container = bentoContainerRef.value
    const nameBox = nameBoxRef.value
    const nameEl = nameRef.value
    const tagline = taglineRef.value
    const roleBox = roleBoxRef.value
    const roleText = roleTextRef.value
    const locationBox = locationBoxRef.value
    const locationText = locationTextRef.value
    const statusBox = statusBoxRef.value
    const statusText = statusTextRef.value
    
    // Collect all bento boxes for animation
    const allBoxes = [
      nameBox,
      roleBox,
      locationBox,
      statusBox,
      ...techBoxRefs.value.filter(Boolean)
    ].filter(Boolean) as HTMLElement[]
    
    // Split name text and set initial states for characters
    let nameSplit: any = null
    try {
      if (SplitText) {
        nameSplit = new SplitText(nameEl, { 
          type: 'chars',
        })
        if (nameSplit.chars && nameSplit.chars.length > 0) {
          // Set initial states for characters (can't be done with Tailwind)
          gsap.set(nameSplit.chars, {
            opacity: 0,
            y: 50,
            rotationX: -90
          })
        }
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    // Collect text elements (initial states already set in template with Tailwind)
    const textElements = [
      tagline,
      roleText,
      locationText,
      statusText,
      ...techTextRefs.value.filter(Boolean)
    ].filter(Boolean) as HTMLElement[]
    
    // Create master timeline
    const tl = gsap.timeline()
    
    // 1. Fade in container with scale
    gsap.set(container, {
      opacity: 0,
      scale: 0.9
    })
    tl.fromTo(container, 
      {
        opacity: 0,
        scale: 0.9
      },
      {
        opacity: 1,
        scale: 1,
        duration: 0.6,
        ease: 'power3.out'
      }
    )
    
    // 2. Animate name box first (largest) with bounce
    tl.fromTo(nameBox,
      {
        opacity: 0,
        scale: 0.8,
        y: 30,
        rotation: -5
      },
      {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.6,
        ease: 'back.out(1.4)'
      }, '-=0.3')
    
    // 3. Animate name characters with stagger and 3D rotation
    if (nameSplit && nameSplit.chars) {
      tl.to(nameSplit.chars, {
        opacity: 1,
        y: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: {
          amount: 0.4,
          from: 'start'
        },
        ease: 'power3.out'
      }, '-=0.3')
    } else {
      // Fallback if SplitText not available
      tl.fromTo(nameEl,
        {
          opacity: 0,
          y: 20
        },
        {
          opacity: 1,
          y: 0,
          duration: 0.4,
          ease: 'power2.out'
        }, '-=0.2')
    }
    
    // 4. Animate tagline with subtle fade
    if (tagline) {
      tl.fromTo(tagline,
        {
          opacity: 0,
          y: 10
        },
        {
          opacity: 1,
          y: 0,
          duration: 0.4,
          ease: 'power2.out'
        }, '-=0.2')
    }
    
    // 5. Animate other boxes with creative stagger (wave effect)
    const otherBoxes = [roleBox, locationBox, statusBox, ...techBoxRefs.value.filter(Boolean)].filter(Boolean) as HTMLElement[]
    tl.fromTo(otherBoxes,
      {
        opacity: 0,
        scale: 0.8,
        y: 30,
        rotation: -5
      },
      {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.5,
        stagger: {
          amount: 0.4,
          from: 'start',
          ease: 'power2.out'
        },
        ease: 'back.out(1.2)'
      }, '-=0.3')
    
    // 6. Animate text elements in boxes with subtle delay
    tl.fromTo(textElements,
      {
        opacity: 0,
        y: 10
      },
      {
        opacity: 1,
        y: 0,
        duration: 0.35,
        stagger: 0.06,
        ease: 'power2.out'
      }, '-=0.4')
    
    // 7. Hold for a moment to appreciate the animation
    tl.to({}, { duration: 1 })
    
    // 8. Animate boxes out with stagger (reverse order) - creative exit
    const reversedBoxes = [...otherBoxes].reverse()
    tl.to(reversedBoxes, {
      opacity: 0,
      scale: 0.85,
      y: -25,
      rotation: 8,
      duration: 0.35,
      stagger: {
        amount: 0.25,
        from: 'start'
      },
      ease: 'power2.in'
    }, '-=0.1')
    
    // 9. Animate name box out with 3D effect
    if (nameSplit && nameSplit.chars) {
      tl.to(nameSplit.chars, {
        opacity: 0,
        y: -40,
        rotationX: 90,
        duration: 0.35,
        stagger: {
          amount: 0.2,
          from: 'start'
        },
        ease: 'power2.in'
      }, '-=0.15')
    }
    
    tl.to(nameBox, {
      opacity: 0,
      scale: 0.9,
      y: -25,
      rotation: 5,
      duration: 0.4,
      ease: 'power2.in'
    }, '-=0.2')
    
    // 10. Fade out container smoothly
    tl.to(container, {
      opacity: 0,
      scale: 0.92,
      y: -15,
      duration: 0.35,
      ease: 'power2.in'
    }, '-=0.25')
    
    // 11. Fade out the entire loader screen
    tl.to(loaderScreen, {
      opacity: 0,
      duration: 0.5,
      ease: 'power2.inOut'
    }, '-=0.2')
    
    // Cleanup
    tl.call(() => {
      if (nameSplit && nameSplit.revert) {
        nameSplit.revert()
      }
      showLoader.value = false
      // Ensure state is set - use nextTick to ensure reactivity
      nextTick(() => {
        setIntroLoaderComplete(true)
        // Force a small delay to ensure the state propagates
        setTimeout(() => {
          // Double check the state is set
          if (!isIntroLoaderComplete.value) {
            setIntroLoaderComplete(true)
          }
          // Refresh ScrollTrigger after intro completes to ensure proper initialization
          import('gsap/ScrollTrigger').then((stModule) => {
            const ScrollTrigger = stModule.default || stModule
            if (ScrollTrigger) {
              // Multiple refreshes to ensure everything is properly initialized
              ScrollTrigger.refresh()
              setTimeout(() => ScrollTrigger.refresh(), 100)
              setTimeout(() => ScrollTrigger.refresh(), 300)
            }
          }).catch(() => {})
        }, 50)
      })
    })
  })
}

</script>

<style scoped>
/* Ensure smooth animations */
:deep(.char) {
  display: inline-block;
  transform-style: preserve-3d;
}
</style>
