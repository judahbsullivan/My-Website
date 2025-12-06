<template>
  <section class="w-full">
    <div class="px-4 sm:px-6">
      <UiBentoBox
        ref="bannerBoxRef"
        container
        backdrop
        border
        shadow
        padding="none"
        rounded="2xl"
        :className="`${homepageData.sections.marquee?.background || 'bg-primary/5'} opacity-0 scale-95 translate-y-6 overflow-hidden`"
      >
        <div class="relative py-6 overflow-hidden space-y-4">
          <!-- Gradient overlays for smooth fade -->
          <div class="absolute left-0 top-0 bottom-0 w-24 bg-linear-to-r from-background/90 to-transparent z-10 pointer-events-none" />
          <div class="absolute right-0 top-0 bottom-0 w-24 bg-linear-to-l from-background/90 to-transparent z-10 pointer-events-none" />
          
          <!-- Row 1: Going Left -->
          <div ref="marqueeRow1Ref" class="flex opacity-0">
            <div ref="row1Track1Ref" class="flex shrink-0 items-center gap-8 px-4">
              <template v-for="(item, index) in row1Items" :key="`row1-1-${index}`">
                <div class="flex items-center gap-3 text-foreground/80">
                  <Icon v-if="item.icon" :name="item.icon" class="w-5 h-5 text-primary" />
                  <span class="text-sm font-medium whitespace-nowrap">{{ item.text }}</span>
                </div>
                <span class="text-primary/40">•</span>
              </template>
            </div>
            <div ref="row1Track2Ref" class="flex shrink-0 items-center gap-8 px-4">
              <template v-for="(item, index) in row1Items" :key="`row1-2-${index}`">
                <div class="flex items-center gap-3 text-foreground/80">
                  <Icon v-if="item.icon" :name="item.icon" class="w-5 h-5 text-primary" />
                  <span class="text-sm font-medium whitespace-nowrap">{{ item.text }}</span>
                </div>
                <span class="text-primary/40">•</span>
              </template>
            </div>
          </div>
          
          <!-- Row 2: Going Right (Opposite) -->
          <div ref="marqueeRow2Ref" class="flex opacity-0">
            <div ref="row2Track1Ref" class="flex shrink-0 items-center gap-8 px-4">
              <template v-for="(item, index) in row2Items" :key="`row2-1-${index}`">
                <div class="flex items-center gap-3 text-foreground/70">
                  <Icon v-if="item.icon" :name="item.icon" class="w-5 h-5 text-secondary-foreground" />
                  <span class="text-sm font-medium whitespace-nowrap">{{ item.text }}</span>
                </div>
                <span class="text-secondary-foreground/40">◆</span>
              </template>
            </div>
            <div ref="row2Track2Ref" class="flex shrink-0 items-center gap-8 px-4">
              <template v-for="(item, index) in row2Items" :key="`row2-2-${index}`">
                <div class="flex items-center gap-3 text-foreground/70">
                  <Icon v-if="item.icon" :name="item.icon" class="w-5 h-5 text-secondary-foreground" />
                  <span class="text-sm font-medium whitespace-nowrap">{{ item.text }}</span>
                </div>
                <span class="text-secondary-foreground/40">◆</span>
              </template>
            </div>
          </div>
          
          <!-- Row 3: Going Left (Same as Row 1) -->
          <div ref="marqueeRow3Ref" class="flex opacity-0">
            <div ref="row3Track1Ref" class="flex shrink-0 items-center gap-8 px-4">
              <template v-for="(item, index) in row3Items" :key="`row3-1-${index}`">
                <div class="flex items-center gap-3 text-foreground/80">
                  <Icon v-if="item.icon" :name="item.icon" class="w-5 h-5 text-primary" />
                  <span class="text-sm font-medium whitespace-nowrap">{{ item.text }}</span>
                </div>
                <span class="text-primary/40">•</span>
              </template>
            </div>
            <div ref="row3Track2Ref" class="flex shrink-0 items-center gap-8 px-4">
              <template v-for="(item, index) in row3Items" :key="`row3-2-${index}`">
                <div class="flex items-center gap-3 text-foreground/80">
                  <Icon v-if="item.icon" :name="item.icon" class="w-5 h-5 text-primary" />
                  <span class="text-sm font-medium whitespace-nowrap">{{ item.text }}</span>
                </div>
                <span class="text-primary/40">•</span>
              </template>
            </div>
          </div>
        </div>
      </UiBentoBox>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import homepageData from '../../data/homepage.json'

// Row 1: Frontend & Frameworks
const row1Items = [
  { text: 'Vue.js', icon: 'logos:vue' },
  { text: 'Nuxt', icon: 'logos:nuxt-icon' },
  { text: 'TypeScript', icon: 'logos:typescript-icon' },
  { text: 'Tailwind CSS', icon: 'logos:tailwindcss-icon' },
  { text: 'GSAP Animations', icon: 'heroicons:sparkles' },
  { text: 'React', icon: 'logos:react' },
  { text: 'Next.js', icon: 'logos:nextjs-icon' },
  { text: 'JavaScript', icon: 'logos:javascript' },
]

// Row 2: Backend & Infrastructure
const row2Items = [
  { text: '.NET Core', icon: 'logos:dotnet' },
  { text: 'PostgreSQL', icon: 'logos:postgresql' },
  { text: 'REST APIs', icon: 'heroicons:server' },
  { text: 'Docker', icon: 'logos:docker-icon' },
  { text: 'AWS', icon: 'logos:aws' },
  { text: 'Azure', icon: 'logos:microsoft-azure' },
  { text: 'Redis', icon: 'logos:redis' },
  { text: 'GraphQL', icon: 'logos:graphql' },
]

