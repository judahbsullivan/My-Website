<template>
  <section class="w-full">
    <div class="px-4 sm:px-6">
      <div class="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
        <!-- About Me - Spans 7 columns, positioned top left -->
        <div class="lg:col-span-7">
          <UiBentoBox
            ref="aboutBoxRef"
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`${homepageData.sections.aboutMe.background} opacity-0 scale-95 translate-y-6 -rotate-2`"
          >
            <UiTitle ref="aboutTitleRef" as="h2" size="md" weight="semibold" class="mb-4">
              About Me
            </UiTitle>
            <div class="space-y-4">
                <div ref="aboutContentRef" class="opacity-0 translate-y-4">
                <p class="text-sm font-semibold text-secondary! mb-2">
                  {{ aboutData.bio.tagline }}
                </p>
                <p class="text-xs text-secondary! max-w-2xl leading-relaxed">
                  {{ aboutData.bio.description }}
                </p>
              </div>
              <div ref="aboutMetaRef" class="flex flex-wrap items-center gap-4 pt-4 border-t border-border/50 opacity-0 translate-y-4">
                <div class="flex items-center gap-2 text-xs text-secondary-foreground">
                  <Icon name="heroicons:map-pin" class="w-4 h-4" />
                  <span>{{ aboutData.bio.location }}</span>
                </div>
                <div v-if="aboutData.bio.status" class="px-3 py-1 text-xs font-medium rounded-full bg-success/20 text-success">
                  {{ aboutData.bio.status }}
                </div>
              </div>
              <div ref="aboutLinksRef" class="flex items-center gap-4 pt-2 opacity-0 translate-y-4">
                <a 
                  :href="aboutData.bio.github" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  class="text-xs text-primary hover:underline flex items-center gap-2"
                >
                  <Icon name="heroicons:code-bracket-square" class="w-4 h-4" />
                  GitHub
                </a>
                <a 
                  v-if="aboutData.bio.website"
                  :href="aboutData.bio.website" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  class="text-xs text-primary hover:underline flex items-center gap-2"
                >
                  <Icon name="heroicons:globe-alt" class="w-4 h-4" />
                  Website
                </a>
                <NuxtLink
                  to="/about"
                  class="text-xs text-primary hover:underline flex items-center gap-2"
                >
                  Learn more
                  <Icon name="heroicons:arrow-right" class="w-4 h-4" />
                </NuxtLink>
              </div>
            </div>
          </UiBentoBox>
        </div>

        <!-- Quick facts - Spans 5 columns, positioned top right -->
        <div class="lg:col-span-5">
          <UiBentoBox
            ref="factsBoxRef"
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`${homepageData.sections.quickFacts.background} opacity-0 scale-95 translate-y-6 rotate-2`"
          >
            <UiTitle ref="factsTitleRef" as="h3" size="sm" weight="semibold" class="mb-5">
              Quick facts
            </UiTitle>
            <div ref="factsContentRef" class="space-y-4">
              <div class="fact-row flex justify-between items-center py-2 border-b border-border/30 opacity-0 translate-y-3">
                <span class="text-sm text-muted-foreground">Experience</span>
                <span class="text-sm font-medium">X+ years</span>
              </div>
              <div class="fact-row flex justify-between items-center py-2 border-b border-border/30 opacity-0 translate-y-3">
                <span class="text-sm text-muted-foreground">Focus</span>
                <span class="text-sm font-medium">
                  Web apps, APIs, architecture
                </span>
              </div>
              <div class="fact-row flex justify-between items-center py-2 opacity-0 translate-y-3">
                <span class="text-sm text-muted-foreground">Stack</span>
                <span class="text-sm font-medium">
                  Vue/Nuxt · .NET · PostgreSQL
                </span>
              </div>
            </div>
          </UiBentoBox>
        </div>

        <!-- What I do - Carousel below, spans full width -->
        <div class="lg:col-span-12 lg:row-start-2">
          <UiBentoBox
            ref="whatIDoBoxRef"
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`${homepageData.sections.whatIDo.background} opacity-0 scale-95 translate-y-6`"
          >
            <div class="flex items-center justify-between mb-6">
              <UiTitle ref="whatIDoTitleRef" as="h2" size="md" weight="semibold">
                What I do
              </UiTitle>
              <div ref="whatIDoControlsRef" class="flex items-center gap-2 opacity-0 translate-y-3">
                <UiButton
                  @click="previousSlide"
                  variant="ghost"
                  size="sm"
                  className="p-2 rounded-lg"
                  aria-label="Previous slide"
                >
                  <Icon name="heroicons:chevron-left" class="w-5 h-5" />
                </UiButton>
                <UiButton
                  @click="nextSlide"
                  variant="ghost"
                  size="sm"
                  className="p-2 rounded-lg"
                  aria-label="Next slide"
                >
                  <Icon name="heroicons:chevron-right" class="w-5 h-5" />
                </UiButton>
              </div>
            </div>
            
            <div ref="whatIDoCarouselRef" class="relative overflow-hidden opacity-0 translate-y-4">
              <div
                ref="carouselWrapper"
                class="overflow-hidden"
              >
                <div
                  ref="carouselContainer"
                  class="flex gap-4"
                >
                  <!-- Duplicate last items at the beginning for infinite loop -->
                  <div
                    v-for="(item, index) in whatIDoItems.slice(-itemsPerView)"
                    :key="`clone-start-${index}`"
                    class="shrink-0"
                    :style="{ width: itemWidth }"
                  >
                    <UiBentoBox
                      :variant="item.variant"
                      size="md"
                      padding="md"
                      rounded="xl"
                      hover
                      className="flex-col items-start h-full"
                    >
                      <div :class="`w-10 h-10 rounded-xl ${item.iconBg} ${item.iconColor} flex items-center justify-center text-sm font-semibold mb-3`">
                        <component
                          :is="item.icon ? 'Icon' : 'span'"
                          v-if="item.icon"
                          :name="item.icon"
                          class="w-5 h-5"
                        />
                        <span v-else>{{ item.label }}</span>
                      </div>
                      <h3 class="text-sm font-semibold mb-2">{{ item.title }}</h3>
                      <p class="text-xs text-muted-foreground leading-relaxed">
                        {{ item.description }}
                      </p>
                    </UiBentoBox>
                  </div>
                  
                  <!-- Original items -->
                  <div
                    v-for="(item, index) in whatIDoItems"
                    :key="index"
                    ref="carouselItems"
                    class="carousel-item shrink-0 opacity-0 scale-95 translate-y-4"
                    :style="{ width: itemWidth }"
                  >
                    <UiBentoBox
                      :variant="item.variant"
                      size="md"
                      padding="md"
                      rounded="xl"
                      hover
                      className="flex-col items-start h-full"
                    >
                      <div :class="`w-10 h-10 rounded-xl ${item.iconBg} ${item.iconColor} flex items-center justify-center text-sm font-semibold mb-3`">
                        <component
                          :is="item.icon ? 'Icon' : 'span'"
                          v-if="item.icon"
                          :name="item.icon"
                          class="w-5 h-5"
                        />
                        <span v-else>{{ item.label }}</span>
                      </div>
                      <h3 class="text-sm font-semibold mb-2">{{ item.title }}</h3>
                      <p class="text-xs text-muted-foreground leading-relaxed">
                        {{ item.description }}
                      </p>
                    </UiBentoBox>
                  </div>
                  
                  <!-- Duplicate first items at the end for infinite loop -->
                  <div
                    v-for="(item, index) in whatIDoItems.slice(0, itemsPerView)"
                    :key="`clone-end-${index}`"
                    class="shrink-0"
                    :style="{ width: itemWidth }"
                  >
                    <UiBentoBox
                      :variant="item.variant"
                      size="md"
                      padding="md"
                      rounded="xl"
                      hover
                      className="flex-col items-start h-full"
                    >
                      <div :class="`w-10 h-10 rounded-xl ${item.iconBg} ${item.iconColor} flex items-center justify-center text-sm font-semibold mb-3`">
                        <component
                          :is="item.icon ? 'Icon' : 'span'"
                          v-if="item.icon"
                          :name="item.icon"
                          class="w-5 h-5"
                        />
                        <span v-else>{{ item.label }}</span>
                      </div>
                      <h3 class="text-sm font-semibold mb-2">{{ item.title }}</h3>
                      <p class="text-xs text-muted-foreground leading-relaxed">
                        {{ item.description }}
                      </p>
                    </UiBentoBox>
                  </div>
                </div>
              </div>
            </div>
            
            <!-- Dots indicator -->
            <div ref="whatIDoDotsRef" class="flex items-center justify-center gap-2 mt-6 opacity-0 translate-y-3">
              <UiButton
                v-for="(item, index) in whatIDoItems"
                :key="index"
                @click="goToSlide(index)"
                variant="ghost"
                size="sm"
                :className="`w-2 h-2 rounded-full p-0 min-w-0 transition-all ${currentIndex === index ? 'bg-primary w-6' : 'bg-muted-foreground/30 hover:bg-muted-foreground/50'}`"
                :aria-label="`Go to slide ${index + 1}`"
              />
          </div>
          </UiBentoBox>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import homepageData from '../../data/homepage.json'
