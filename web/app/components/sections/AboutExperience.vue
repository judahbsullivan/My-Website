<template>
  <UiBentoBox
    ref="sectionRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`${experienceData.background} opacity-0 scale-90 -rotate-3`"
  >
    <div class="flex items-center gap-3 mb-8">
      <div class="w-12 h-12 rounded-xl bg-primary/20 flex items-center justify-center">
        <Icon name="heroicons:briefcase" class="w-6 h-6 text-primary" />
      </div>
      <h3 class="experience-headline text-xl font-bold text-foreground uppercase tracking-tight overflow-hidden opacity-0">
        {{ experienceData.title }}
      </h3>
    </div>
    
    <div ref="timelineContainerRef" class="relative max-w-7xl mx-auto w-full pl-8 pr-4 md:px-0">
      <!-- Timeline line - stops at last dot (desktop) -->
      <div 
        class="absolute left-1/2 top-0 w-px border-l-2 border-dashed border-border/50 hidden md:block transform -translate-x-1/2"
        :style="{ height: `${lineHeight}px` }"
      />
      
      <!-- Gradient fill line (animated) -->
      <div 
        id="timeline-fill"
        ref="timelineFillRef"
        class="absolute left-1/2 top-0 hidden md:block transform -translate-x-1/2 w-[3px] rounded-full overflow-hidden"
        :style="timelineFillStyle"
      />
      
      <!-- Mobile background line (left side) -->
      <div class="absolute left-2 top-0 bottom-0 w-[2px] bg-border/60 rounded-full md:hidden" />
      
      <!-- Mobile gradient fill line -->
      <div
        id="timeline-fill-mobile"
        ref="timelineFillMobileRef"
        class="absolute left-2 top-0 w-[2px] rounded-full md:hidden"
        :style="timelineFillStyle"
      />
      
      <div class="relative">
        <div
          v-for="(exp, index) in experienceData.experiences"
          :key="exp.title"
          :data-last-item="index === experienceData.experiences.length - 1"
          class="timeline-item relative mb-24 md:mb-28"
        >
          <!-- Mobile timeline dot (left side) -->
          <div class="absolute top-0 -left-[30px] md:hidden z-10">
            <div class="timeline-dot relative w-4 h-4 rounded-full border-2 border-dashed border-border/50 bg-background flex items-center justify-center">
              <div class="timeline-dot-inner w-2 h-2 rounded-full bg-foreground" />
            </div>
          </div>
          
          <!-- Timeline bullet - centered on the line (desktop) -->
          <div class="absolute top-0 left-1/2 hidden md:block transform -translate-x-1/2 z-10">
            <div class="timeline-dot relative w-7 h-7 rounded-full border-2 border-dashed border-border/50 bg-background flex items-center justify-center">
              <div class="timeline-dot-inner w-2.5 h-2.5 rounded-full bg-foreground" />
            </div>
          </div>
          
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6 md:gap-10 items-start">
            <!-- Company/Date Section -->
            <div :class="[
              'text-left',
              index % 2 === 0 ? 'md:text-right md:pr-6' : 'md:text-left md:pl-6 md:order-2'
            ]">
              <div class="mb-4">
                <h5 class="timeline-company text-2xl md:text-3xl font-semibold mb-2">
                  {{ exp.company }}
                </h5>
                <small class="timeline-date text-muted-foreground text-base">
                  {{ exp.period }}
                </small>
              </div>
        </div>

            <!-- Description Section -->
            <div :class="[
              'text-left',
              index % 2 === 0 ? 'md:pl-6' : 'md:pr-6 md:order-1',
              index % 2 === 0 ? 'md:text-left' : 'md:text-right'
            ]">
              <h6 class="timeline-title text-xl md:text-2xl font-semibold mb-2 capitalize">
                {{ exp.title }}
              </h6>
              <p class="timeline-location text-muted-foreground text-sm mb-4">
                {{ exp.location }} · {{ exp.type }}
              </p>
              <p class="text-sm text-foreground mb-4">{{ exp.description }}</p>
              <ul
                :class="[
                  'timeline-bullets space-y-2 mb-4',
                  index % 2 === 0
                    ? 'list-disc list-inside text-left'
                    : 'list-disc list-inside text-left md:list-none md:text-right'
                ]"
              >
                <li
                  v-for="(responsibility, idx) in exp.responsibilities"
                  :key="idx"
                  :class="['text-base', index % 2 !== 0 ? 'md:after:ml-2 md:after:inline-block' : '']"
                  :style="index % 2 !== 0 ? { '--after-content': '\'•\'' } : {}"
                  class="md:nth-[n]:after:content-(--after-content)"
                >
                  {{ responsibility }}
          </li>
        </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick, watch } from 'vue'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'

