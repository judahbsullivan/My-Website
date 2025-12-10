<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`mb-6 ${containerBackground} opacity-0 scale-80 translate-y-8 -rotate-5`"
  >
    <div class="flex items-center justify-between mb-6">
      <h3 
        ref="titleRef"
        class="text-xl font-bold text-foreground opacity-0 translate-y-2.5"
      >
        Skills & Technologies
      </h3>
      <Icon name="heroicons:code-bracket" class="w-6 h-6 text-primary" />
    </div>
    
    <!-- Tabs -->
    <div ref="tabsRef" class="mb-6 opacity-0 translate-y-2.5">
      <div class="flex flex-wrap gap-2 border-b border-border/50 pb-4">
        <UiButton
          v-for="category in skillCategories"
          :key="category.id"
          @click="selectedCategory = category.id"
          :class="[
            'px-4 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center gap-2',
            selectedCategory === category.id
              ? 'bg-primary text-white'
              : 'bg-muted text-muted-foreground hover:bg-muted/80 hover:text-foreground'
          ]"
        >
          <Icon :name="category.icon" class="w-4 h-4" />
          {{ category.name }}
        </UiButton>
      </div>
    </div>

    <!-- Skills Grid -->
    <div ref="skillsGridRef" class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <UiBentoBox
        v-for="(skill, index) in currentSkills"
        :key="skill.name"
        :ref="el => { if (el) skillItemRefs[index] = el as any }"
        :variant="skill.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
        size="md"
        padding="md"
        :className="`flex flex-col items-center justify-center gap-2 ${getBrandColor(skill.name)} opacity-0 scale-80 translate-y-8 -rotate-5`"
      >
        <div class="w-10 h-10 flex items-center justify-center">
          <Icon :name="skill.icon" class="w-8 h-8" />
        </div>
        <p class="text-xs font-medium text-foreground text-center">{{ skill.name }}</p>
      </UiBentoBox>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick, watch } from 'vue'
import { useNuxtApp } from '#app'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Skill {
  name: string
  icon: string
  variant: string
}

interface SkillCategory {
  id: string
  name: string
  icon: string
  skills: Skill[]
}

interface Props {
  skillCategories: SkillCategory[]
  background?: string
}

const props = defineProps<Props>()
const containerBackground = computed(() => props.background || 'bg-accent')

const selectedCategory = ref(props.skillCategories[0]?.id || '')

const currentSkills = computed(() => {
  const category = props.skillCategories.find(c => c.id === selectedCategory.value)
  return category?.skills || []
})

const getBrandColor = (skillName: string): string => {
  const brandColors: Record<string, string> = {
    'Vue.js': 'bg-emerald-500/10',
    'Nuxt': 'bg-green-500/10',
    'Next.js': 'bg-gray-500/10',
    'React': 'bg-cyan-500/10',
    'Astro': 'bg-orange-500/10',
    'Svelte': 'bg-orange-600/10',
    'Tailwind CSS': 'bg-teal-500/10',
    'TypeScript': 'bg-blue-500/10',
    'JavaScript': 'bg-yellow-500/10',
    'Node.js': 'bg-green-600/10',
    '.NET': 'bg-purple-500/10',
    'PostgreSQL': 'bg-blue-600/10',
    'Docker': 'bg-blue-400/10',
    'AWS': 'bg-orange-400/10',
    'Git': 'bg-red-500/10',
    'GitHub': 'bg-gray-600/10',
  }
  return brandColors[skillName] || ''
}

// Animation refs
const bentoBoxRef = ref<any>(null)
const titleRef = ref<HTMLElement | null>(null)
const tabsRef = ref<HTMLElement | null>(null)
const skillsGridRef = ref<HTMLElement | null>(null)
const skillItemRefs = ref<(any | null)[]>([])

// ScrollTrigger animations
let scrollTriggers: any[] = []

// Stored elements for exit
let storedElements: {
  bentoBox: HTMLElement | null
  title: HTMLElement | null
  tabs: HTMLElement | null
  skillItems: HTMLElement[]
} = {
  bentoBox: null,
  title: null,
  tabs: null,
  skillItems: []
}

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