import aboutData from '../../data/about.json'

// What I do items data
const whatIDoItems = [
  {
    title: 'Front‑end',
    description: 'Modern, responsive interfaces with Vue/Nuxt and a focus on accessibility and performance.',
    variant: 'primary' as const,
    iconBg: 'bg-primary/20',
    iconColor: 'text-primary',
    label: 'FE'
  },
  {
    title: 'Back‑end',
    description: 'Robust APIs with .NET and PostgreSQL, designed for scalability and maintainability.',
    variant: 'secondary' as const,
    iconBg: 'bg-blue-500/20',
    iconColor: 'text-blue-500',
    label: 'BE'
  },
  {
    title: 'Product thinking',
    description: 'Clear flows, thoughtful micro‑interactions, and an emphasis on communicating value to users.',
    variant: 'info' as const,
    iconBg: 'bg-purple-500/20',
    iconColor: 'text-purple-500',
    label: 'UX'
  },
  {
    title: 'Deployment',
    description: 'CI/CD pipelines, automated deployments, and infrastructure as code for reliable releases.',
    variant: 'success' as const,
    iconBg: 'bg-success/20',
    iconColor: 'text-success',
    icon: 'heroicons:rocket-launch'
  },
  {
    title: 'Cloud',
    description: 'Cloud infrastructure, serverless architectures, and scalable solutions on AWS and Azure.',
    variant: 'warning' as const,
    iconBg: 'bg-amber-500/20',
    iconColor: 'text-amber-500',
    icon: 'heroicons:cloud'
  },
  {
    title: 'Animations',
    description: 'Smooth transitions, micro-interactions, and engaging animations that enhance user experience.',
    variant: 'danger' as const,
    iconBg: 'bg-destructive/20',
    iconColor: 'text-destructive',
    icon: 'heroicons:sparkles'
  },
  {
    title: 'Database',
    description: 'Database design, optimization, and management with PostgreSQL, SQL Server, and NoSQL solutions.',
    variant: 'default' as const,
    iconBg: 'bg-muted',
    iconColor: 'text-foreground',
    icon: 'heroicons:circle-stack'
  },
  {
    title: 'DevOps',
    description: 'Containerization, orchestration, monitoring, and automation for efficient development workflows.',
    variant: 'primary' as const,
    iconBg: 'bg-primary/20',
    iconColor: 'text-primary',
    icon: 'heroicons:wrench-screwdriver'
  },
  {
    title: 'Testing',
    description: 'Unit, integration, and E2E testing to ensure code quality and reliability across the stack.',
    variant: 'secondary' as const,
    iconBg: 'bg-blue-500/20',
    iconColor: 'text-blue-500',
    icon: 'heroicons:check-circle'
  }
]