interface Props {
  experienceData: {
    title: string
    background: string
    experiences: Array<{
      title: string
      company: string
      period: string
      location: string
      type: string
      description: string
      responsibilities: string[]
    }>
  }
}

const props = defineProps<Props>()

const sectionRef = ref<any>(null)
const timelineContainerRef = ref<HTMLElement | null>(null)
const timelineFillRef = ref<HTMLElement | null>(null)
const timelineFillMobileRef = ref<HTMLElement | null>(null)
const lineHeight = ref<number>(0)

const timelineFillStyle = computed(() => ({
  height: '0px',
  backgroundImage: 'linear-gradient(to bottom, rgba(59,130,246,0.25), rgba(168,85,247,0.25))'
}))

// Stored for exit
let storedElements: {
  bentoBox: HTMLElement | null
  headline: HTMLElement | null
  items: HTMLElement[]
} = {
  bentoBox: null,
  headline: null,
  items: []
}

const getBulletClass = (index: number) => {
  if (index % 2 !== 0) {
    // Use single quotes inside the Tailwind arbitrary value
    return 'text-base md:after:content-[\'•\'] md:after:ml-2 md:after:inline-block'
  }
  return 'text-base'
}

let scrollTriggers: any[] = []

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

function calculateLineHeight() {
  if (import.meta.server) return
  requestAnimationFrame(() => {
    if (!timelineContainerRef.value) return
    const lastItem = timelineContainerRef.value.querySelector('[data-last-item="true"]') as HTMLElement
    if (lastItem) {
      const lastItemRect = lastItem.getBoundingClientRect()
      const containerRect = timelineContainerRef.value.getBoundingClientRect()
      const dotCenter = lastItemRect.top - containerRect.top + 14
      lineHeight.value = Math.max(dotCenter, 0)
    }
  })
}

