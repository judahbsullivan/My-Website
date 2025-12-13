<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`md:col-span-2 ${introData.background} opacity-0 scale-80 translate-y-8 -rotate-5`"
  >
    <div class="flex flex-col sm:flex-row items-start sm:items-center gap-4 mb-6">
      <div 
        ref="avatarRef"
        class="w-16 h-16 rounded-xl overflow-hidden border-2 border-border bg-primary/20 flex items-center justify-center shrink-0 opacity-0 scale-80 translate-y-8 -rotate-5"
      >
        <NuxtImg 
          v-if="githubStats?.avatar_url" 
          :src="githubStats.avatar_url" 
          alt="Judah Sullivan"
          width="64"
          height="64"
          fit="cover"
          quality="80"
          format="webp"
          loading="lazy"
          class="w-full h-full object-cover"
        />
        <Icon v-else name="heroicons:user" class="w-8 h-8 text-primary" />
      </div>
      <div class="flex-1 min-w-0">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-2 sm:gap-4">
          <div class="flex-1 min-w-0">
            <div ref="nameWrapperRef" class="overflow-hidden">
              <h2 
                ref="nameRef"
                class="text-xl sm:text-2xl font-bold text-foreground opacity-0"
              >
                {{ introData.name }}
              </h2>
            </div>
            <div ref="titleWrapperRef" class="overflow-hidden">
              <p 
                ref="titleRef"
                class="text-sm sm:text-base text-muted-foreground opacity-0"
              >
                {{ introData.title }}
              </p>
            </div>
            <div 
              ref="metaRef"
              class="flex flex-wrap items-center gap-2 sm:gap-3 mt-2 opacity-0 translate-y-2.5"
            >
              <span class="text-xs text-muted-foreground flex items-center gap-1">
                <Icon name="heroicons:map-pin" class="w-4 h-4 shrink-0" />
                <span class="truncate">{{ introData.location }}</span>
              </span>
              <span v-if="githubStats" class="text-xs text-muted-foreground flex items-center gap-1">
                <Icon name="heroicons:code-bracket-square" class="w-4 h-4 shrink-0" />
                <span>{{ githubStats.public_repos }} repos</span>
              </span>
            </div>
          </div>
          <div 
            v-if="introData.status" 
            ref="statusRef"
            class="px-3 py-1 text-xs font-medium rounded-full bg-success/20 text-success shrink-0 opacity-0 scale-80 translate-y-8 -rotate-5"
          >
            {{ introData.status }}
          </div>
        </div>
      </div>
    </div>
    <div ref="descriptionWrapperRef" class="overflow-hidden mb-4">
      <p 
        ref="descriptionRef"
        class="text-sm sm:text-base text-foreground leading-relaxed opacity-0"
      >
      {{ introData.description }}
    </p>
    </div>
    <div 
      ref="linksRef"
      class="flex flex-wrap items-center gap-3 sm:gap-4 pt-4 border-t border-border/50 opacity-0 translate-y-2.5"
    >
      <a 
        :href="introData.github" 
        target="_blank" 
        rel="noopener noreferrer"
        class="text-xs sm:text-sm text-primary hover:underline flex items-center gap-2"
      >
        <Icon name="heroicons:code-bracket-square" class="w-4 h-4 shrink-0" />
        GitHub
      </a>
      <a 
        v-if="introData.website"
        :href="introData.website" 
        target="_blank" 
        rel="noopener noreferrer"
        class="text-xs sm:text-sm text-primary hover:underline flex items-center gap-2"
      >
        <Icon name="heroicons:globe-alt" class="w-4 h-4 shrink-0" />
        Website
      </a>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../../composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  introData: {
    name: string
    title: string
    description: string
    location: string
    website?: string
    github: string
    status?: string
    background: string
  }
  githubStats?: {
    avatar_url: string
    public_repos: number
    followers: number
    following: number
  } | null
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()
    
    const bentoBox = storedElements.bentoBox || bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
    const avatar = storedElements.avatar || avatarRef.value
    const name = storedElements.name || nameRef.value
    const title = storedElements.title || titleRef.value
    const description = storedElements.description || descriptionRef.value
    const meta = storedElements.meta || metaRef.value
    const status = storedElements.status || statusRef.value
    const links = storedElements.links || linksRef.value
    
    const nameSplit = storedNameSplit
    const titleSplit = storedTitleSplit
    const descriptionSplit = storedDescriptionSplit
    
    if (!bentoBox) return resolve()
    
    const tl = gsap.timeline({
      onComplete: () => {
        // Clean up SplitText
        try { nameSplit?.revert?.() } catch (e) {}
        try { titleSplit?.revert?.() } catch (e) {}
        try { descriptionSplit?.revert?.() } catch (e) {}
        resolve()
      }
    })
    
    const contentDuration = 0.5
    
    // Name chars out
    if (nameSplit && nameSplit.chars && nameSplit.chars.length > 0) {
      tl.to(nameSplit.chars, {
        yPercent: -120,
        rotationX: 90,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.25, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (name) { tl.to(name, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0) }
    
    // Title lines out
    if (titleSplit && titleSplit.lines && titleSplit.lines.length > 0) {
      tl.to(titleSplit.lines, {
        yPercent: -100,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.2, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (title) { tl.to(title, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0) }
    
    // Description lines out
    if (descriptionSplit && descriptionSplit.lines && descriptionSplit.lines.length > 0) {
      tl.to(descriptionSplit.lines, {
        yPercent: -100,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.25, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (description) { tl.to(description, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0) }
    
    // Meta & links fade/slide
    const textElements = [meta, links].filter(Boolean) as HTMLElement[]
    if (textElements.length > 0) {
      tl.to(textElements, { y: -10, autoAlpha: 0, duration: contentDuration - 0.1, stagger: 0.06, ease: 'power2.in' }, 0)
    }
    
    // Avatar & status scale out
    const boxElements = [avatar, status].filter(Boolean) as HTMLElement[]
    if (boxElements.length > 0) {
      tl.to(boxElements, { scale: 0, autoAlpha: 0, duration: contentDuration, stagger: 0.08, ease: 'back.in(1.7)' }, 0)
    }
    
    // Container out slightly after content
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
defineProps<Props>()

const { isIntroLoaderComplete } = useIntroLoader()
const bentoBoxRef = ref<any>(null)
const avatarRef = ref<HTMLElement | null>(null)
const nameWrapperRef = ref<HTMLElement | null>(null)
const nameRef = ref<HTMLElement | null>(null)
const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<HTMLElement | null>(null)
const descriptionWrapperRef = ref<HTMLElement | null>(null)
const descriptionRef = ref<HTMLElement | null>(null)
const metaRef = ref<HTMLElement | null>(null)
const statusRef = ref<HTMLElement | null>(null)
const linksRef = ref<HTMLElement | null>(null)

let hasAnimated = false
let storedNameSplit: any = null
let storedTitleSplit: any = null
let storedDescriptionSplit: any = null
let storedElements: {
  bentoBox: HTMLElement | null
  avatar: HTMLElement | null
  name: HTMLElement | null
  title: HTMLElement | null
  description: HTMLElement | null
  meta: HTMLElement | null
  status: HTMLElement | null
  links: HTMLElement | null
} = {
  bentoBox: null,
  avatar: null,
  name: null,
  title: null,
  description: null,
  meta: null,
  status: null,
  links: null
}

function animateIntro() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) {
    hasAnimated = true
    return
  }
  
  nextTick(() => {
    const bentoBox = bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
    const avatar = avatarRef.value
    const name = nameRef.value
    const title = titleRef.value
    const description = descriptionRef.value
    const meta = metaRef.value
    const status = statusRef.value
    const links = linksRef.value
    
    if (!bentoBox || !name || !title || !description) {
      setTimeout(() => animateIntro(), 50)
      return
    }
    
    // Split name text (like IntroLoader)
    let nameSplit: any = null
    try {
      if (SplitText) {
        nameSplit = new SplitText(name, { 
          type: 'chars',
        })
        if (nameSplit.chars && nameSplit.chars.length > 0) {
          gsap.set(nameSplit.chars, {
            opacity: 0,
            y: 50,
            rotationX: -90
          })
        }
        storedNameSplit = nameSplit
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    // Split title into lines with mask
    let titleSplit: any = null
    try {
      if (SplitText && title) {
        titleSplit = new SplitText(title, { 
          type: 'lines',
          mask: 'lines',
          smartWrap: true,
          linesClass: 'line++',
        })
        if (titleSplit.lines && titleSplit.lines.length > 0) {
          gsap.set(titleSplit.lines, {
            opacity: 0,
            yPercent: 100
          })
        }
        storedTitleSplit = titleSplit
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    // Split description into lines with mask
    let descriptionSplit: any = null
    try {
      if (SplitText && description) {
        descriptionSplit = new SplitText(description, { 
          type: 'lines',
          mask: 'lines',
          smartWrap: true,
          linesClass: 'line++',
        })
        if (descriptionSplit.lines && descriptionSplit.lines.length > 0) {
          gsap.set(descriptionSplit.lines, {
            opacity: 0,
            yPercent: 100
          })
        }
        storedDescriptionSplit = descriptionSplit
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    // Collect elements for animation
    const boxElements = [
      avatar,
      status
    ].filter(Boolean) as HTMLElement[]
    
    const textElements = [
      meta,
      links
    ].filter(Boolean) as HTMLElement[]
    
    storedElements = { bentoBox, avatar, name, title, description, meta, status, links }
    
    // Create master timeline (matching IntroLoader pattern)
    const tl = gsap.timeline()
    
    // 1. Animate bento box first (like IntroLoader nameBox) with bounce
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
    
    // 2. Animate avatar and status boxes with bounce
    tl.fromTo(boxElements,
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
        stagger: {
          amount: 0.3,
          from: 'start'
        },
        ease: 'back.out(1.4)'
      }, '-=0.3')
    
    // 3. Animate name characters with stagger and 3D rotation
    if (nameSplit && nameSplit.chars) {
      if (name) { tl.to(name, { opacity: 1, duration: 0.001 }, '-=0.3') }
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
      tl.fromTo(name,
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
    
    // 4. Animate title lines with mask reveal
    if (titleSplit && titleSplit.lines) {
      if (title) { tl.to(title, { opacity: 1, duration: 0.001 }, '-=0.25') }
      tl.to(titleSplit.lines, {
        opacity: 1,
        yPercent: 0,
        duration: 0.5,
        stagger: {
          amount: 0.3,
          from: 'start'
        },
        ease: 'power2.out'
      }, '-=0.2')
    } else if (title) {
      // Fallback if SplitText not available
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
        }, '-=0.2')
    }
    
    // 5. Animate description lines with mask reveal
    if (descriptionSplit && descriptionSplit.lines) {
      if (description) { tl.to(description, { opacity: 1, duration: 0.001 }, '-=0.25') }
      tl.to(descriptionSplit.lines, {
        opacity: 1,
        yPercent: 0,
        duration: 0.5,
        stagger: {
          amount: 0.3,
          from: 'start'
        },
        ease: 'power2.out'
      }, '-=0.3')
    } else if (description) {
      // Fallback if SplitText not available
      tl.fromTo(description,
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
    
    // 6. Animate text elements with subtle delay
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
    
    hasAnimated = true
  })
}

// Watch for intro loader completion
watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => {
      animateIntro()
    }, 150)
  }
})

onMounted(() => {
  registerExitAnimation('aboutIntro', animateExit)
  
  nextTick(() => {
    if (!storedElements.bentoBox) {
      const bentoBox = bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
      storedElements = {
        bentoBox,
        avatar: avatarRef.value,
        name: nameRef.value,
        title: titleRef.value,
        description: descriptionRef.value,
        meta: metaRef.value,
        status: statusRef.value,
        links: linksRef.value
      }
    }
  })
  
  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateIntro()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation('aboutIntro')
  hasAnimated = false
  storedNameSplit = null
  storedTitleSplit = null
  storedDescriptionSplit = null
  storedElements = {
    bentoBox: null,
    avatar: null,
    name: null,
    title: null,
    description: null,
    meta: null,
    status: null,
    links: null
  }
})
</script>

<style scoped>
/* SplitText mask styles */
:deep(.char) {
  display: inline-block;
  transform-style: preserve-3d;
}

:deep(.line) {
  overflow: hidden;
}
</style>