const carouselWrapper = ref<HTMLElement | null>(null)
const carouselContainer = ref<HTMLElement | null>(null)
const carouselItems = ref<(HTMLElement | null)[]>([])

// About Me refs
const aboutBoxRef = ref<any>(null)
const aboutTitleRef = ref<any>(null)
const aboutContentRef = ref<HTMLElement | null>(null)
const aboutMetaRef = ref<HTMLElement | null>(null)
const aboutLinksRef = ref<HTMLElement | null>(null)

// Quick Facts refs
const factsBoxRef = ref<any>(null)
const factsTitleRef = ref<any>(null)
const factsContentRef = ref<HTMLElement | null>(null)

// What I Do refs
const whatIDoBoxRef = ref<any>(null)
const whatIDoTitleRef = ref<any>(null)
const whatIDoControlsRef = ref<HTMLElement | null>(null)
const whatIDoCarouselRef = ref<HTMLElement | null>(null)
const whatIDoDotsRef = ref<HTMLElement | null>(null)
const currentIndex = ref(0) // Real index (0 to whatIDoItems.length - 1)
const displayIndex = ref(0) // Display index including clones
const itemsPerView = ref(4)
const autoplayInterval = ref<NodeJS.Timeout | null>(null)
const itemWidth = ref('calc(25% - 12px)') // 4 items with gap
const isAnimating = ref(false)