async function setupScrollAnimation() {
  if (import.meta.server) return
  
  const { waitForReady } = useGSAP()
  const { gsap: readyGsap, ScrollTrigger: readyST, SplitText } = await waitForReady()
  
  if (!readyGsap || !readyST) {
    console.warn('GSAP or ScrollTrigger not available for AboutExperience')
    return
  }
  
  await nextTick()

  const section = resolveEl(sectionRef.value)
  const container = timelineContainerRef.value
  const fillEl = timelineFillRef.value
  const fillElMobile = timelineFillMobileRef.value
  
  if (!section || !container) return
  
  const timelineItemsAll = Array.from(container.querySelectorAll('.timeline-item')) as HTMLElement[]
  storedElements = {
    bentoBox: section,
    headline: section.querySelector('.experience-headline') as HTMLElement | null,
    items: timelineItemsAll
  }
  
  const headlineEl = storedElements.headline
  if (storedElements.bentoBox) {
    readyGsap.set(storedElements.bentoBox, { opacity: 0, scale: 0.9, rotation: -3 })
    if (headlineEl) readyGsap.set(headlineEl, { opacity: 0, y: 20 })
    if (timelineItemsAll.length > 0) readyGsap.set(timelineItemsAll, { opacity: 0, y: 30 })
    
    const enterTl = readyGsap.timeline({
      scrollTrigger: {
        trigger: storedElements.bentoBox,
        start: 'top 80%',
        once: true
      }
    })
    
    enterTl.to(storedElements.bentoBox, {
      opacity: 1,
      scale: 1,
      rotation: 0,
      duration: 0.7,
      ease: 'back.out(1.4)'
    })
    
    if (headlineEl) {
      enterTl.to(headlineEl, { opacity: 1, y: 0, duration: 0.45, ease: 'power2.out' }, '-=0.3')
    }
    
    if (timelineItemsAll.length > 0) {
      enterTl.to(timelineItemsAll, {
        opacity: 1,
        y: 0,
        duration: 0.45,
        stagger: 0.06,
        ease: 'power2.out'
      }, '-=0.35')
    }
    
    scrollTriggers.push(enterTl)
  }
  
  const headlineElAnim = section.querySelector('.experience-headline') as HTMLElement
  if (headlineElAnim) {
    let headlineSplit: any = null
    try {
      if (SplitText) {
        readyGsap.set(headlineElAnim, { lineHeight: '0.9' })
        headlineSplit = new SplitText(headlineElAnim, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (headlineSplit.chars && headlineSplit.chars.length > 0) {
          headlineSplit.chars.forEach((char: HTMLElement) => {
            readyGsap.set(char, {
              opacity: 0,
              yPercent: 120,
              rotationX: -90,
              lineHeight: '0.9'
            })
          })
        }
      }
    } catch (e) {
      // SplitText failed
    }
    
    const headlineTl = readyGsap.timeline({
      scrollTrigger: {
        trigger: section,
        start: 'top 85%',
        once: true
      }
    })
    
    if (headlineSplit && headlineSplit.chars) {
      headlineTl.to(headlineSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: {
          amount: 0.4,
          from: 'start'
        },
        ease: 'power3.out'
      })
    } else {
      headlineTl.fromTo(headlineElAnim, {
        opacity: 0,
        y: 20
      }, {
        opacity: 1,
        y: 0,
        duration: 0.4,
        ease: 'power2.out'
      })
    }
    
    scrollTriggers.push(headlineTl)
  }
  
  const timelineItems = container.querySelectorAll('.timeline-item')
  const dots: Array<{ y: number; inner: HTMLElement; outer: HTMLElement; filled: boolean }> = []
  
  timelineItems.forEach((item) => {
    const companyEl = item.querySelector('.timeline-company') as HTMLElement
    const dateEl = item.querySelector('.timeline-date') as HTMLElement
    const titleEl = item.querySelector('.timeline-title') as HTMLElement
    const locationEl = item.querySelector('.timeline-location') as HTMLElement
    const bulletsEl = item.querySelector('.timeline-bullets') as HTMLElement
    const dotInner = item.querySelector('.timeline-dot-inner') as HTMLElement
    const dotOuter = item.querySelector('.timeline-dot') as HTMLElement
    
    // Set initial states
    if (companyEl) readyGsap.set(companyEl, { y: 20, autoAlpha: 0 })
    if (dateEl) readyGsap.set(dateEl, { y: 20, autoAlpha: 0 })
    if (titleEl) readyGsap.set(titleEl, { y: 20, autoAlpha: 0 })
    if (locationEl) readyGsap.set(locationEl, { y: 20, autoAlpha: 0 })
    if (bulletsEl) readyGsap.set(bulletsEl.querySelectorAll('li'), { y: 20, autoAlpha: 0 })
    if (dotInner) readyGsap.set(dotInner, { scale: 0.8, backgroundColor: 'var(--foreground)', backgroundImage: 'none' })
    if (dotOuter) readyGsap.set(dotOuter, { backgroundImage: 'none' })
    
    const itemTl = readyGsap.timeline({
      scrollTrigger: {
        trigger: item as Element,
        start: 'top 85%',
        once: true
      }
    })
    
    if (companyEl) {
      itemTl.to(companyEl, { y: 0, autoAlpha: 1, duration: 0.7, ease: 'power2.out' }, 0)
    }
    if (dateEl) {
      itemTl.to(dateEl, { y: 0, autoAlpha: 1, duration: 0.7, ease: 'power2.out' }, 0.1)
    }
    if (titleEl) {
      itemTl.to(titleEl, { y: 0, autoAlpha: 1, duration: 0.8, ease: 'power2.out' }, 0.2)
    }
    if (locationEl) {
      itemTl.to(locationEl, { y: 0, autoAlpha: 1, duration: 0.7, ease: 'power2.out' }, 0.3)
    }
    if (bulletsEl) {
      const bullets = bulletsEl.querySelectorAll('li')
      itemTl.to(bullets, {
        y: 0,
        autoAlpha: 1,
        duration: 0.6,
        stagger: 0.1,
        ease: 'power2.out'
      }, 0.4)
    }
    if (dotInner) {
      itemTl.to(dotInner, {
        scale: 1,
        duration: 0.25,
        ease: 'power1.out',
        backgroundImage: 'linear-gradient(to right, rgba(59,130,246,0.85), rgba(168,85,247,0.85))',
        backgroundColor: 'transparent'
      }, 0.15)
    }
    
    // Register dot for line-fill logic
    if (dotInner && dotOuter) {
      const itemEl = item as HTMLElement
      const dotY = itemEl.offsetTop + 14
      dots.push({ y: dotY, inner: dotInner, outer: dotOuter, filled: false })
    }
    
    scrollTriggers.push(itemTl)
  })
  
  if (fillEl) {
    const setter = readyGsap.quickSetter(fillEl, 'height', 'px')
    const maxDotY = dots.length ? Math.max(...dots.map(d => d.y)) : 0
    const total = (maxDotY ? Math.max(0, maxDotY - 2) : 0) || (lineHeight.value > 0 ? Math.max(0, lineHeight.value - 2) : 400)
    
    if (total > 0 && Number.isFinite(total) && container && container instanceof Element && document.body.contains(container)) {
      try {
        const rect = container.getBoundingClientRect()
        if (rect.width > 0 || rect.height > 0) {
          const st = readyST.create({
            trigger: container,
            start: 'top 40%',
            end: `+=${total}`,
            onUpdate: (self) => {
              const h = Math.min(total, total * self.progress)
              setter(h)
              
              const current = h
              dots.forEach((d) => {
                if (!d.filled && current >= d.y) {
                  d.filled = true
                  readyGsap.to(d.inner, {
                    backgroundImage: 'linear-gradient(to right, rgba(59,130,246,0.95), rgba(168,85,247,0.95))',
                    backgroundColor: 'transparent',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                  readyGsap.to(d.outer, {
                    backgroundImage: 'linear-gradient(to bottom, rgba(59,130,246,0.2), rgba(168,85,247,0.2))',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                } else if (d.filled && current < d.y) {
                  d.filled = false
                  readyGsap.to(d.inner, {
                    backgroundImage: 'none',
                    backgroundColor: 'var(--foreground)',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                  readyGsap.to(d.outer, {
                    backgroundImage: 'none',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                }
              })
            }
          })
          scrollTriggers.push(st as any)
        }
      } catch (error) {
        console.warn('Error creating desktop timeline ScrollTrigger:', error)
      }
    }
  }
  
  if (fillElMobile) {
    const setterMobile = readyGsap.quickSetter(fillElMobile, 'height', 'px')
    const maxDotY = dots.length ? Math.max(...dots.map(d => d.y)) : 0
    const scrollHeight = container?.scrollHeight || 0
    const totalMobile = (maxDotY ? Math.max(0, maxDotY - 2) : 0) || (scrollHeight > 0 ? scrollHeight : 400)
    
    if (totalMobile > 0 && Number.isFinite(totalMobile) && container && container instanceof Element && document.body.contains(container)) {
      try {
        const rect = container.getBoundingClientRect()
        if (rect.width > 0 || rect.height > 0) {
          const st = readyST.create({
            trigger: container,
            start: 'top 25%',
            end: `+=${totalMobile}`,
            onUpdate: (self) => {
              const h = Math.min(totalMobile, totalMobile * self.progress)
              setterMobile(h)
              
              dots.forEach((d) => {
                if (!d.filled && h >= d.y) {
                  d.filled = true
                  readyGsap.to(d.inner, {
                    backgroundImage: 'linear-gradient(to right, rgba(59,130,246,0.95), rgba(168,85,247,0.95))',
                    backgroundColor: 'transparent',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                  readyGsap.to(d.outer, {
                    backgroundImage: 'linear-gradient(to bottom, rgba(59,130,246,0.2), rgba(168,85,247,0.2))',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                } else if (d.filled && h < d.y) {
                  d.filled = false
                  readyGsap.to(d.inner, {
                    backgroundImage: 'none',
                    backgroundColor: 'var(--foreground)',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                  readyGsap.to(d.outer, {
                    backgroundImage: 'none',
                    duration: 0.2,
                    ease: 'power1.out'
                  })
                }
              })
            }
          })
          scrollTriggers.push(st as any)
        }
      } catch (error) {
        console.warn('Error creating mobile timeline ScrollTrigger:', error)
      }
    }
  }
  
  setTimeout(() => {
    try {
      readyST.refresh()
    } catch (error) {
      console.warn('ScrollTrigger refresh failed in AboutExperience:', error)
    }
  }, 100)
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    
    const { gsap: readyGsap } = useGSAP()
    if (!readyGsap) return resolve()
    
    const bentoBox = storedElements.bentoBox || resolveEl(sectionRef.value)
    const headline = storedElements.headline || (bentoBox?.querySelector('.experience-headline') as HTMLElement | null)
    const items = storedElements.items && storedElements.items.length > 0
      ? storedElements.items
      : Array.from((bentoBox?.querySelectorAll('.timeline-item') || []) as NodeListOf<HTMLElement>)
    
    if (!bentoBox) return resolve()
    
    const tl = readyGsap.timeline({
      onComplete: () => resolve()
    })
    
    const contentDuration = 0.5
    
    if (headline) {
      tl.to(headline, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    
    if (items.length > 0) {
      const itemBlocks: HTMLElement[] = []
      items.forEach(item => {
        itemBlocks.push(item)
        const texts = item.querySelectorAll('.timeline-company, .timeline-date, .timeline-title, .timeline-location, .timeline-bullets li')
        texts.forEach(el => itemBlocks.push(el as HTMLElement))
      })
      tl.to(itemBlocks, { y: -12, autoAlpha: 0, duration: contentDuration, stagger: 0.05, ease: 'power2.in' }, 0)
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

let timeoutId: ReturnType<typeof setTimeout> | null = null

watch(() => props.experienceData.experiences, () => {
  if (import.meta.server) return
  setTimeout(calculateLineHeight, 150)
}, { deep: true })

onMounted(() => {
  if (import.meta.server) return
  
  registerExitAnimation('aboutExperience', animateExit)
  
  timeoutId = setTimeout(calculateLineHeight, 150)
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', calculateLineHeight)
  }
  
  setupScrollAnimation()
})

onUnmounted(() => {
  if (import.meta.server) return
  
  unregisterExitAnimation('aboutExperience')
  
  if (timeoutId) clearTimeout(timeoutId)
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', calculateLineHeight)
  }
  
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
  storedElements = { bentoBox: null, headline: null, items: [] }
})
</script>