async function setupScrollAnimation() {
  if (import.meta.server) return
  
  const { gsap, ScrollTrigger, waitForReady } = useGSAP()
  
  // Wait for ScrollTrigger to be ready
  const { gsap: readyGsap, ScrollTrigger: readyST } = await waitForReady()
  
  if (!readyGsap || !readyST) {
    console.warn('GSAP or ScrollTrigger not available')
    return
  }
  
  await nextTick()
  
      const bentoBox = resolveEl(bentoBoxRef.value)
      const title = titleRef.value
      const tabs = tabsRef.value
      
      if (!bentoBox || !title) return
      
      // Create timeline with ScrollTrigger
  const tl = readyGsap.timeline({
        scrollTrigger: {
          trigger: bentoBox,
          start: 'top 80%',
          once: true
        }
      })
      
      // 1. Animate bento box with bounce (like IntroLoader)
      tl.fromTo(bentoBox,
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
        }
      )
      
      // 2. Animate title with subtle fade
      tl.fromTo(title,
        {
          opacity: 0,
          y: 10
        },
        {
          opacity: 1,
          y: 0,
          duration: 0.4,
          ease: 'power2.out'
        }, '-=0.3')
      
      // 3. Animate tabs
      if (tabs) {
        tl.fromTo(tabs,
          {
            opacity: 0,
            y: 10
          },
          {
            opacity: 1,
            y: 0,
            duration: 0.35,
            ease: 'power2.out'
          }, '-=0.2')
      }
      
      // 4. Animate skill items with stagger (like IntroLoader boxes)
      // Query skill items directly from DOM since refs might not be ready
      const skillsGrid = skillsGridRef.value
      const skillItems = skillsGrid ? Array.from(skillsGrid.querySelectorAll('[class*="opacity-0"]')) as HTMLElement[] : []
      
      if (skillItems.length > 0) {
        tl.fromTo(skillItems,
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
      }
      
      storedElements = {
        bentoBox,
        title,
        tabs,
        skillItems
      }
      
      scrollTriggers.push(tl)
}

// Watch for category changes to re-animate skill items
watch(selectedCategory, () => {
  // Reset and re-animate skill items when category changes
  nextTick(() => {
    setTimeout(() => {
      const skillsGrid = skillsGridRef.value
      const skillItems = skillsGrid ? Array.from(skillsGrid.querySelectorAll('[class*="opacity-0"]')) as HTMLElement[] : []
      
      if (skillItems.length > 0) {
        const nuxtApp = useNuxtApp()
        const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
        
        if (!gsap) return
        
        // Set initial state
        gsap.set(skillItems, {
          opacity: 0,
          scale: 0.8,
          y: 30,
          rotation: -5
        })
        
        // Animate in with stagger
        gsap.to(skillItems, {
          opacity: 1,
          scale: 1,
          y: 0,
          rotation: 0,
          duration: 0.5,
          stagger: {
            amount: 0.3,
            from: 'start',
            ease: 'power2.out'
          },
          ease: 'back.out(1.2)'
        })
      }
    }, 50)
  })
})

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()
    
    const bentoBox = storedElements.bentoBox || resolveEl(bentoBoxRef.value)
    const title = storedElements.title || titleRef.value
    const tabs = storedElements.tabs || tabsRef.value
    const skillItems = storedElements.skillItems && storedElements.skillItems.length > 0
      ? storedElements.skillItems
      : (skillsGridRef.value ? Array.from(skillsGridRef.value.querySelectorAll('[class*="opacity-0"], [class*="scale-80"]')) as HTMLElement[] : [])
    
    if (!bentoBox) return resolve()
    
    const tl = gsap.timeline({
      onComplete: () => resolve()
    })
    
    const contentDuration = 0.5
    
    if (title) {
      tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    
    if (tabs) {
      tl.to(tabs, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    
    if (skillItems.length > 0) {
      tl.to(skillItems, {
        scale: 0,
        autoAlpha: 0,
        y: -10,
        duration: contentDuration,
        stagger: 0.05,
        ease: 'back.in(1.7)'
      }, 0)
    }
    
    const containerDelay = contentDuration * 0.85
    tl.to(bentoBox, {
      autoAlpha: 0,
      scale: 0.85,
      y: -30,
      rotation: 4,
      duration: 0.5,
      ease: 'power2.in'
    }, containerDelay)
  })
}

onMounted(() => {
  setupScrollAnimation()
  registerExitAnimation('aboutSkills', animateExit)
  
  nextTick(() => {
    if (!storedElements.bentoBox) {
      storedElements = {
        bentoBox: resolveEl(bentoBoxRef.value),
        title: titleRef.value,
        tabs: tabsRef.value,
        skillItems: skillsGridRef.value ? Array.from(skillsGridRef.value.querySelectorAll('[class*="opacity-0"], [class*="scale-80"]')) as HTMLElement[] : []
      }
    }
  })
})

onUnmounted(() => {
  // Cleanup ScrollTriggers
  scrollTriggers.forEach(st => {
    if (st && st.scrollTrigger) {
      st.scrollTrigger.kill()
    }
    if (st && st.kill) {
      st.kill()
    }
  })
  scrollTriggers = []
  
  unregisterExitAnimation('aboutSkills')
  storedElements = {
    bentoBox: null,
    title: null,
    tabs: null,
    skillItems: []
  }
})
</script>