// Calculate max index - can slide until last item
const maxIndex = computed(() => {
  const total = whatIDoItems.length
  return Math.max(0, total - itemsPerView.value)
})

// Update items per view based on screen size
const updateItemsPerView = () => {
  if (typeof window === 'undefined') return
  
  if (window.innerWidth >= 1024) {
    itemsPerView.value = 4
    itemWidth.value = 'calc(25% - 12px)'
  } else if (window.innerWidth >= 640) {
    itemsPerView.value = 3
    itemWidth.value = 'calc(33.333% - 11px)'
  } else {
    itemsPerView.value = 2
    itemWidth.value = 'calc(50% - 8px)'
  }
  
  // Ensure currentIndex doesn't exceed maxIndex
  if (currentIndex.value > maxIndex.value) {
    currentIndex.value = maxIndex.value
  }
}

// Calculate slide distance
const getSlideDistance = () => {
  if (!carouselWrapper.value) return 0
  const wrapperWidth = carouselWrapper.value.offsetWidth
  const gapSize = 16
  const gaps = itemsPerView.value - 1
  const itemWidthPx = (wrapperWidth - (gaps * gapSize)) / itemsPerView.value
  return itemWidthPx + gapSize
}

// GSAP animation for slide transition with infinite loop
const animateSlide = (direction: 'next' | 'prev' | 'goto', targetIndex?: number) => {
  if (!carouselContainer.value || !carouselWrapper.value || isAnimating.value) return
  
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  if (!gsap) return
  
  isAnimating.value = true
  let newIndex = currentIndex.value
  let newDisplayIndex = displayIndex.value
  
  if (direction === 'next') {
    newIndex = (currentIndex.value + 1) % whatIDoItems.length
    newDisplayIndex = displayIndex.value + 1
  } else if (direction === 'prev') {
    newIndex = currentIndex.value === 0 ? whatIDoItems.length - 1 : currentIndex.value - 1
    newDisplayIndex = displayIndex.value - 1
  } else if (direction === 'goto' && targetIndex !== undefined) {
    newIndex = targetIndex
    newDisplayIndex = itemsPerView.value + targetIndex // Account for clones at start
  } else {
    isAnimating.value = false
    return
  }
  
  const slideDistance = getSlideDistance()
  const translateX = -(newDisplayIndex * slideDistance)
  
  // Animate with GSAP
  gsap.to(carouselContainer.value, {
    x: translateX,
    duration: 0.6,
    ease: 'power2.out',
    onComplete: () => {
      currentIndex.value = newIndex
      displayIndex.value = newDisplayIndex
      
      // Seamlessly loop: if at the end, jump to beginning without animation
      if (displayIndex.value >= whatIDoItems.length + itemsPerView.value) {
        displayIndex.value = itemsPerView.value
        gsap.set(carouselContainer.value, { x: -(displayIndex.value * slideDistance) })
      } else if (displayIndex.value < itemsPerView.value) {
        displayIndex.value = whatIDoItems.length + itemsPerView.value - 1
        gsap.set(carouselContainer.value, { x: -(displayIndex.value * slideDistance) })
      }
      
      isAnimating.value = false
    }
  })
}

const nextSlide = () => animateSlide('next')
const previousSlide = () => animateSlide('prev')
const goToSlide = (index: number) => animateSlide('goto', index)

// Autoplay functionality - infinite loop
const startAutoplay = () => {
  if (autoplayInterval.value) {
    clearInterval(autoplayInterval.value)
  }
  
  autoplayInterval.value = setInterval(() => {
    nextSlide()
  }, 5000) // Change slide every 5 seconds
}

const stopAutoplay = () => {
  if (autoplayInterval.value) {
    clearInterval(autoplayInterval.value)
    autoplayInterval.value = null
  }
}