// Row 3: Skills & Practices
const row3Items = [
  { text: 'UI/UX Design', icon: 'heroicons:paint-brush' },
  { text: 'Performance Optimization', icon: 'heroicons:bolt' },
  { text: 'Cloud Infrastructure', icon: 'heroicons:cloud' },
  { text: 'CI/CD Pipelines', icon: 'heroicons:rocket-launch' },
  { text: 'Agile Development', icon: 'heroicons:squares-2x2' },
  { text: 'Code Review', icon: 'heroicons:code-bracket' },
  { text: 'Testing', icon: 'heroicons:check-badge' },
  { text: 'Documentation', icon: 'heroicons:document-text' },
]

const bannerBoxRef = ref<any>(null)
const marqueeRow1Ref = ref<HTMLElement | null>(null)
const marqueeRow2Ref = ref<HTMLElement | null>(null)
const marqueeRow3Ref = ref<HTMLElement | null>(null)
const row1Track1Ref = ref<HTMLElement | null>(null)
const row1Track2Ref = ref<HTMLElement | null>(null)
const row2Track1Ref = ref<HTMLElement | null>(null)
const row2Track2Ref = ref<HTMLElement | null>(null)
const row3Track1Ref = ref<HTMLElement | null>(null)
const row3Track2Ref = ref<HTMLElement | null>(null)

let scrollTriggers: any[] = []
let marqueeTweens: any[] = []

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

function setupScrollAnimation() {
  if (import.meta.server) return
  
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap) return
  
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (ScrollTrigger && gsap.registerPlugin) {
      gsap.registerPlugin(ScrollTrigger)
    }
    
    nextTick(() => {
      const box = resolveEl(bannerBoxRef.value)
      const row1 = marqueeRow1Ref.value
      const row2 = marqueeRow2Ref.value
      const row3 = marqueeRow3Ref.value
      
      if (!box) return
      
      // Create timeline with ScrollTrigger
      const tl = gsap.timeline({
        scrollTrigger: {
          trigger: box,
          start: 'top 85%',
          once: true
        }
      })
      
      // 1. Box comes in with scale, opacity (matching other sections)
      tl.fromTo(box, 
        { opacity: 0, scale: 0.95, y: 24 },
        { opacity: 1, scale: 1, y: 0, duration: 0.6, ease: 'power3.out' }
      )
      
      // 2. Rows fade in with stagger
      const rows = [row1, row2, row3].filter(Boolean)
      if (rows.length > 0) {
        tl.fromTo(rows,
          { opacity: 0, y: 10 },
          { opacity: 1, y: 0, duration: 0.4, stagger: 0.1, ease: 'power2.out' },
          '-=0.3'
        )
      }
      
      // 3. Start marquee animations after entrance completes
      tl.call(() => {
        startMarquees(gsap)
      })
      
      scrollTriggers.push(tl)
    })
  }).catch(() => {
    console.warn('ScrollTrigger not available')
  })
}

function startMarquees(gsap: any) {
  // Row 1: Going Left
  if (row1Track1Ref.value && row1Track2Ref.value) {
    const trackWidth = row1Track1Ref.value.offsetWidth
    const tween = gsap.to([row1Track1Ref.value, row1Track2Ref.value], {
      x: `-=${trackWidth}`,
      duration: 25,
      ease: 'none',
      repeat: -1,
      modifiers: {
        x: (x: string) => `${parseFloat(x) % trackWidth}px`
      }
    })
    marqueeTweens.push(tween)
  }
  
  // Row 2: Going Right (Opposite direction)
  if (row2Track1Ref.value && row2Track2Ref.value) {
    const trackWidth = row2Track1Ref.value.offsetWidth
    // Start from negative position to go right
    gsap.set([row2Track1Ref.value, row2Track2Ref.value], { x: -trackWidth })
    const tween = gsap.to([row2Track1Ref.value, row2Track2Ref.value], {
      x: `+=${trackWidth}`,
      duration: 30,
      ease: 'none',
      repeat: -1,
      modifiers: {
        x: (x: string) => {
          const xNum = parseFloat(x)
          // Wrap around when going right
          return `${((xNum % trackWidth) + trackWidth) % trackWidth - trackWidth}px`
        }
      }
    })
    marqueeTweens.push(tween)
  }
  
  // Row 3: Going Left (Same as Row 1, slightly different speed)
  if (row3Track1Ref.value && row3Track2Ref.value) {
    const trackWidth = row3Track1Ref.value.offsetWidth
    const tween = gsap.to([row3Track1Ref.value, row3Track2Ref.value], {
      x: `-=${trackWidth}`,
      duration: 28,
      ease: 'none',
      repeat: -1,
      modifiers: {
        x: (x: string) => `${parseFloat(x) % trackWidth}px`
      }
    })
    marqueeTweens.push(tween)
  }
}

onMounted(() => {
  setupScrollAnimation()
})

onUnmounted(() => {
  // Cleanup ScrollTriggers
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
  
  // Kill all marquee tweens
  marqueeTweens.forEach((tween) => {
    if (tween?.kill) tween.kill()
  })
  marqueeTweens = []
})
</script>