onMounted(() => {
  updateItemsPerView()
  
  // Initialize display index to account for clones at the start
  displayIndex.value = itemsPerView.value
  
  window.addEventListener('resize', () => {
    updateItemsPerView()
    // Reset position on resize
    if (carouselContainer.value && carouselWrapper.value) {
      const slideDistance = getSlideDistance()
      displayIndex.value = itemsPerView.value + currentIndex.value
      const nuxtApp = useNuxtApp()
      const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
      if (gsap) {
      gsap.set(carouselContainer.value, { x: -(displayIndex.value * slideDistance) })
      }
    }
  })
  
  startAutoplay()
  
  // Initialize GSAP animation - start at the first real item (after clones)
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  if (carouselContainer.value && gsap) {
    const slideDistance = getSlideDistance()
    gsap.set(carouselContainer.value, {
      x: -(itemsPerView.value * slideDistance)
    })
  }
  
  // Setup ScrollTrigger animations (matching Hero style)
  setupScrollAnimations()
})

// ScrollTrigger animations matching Hero style
let scrollTriggers: any[] = []

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

function setupScrollAnimations() {
  if (import.meta.server) return
  
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap) return
  
  // Import ScrollTrigger
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (ScrollTrigger && gsap.registerPlugin) {
      gsap.registerPlugin(ScrollTrigger)
    }
    
    nextTick(() => {
      // Animate About Me box with inner content
      createAboutMeReveal()
      // Animate Quick Facts box with inner content (slightly later)
      createQuickFactsReveal()
      // Animate What I Do box with inner content
      createWhatIDoReveal()
    })
  }).catch(() => {
    console.warn('ScrollTrigger not available')
  })
}

function createAboutMeReveal() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) return
  
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (!ScrollTrigger) return
    
    const box = resolveEl(aboutBoxRef.value)
    const titleComponent = aboutTitleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (box?.querySelector('h2') as HTMLElement)
    const content = aboutContentRef.value
    const meta = aboutMetaRef.value
    const links = aboutLinksRef.value
    
    if (!box || !title) return
    
    // Split title with mask
    let titleSplit: any = null
    try {
      if (SplitText) {
        gsap.set(title, { lineHeight: '0.9' })
        titleSplit = new SplitText(title, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (titleSplit.chars?.length > 0) {
          titleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90, lineHeight: '0.9' })
          })
        }
      }
    } catch (e) {
      titleSplit = null
    }
    
    // Create timeline with ScrollTrigger
    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: 'top 80%',
        once: true
      }
    })
    
    // 1. Box comes in with scale, rotation, opacity (like IntroLoader)
    tl.fromTo(box, 
      { opacity: 0, scale: 0.95, y: 24, rotation: -2 },
      { opacity: 1, scale: 1, y: 0, rotation: 0, duration: 0.6, ease: 'power3.out' }
    )
    
    // 2. Title chars animate
    if (titleSplit?.chars) {
      tl.to(titleSplit.chars, {
        opacity: 1, yPercent: 0, rotationX: 0,
        duration: 0.5, stagger: { amount: 0.3, from: 'start' }, ease: 'power3.out'
      }, '-=0.3')
    }
    
    // 3. Content (tagline + description) fades in
    if (content) {
      tl.fromTo(content,
        { opacity: 0, y: 16 },
        { opacity: 1, y: 0, duration: 0.5, ease: 'power2.out' },
        '-=0.2'
      )
    }
    
    // 4. Meta (location, status) fades in
    if (meta) {
      tl.fromTo(meta,
        { opacity: 0, y: 16 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.2'
      )
    }
    
    // 5. Links fade in
    if (links) {
      tl.fromTo(links,
        { opacity: 0, y: 16 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.2'
      )
    }
    
    scrollTriggers.push(tl)
  })
}

function createQuickFactsReveal() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) return
  
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (!ScrollTrigger) return
    
    const box = resolveEl(factsBoxRef.value)
    const titleComponent = factsTitleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (box?.querySelector('h3') as HTMLElement)
    const factsContainer = factsContentRef.value
    
    if (!box || !title) return
    
    // Split title with mask
    let titleSplit: any = null
    try {
      if (SplitText) {
        gsap.set(title, { lineHeight: '0.9' })
        titleSplit = new SplitText(title, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (titleSplit.chars?.length > 0) {
          titleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90, lineHeight: '0.9' })
          })
        }
      }
    } catch (e) {
      titleSplit = null
    }
    
    // Create timeline with ScrollTrigger (slightly later start for stagger effect)
    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: 'top 78%',
        once: true
      }
    })
    
    // 1. Box comes in with scale, rotation (opposite direction), opacity
    tl.fromTo(box, 
      { opacity: 0, scale: 0.95, y: 24, rotation: 2 },
      { opacity: 1, scale: 1, y: 0, rotation: 0, duration: 0.6, ease: 'power3.out' }
    )
    
    // 2. Title chars animate
    if (titleSplit?.chars) {
      tl.to(titleSplit.chars, {
        opacity: 1, yPercent: 0, rotationX: 0,
        duration: 0.5, stagger: { amount: 0.3, from: 'start' }, ease: 'power3.out'
      }, '-=0.3')
    }
    
    // 3. Fact rows stagger in
    if (factsContainer) {
      const factRows = factsContainer.querySelectorAll('.fact-row')
      if (factRows.length > 0) {
        tl.fromTo(factRows,
          { opacity: 0, y: 12 },
          { opacity: 1, y: 0, duration: 0.4, stagger: 0.1, ease: 'power2.out' },
          '-=0.2'
        )
      }
    }
    
    scrollTriggers.push(tl)
  })
}

function createWhatIDoReveal() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) return
  
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (!ScrollTrigger) return
    
    const box = resolveEl(whatIDoBoxRef.value)
    const titleComponent = whatIDoTitleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (box?.querySelector('h2') as HTMLElement)
    const controls = whatIDoControlsRef.value
    const carousel = whatIDoCarouselRef.value
    const dots = whatIDoDotsRef.value
    
    if (!box || !title) return
    
    // Get carousel items for stagger animation
    const items = carousel?.querySelectorAll('.carousel-item')
    
    // Split title with mask
    let titleSplit: any = null
    try {
      if (SplitText) {
        gsap.set(title, { lineHeight: '0.9' })
        titleSplit = new SplitText(title, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (titleSplit.chars?.length > 0) {
          titleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90, lineHeight: '0.9' })
          })
        }
      }
    } catch (e) {
      titleSplit = null
    }
    
    // Create timeline with ScrollTrigger
    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: 'top 80%',
        once: true
      }
    })
    
    // 1. Box comes in with scale, opacity
    tl.fromTo(box, 
      { opacity: 0, scale: 0.95, y: 24 },
      { opacity: 1, scale: 1, y: 0, duration: 0.6, ease: 'power3.out' }
    )
    
    // 2. Title chars animate
    if (titleSplit?.chars) {
      tl.to(titleSplit.chars, {
        opacity: 1, yPercent: 0, rotationX: 0,
        duration: 0.5, stagger: { amount: 0.3, from: 'start' }, ease: 'power3.out'
      }, '-=0.3')
    }
    
    // 3. Controls fade in
    if (controls) {
      tl.fromTo(controls,
        { opacity: 0, y: 12 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.3'
      )
    }
    
    // 4. Carousel container fades in
    if (carousel) {
      tl.fromTo(carousel,
        { opacity: 0, y: 16 },
        { opacity: 1, y: 0, duration: 0.5, ease: 'power2.out' },
        '-=0.2'
      )
    }
    
    // 5. Carousel items stagger in with scale and rotation
    if (items && items.length > 0) {
      tl.fromTo(items,
        { opacity: 0, scale: 0.95, y: 16 },
        { 
          opacity: 1, 
          scale: 1, 
          y: 0, 
          duration: 0.5, 
          stagger: { amount: 0.4, from: 'start' }, 
          ease: 'power3.out' 
        },
        '-=0.3'
      )
    }
    
    // 6. Dots fade in
    if (dots) {
      tl.fromTo(dots,
        { opacity: 0, y: 12 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.2'
      )
    }
    
    scrollTriggers.push(tl)
  })
}

onUnmounted(() => {
  window.removeEventListener('resize', updateItemsPerView)
  stopAutoplay()
  // Cleanup ScrollTriggers
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
})
</script>

<style scoped>
/* SplitText mask styles - prevent line-height changes */
:deep(.char) {
  display: inline-block;
  overflow: hidden;
  vertical-align: baseline;
  transform-style: preserve-3d;
  line-height: 0.9;
  white-space: pre-wrap;
}

:deep(.line) {
  overflow: hidden;
}

/* Ensure titles maintain line-height */
:deep(h2),
:deep(h3) {
  line-height: 0.9 !important;
  display: block;
}
</style>

