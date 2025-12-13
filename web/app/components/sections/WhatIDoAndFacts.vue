<template>
  <section class="w-full">
    <div class="px-4 sm:px-6">
      <div class="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
        <!-- About Me - Spans 7 columns, positioned top left -->
        <div class="lg:col-span-7 flex flex-col gap-6">
          <UiBentoBox
            ref="aboutBoxRef"
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`${homeData.sections.aboutMe.background} opacity-0 scale-95 translate-y-6 -rotate-2`"
          >
            <UiTitle ref="aboutTitleRef" as="h2" size="md" weight="semibold" class="mb-4">
              About Me
            </UiTitle>
            <div class="space-y-4">
              <div ref="aboutContentRef" class="opacity-0 translate-y-4">
                <p class="text-sm font-semibold text-secondary! mb-2">
                  {{ aboutData.header.tagline }}
                </p>
                <p class="text-xs text-secondary! max-w-2xl leading-relaxed">
                  {{ aboutData.intro.description }}
                </p>
              </div>
              <div ref="aboutMetaRef" class="flex flex-wrap items-center gap-4 pt-4 border-t border-border/50 opacity-0 translate-y-4">
                <div class="flex items-center gap-2 text-xs text-secondary-foreground">
                  <Icon name="heroicons:map-pin" class="w-4 h-4" />
                  <span>{{ aboutData.intro.location }}</span>
                </div>
                <div v-if="aboutData.intro.status" class="px-3 py-1 text-xs font-medium rounded-full bg-success/20 text-success">
                  {{ aboutData.intro.status }}
                </div>
              </div>
              <div ref="aboutLinksRef" class="flex items-center gap-4 pt-2 opacity-0 translate-y-4">
                <a 
                  :href="aboutData.intro.github" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  class="text-xs text-primary hover:underline flex items-center gap-2"
                >
                  <Icon name="heroicons:code-bracket-square" class="w-4 h-4" />
                  GitHub
                </a>
                <a 
                  v-if="aboutData.intro.website"
                  :href="aboutData.intro.website" 
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

          <!-- Social Links Bento Box below About Me -->
          <UiBentoBox
            ref="socialBoxRef"
            container
            backdrop
            border
            shadow
            padding="sm"
            rounded="2xl"
            :className="`${homeData.sections.socialBox?.background || 'bg-card dark:bg-card/90'} opacity-0 scale-95 translate-y-6 rotate-2`"
          >
            <div class="w-full flex items-center justify-center sm:justify-evenly gap-2 sm:gap-3 flex-wrap sm:flex-nowrap">
              <UiButton
                v-for="social in homeData.sections.socialBox.icons"
                :key="social.name"
                :href="social.href || aboutData.intro.github"
                target="_blank"
                rel="noopener noreferrer"
                :variant="social.variant as 'primary' | 'secondary' | 'outline' | 'ghost'"
                :overlay-color="social.overlayColor"
                :class="`social-icon-link flex-1 sm:flex-1 min-w-0 items-center justify-center p-2 rounded-lg transition-colors group opacity-0 font-normal! normal-case! tracking-normal! ${social.background || ''} ${social.backgroundHover || ''}`"
              >
                <div :class="`w-10 h-10 rounded-full ${social.bubbleBg} flex items-center justify-center ${social.bubbleHover} transition-colors`">
                  <Icon :name="social.icon" :class="`w-5 h-5 ${social.iconColor}`" />
                </div>
              </UiButton>
            </div>
          </UiBentoBox>
        </div>

        <!-- Quick facts - Spans 5 columns, positioned top right -->
        <div class="lg:col-span-5 flex flex-col gap-6">
          <UiBentoBox
            ref="factsBoxRef"
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`${homeData.sections.quickFacts.background} opacity-0 scale-95 translate-y-6 rotate-2`"
          >
            <UiTitle ref="factsTitleRef" as="h3" size="sm" weight="semibold" class="mb-5">
              Quick facts
            </UiTitle>
            <div ref="factsContentRef" class="space-y-4">
              <div class="fact-row flex justify-between items-center py-2 border-b border-border/30 opacity-0 translate-y-3">
                <span class="text-sm text-muted-foreground">Experience</span>
                <span class="text-sm font-medium">{{ homeData.sections.quickFacts.experience }}</span>
              </div>
              <div class="fact-row flex justify-between items-center py-2 border-b border-border/30 opacity-0 translate-y-3">
                <span class="text-sm text-muted-foreground">Focus</span>
                <span class="text-sm font-medium">
                  {{ homeData.sections.quickFacts.focus }}
                </span>
              </div>
              <div class="fact-row flex justify-between items-center py-2 opacity-0 translate-y-3">
                <span class="text-sm text-muted-foreground">Stack</span>
                <span class="text-sm font-medium">
                  {{ homeData.sections.quickFacts.stack }}
                </span>
              </div>
            </div>
          </UiBentoBox>

          <!-- Avatar Bento Box - Same width as Quick Facts -->
          <UiBentoBox
            ref="avatarBoxRef"
            container
            backdrop
            border
            shadow
            padding="sm"
            rounded="2xl"
            :className="`${homeData.sections.avatarBox?.background || 'bg-card dark:bg-card/90'} opacity-0 scale-95 translate-y-6 -rotate-2`"
          >
            <div class="grid grid-cols-3 gap-2 sm:gap-3 items-stretch">
              <!-- Bento 1: Total Stars -->
              <UiBentoBox
                ref="innerBox1Ref"
                container
                backdrop
                border
                shadow
                padding="sm"
                rounded="xl"
                className="opacity-0 scale-90 translate-y-2 bg-yellow-500/20 dark:bg-yellow-500/15 aspect-square min-w-0"
              >
                <div class="w-full h-full flex flex-col items-center justify-center gap-1 sm:gap-2 min-w-0">
                  <div class="w-8 h-8 sm:w-10 sm:h-10 rounded-full bg-yellow-500/30 dark:bg-yellow-500/20 flex items-center justify-center shrink-0">
                    <Icon name="heroicons:star" class="w-4 h-4 sm:w-5 sm:h-5 text-orange-500 dark:text-yellow-400" />
                  </div>
                  <div class="text-center min-w-0 w-full px-1">
                    <p class="text-xs hidden sm:block text-muted-foreground mb-0.5">Total Stars</p>
                    <p class="text-sm sm:text-base font-bold text-foreground truncate">{{ totalStars || '...' }}</p>
                  </div>
                </div>
              </UiBentoBox>
              
              <!-- Bento 2: Total Contributions -->
              <UiBentoBox
                ref="innerBox2Ref"
                container
                backdrop
                border
                shadow
                padding="sm"
                rounded="xl"
                className="opacity-0 scale-90  translate-y-2 bg-success/10 dark:bg-success/5 aspect-square min-w-0"
              >
                <div class="w-full h-full flex flex-col items-center justify-center gap-1 sm:gap-2 min-w-0">
                  <div class="w-8 h-8 sm:w-10 sm:h-10 rounded-full bg-success/20 dark:bg-success/10 flex items-center justify-center shrink-0">
                    <Icon name="heroicons:chart-bar" class="w-4 h-4 sm:w-5 sm:h-5 text-success" />
                  </div>
                  <div class="text-center min-w-0 w-full px-1">
                    <p class="text-xs text-muted-foreground hidden sm:block mb-0.5">Contributions</p>
                    <p class="text-sm sm:text-base font-bold text-foreground truncate">{{ totalContributions || '...' }}</p>
                  </div>
                </div>
              </UiBentoBox>
              
              <!-- Right: Avatar - takes 1/3 width -->
              <div ref="avatarImageRef" class="w-full opacity-0 scale-90 translate-y-3 flex items-center justify-center min-w-0">
                <div class="aspect-square w-full rounded-xl overflow-hidden border-2 border-border bg-primary/10 flex items-center justify-center">
                  <NuxtImg 
                    v-if="githubAvatar" 
                    :src="githubAvatar" 
                    alt="Judah Sullivan"
                    width="200"
                    height="200"
                    fit="cover"
                    quality="80"
                    format="webp"
                    loading="lazy"
                    class="w-full h-full object-cover"
                  />
                  <Icon v-else name="heroicons:user" class="w-5 h-5 text-primary" />
                </div>
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
            :className="`${homeData.sections.whatIDo.background} opacity-0 scale-95 translate-y-6`"
          >
            <div class="flex items-center justify-between mb-6">
              <UiTitle ref="whatIDoTitleRef" as="h2" size="md" weight="semibold">
                What I do
              </UiTitle>
              <div ref="whatIDoControlsRef" class="flex items-center gap-2 opacity-0 translate-y-3">
                <UiButton
                  @click="previousSlide"
                  size="sm"
                  className="p-2 rounded-lg"
                  aria-label="Previous slide"
                >
                  <Icon name="heroicons:chevron-left" class="w-5 h-5" />
                </UiButton>
                <UiButton
                  @click="nextSlide"
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
                      :variant="item.variant as any"
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
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import homeData from '../../data/home.json'
import aboutData from '../../data/about.json'

// What I do items data
const whatIDoItems = homeData.sections.whatIDo.items

const carouselWrapper = ref<HTMLElement | null>(null)
const carouselContainer = ref<HTMLElement | null>(null)
const carouselItems = ref<(HTMLElement | null)[]>([])

// About Me refs
const aboutBoxRef = ref<any>(null)
const aboutTitleRef = ref<any>(null)
const aboutContentRef = ref<HTMLElement | null>(null)
const aboutMetaRef = ref<HTMLElement | null>(null)
const aboutLinksRef = ref<HTMLElement | null>(null)

// Social Box refs
const socialBoxRef = ref<any>(null)

// Quick Facts refs
const factsBoxRef = ref<any>(null)
const factsTitleRef = ref<any>(null)
const factsContentRef = ref<HTMLElement | null>(null)

// Avatar Box refs
const avatarBoxRef = ref<any>(null)
const avatarContentRef = ref<HTMLElement | null>(null)
const avatarImageRef = ref<HTMLElement | null>(null)
const innerBox1Ref = ref<any>(null)
const innerBox2Ref = ref<any>(null)

// Fetch GitHub stats
const { data: githubStats } = await useFetch('https://api.github.com/users/judahbsullivan', {
  default: () => null,
  server: false, // Only fetch on client to avoid CORS issues
  transform: (data: any) => {
    if (!data) return null
    return {
      avatar_url: data.avatar_url,
      public_repos: data.public_repos,
      followers: data.followers,
      following: data.following,
      created_at: data.created_at,
      bio: data.bio,
      location: data.location,
      company: data.company,
      blog: data.blog
    }
  }
})

const githubAvatar = computed(() => githubStats.value?.avatar_url || null)

// Fetch total stars from all repositories
const { data: reposData } = await useFetch('https://api.github.com/users/judahbsullivan/repos?per_page=100&sort=updated', {
  default: () => [],
  server: false,
  transform: (data: any[]) => {
    if (!data || !Array.isArray(data)) return []
    return data
  }
})

const totalStars = computed(() => {
  if (!reposData.value || !Array.isArray(reposData.value)) return null
  const stars = reposData.value.reduce((sum: number, repo: any) => sum + (repo.stargazers_count || 0), 0)
  return stars > 0 ? stars.toLocaleString() : '0'
})

// Fetch total contributions - try multiple APIs with fallback
const { data: contributionCount, error: contributionError } = await useFetch('https://github-contributions-api.jogruber.de/v4/judahbsullivan', {
  default: () => null,
  server: false,
  lazy: true,
  transform: (data: any) => {
    if (!data || !data.contributions) return null
    
    let total = 0
    const startDate = new Date('2024-02-24')
    
    // Sum contributions from Feb 24, 2024 onwards
    if (Array.isArray(data.contributions)) {
      data.contributions.forEach((year: any) => {
        if (year.weeks && Array.isArray(year.weeks)) {
          year.weeks.forEach((week: any) => {
            if (week.contributionDays && Array.isArray(week.contributionDays)) {
              week.contributionDays.forEach((day: any) => {
                if (day.date) {
                  const dayDate = new Date(day.date)
                  if (dayDate >= startDate) {
                    total += day.contributionCount || 0
                  }
                }
              })
            }
          })
        }
      })
    }
    
    return total > 0 ? total : null
  }
})

// Fallback API
const { data: altContributionData, error: altError } = await useFetch('https://github-contributions-api.deno.dev/judahbsullivan.json', {
  default: () => null,
  server: false,
  lazy: true,
  transform: (data: any) => {
    if (!data) return null
    
    let total = 0
    const startDate = new Date('2024-02-24')
    
    // Try to extract from different possible structures
    if (data.total) {
      return data.total
    }
    
    if (data.contributions && Array.isArray(data.contributions)) {
      data.contributions.forEach((year: any) => {
        if (year.weeks) {
          year.weeks.forEach((week: any) => {
            if (week.contributionDays) {
              week.contributionDays.forEach((day: any) => {
                if (day.date) {
                  const dayDate = new Date(day.date)
                  if (dayDate >= startDate) {
                    total += day.contributionCount || 0
                  }
                } else {
                  total += day.contributionCount || 0
                }
              })
            }
          })
        }
      })
    }
    
    return total > 0 ? total : null
  }
})

const totalContributions = computed(() => {
  // Try primary API first
  if (contributionCount.value !== null && contributionCount.value !== undefined && contributionCount.value > 0) {
    return contributionCount.value.toLocaleString()
  }
  
  // Try fallback API
  if (altContributionData.value !== null && altContributionData.value !== undefined && altContributionData.value > 0) {
    return altContributionData.value.toLocaleString()
  }
  
  // Fallback to known value if APIs fail
  return '808'
})

// Format date helper
const formatDate = (dateString: string | null | undefined): string => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short' })
}

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

// Store all elements on mount as fallback
function storeElementsOnMount() {
  nextTick(() => {
    // Only store if not already stored
    if (storedElements.aboutBox) return
    
    storedElements.aboutBox = resolveEl(aboutBoxRef.value)
    storedElements.socialBox = resolveEl(socialBoxRef.value)
    storedElements.factsBox = resolveEl(factsBoxRef.value)
    storedElements.avatarBox = resolveEl(avatarBoxRef.value)
    storedElements.whatIDoBox = resolveEl(whatIDoBoxRef.value)
    storedElements.aboutContent = aboutContentRef.value
    storedElements.aboutMeta = aboutMetaRef.value
    storedElements.aboutLinks = aboutLinksRef.value
    storedElements.factsContent = factsContentRef.value
    storedElements.innerBox1 = resolveEl(innerBox1Ref.value)
    storedElements.innerBox2 = resolveEl(innerBox2Ref.value)
    storedElements.avatarImage = avatarImageRef.value
    storedElements.whatIDoControls = whatIDoControlsRef.value
    storedElements.whatIDoCarousel = whatIDoCarouselRef.value
    storedElements.whatIDoDots = whatIDoDotsRef.value
    
    console.log('[WhatIDo] Stored all elements on mount (fallback)')
  })
}

onMounted(() => {
  // Register both exit and entrance animations
  registerExitAnimation('whatIDo', animateExit)
  
  // Store elements immediately as fallback
  storeElementsOnMount()
  
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

// Store SplitText instances for exit animation
let storedAboutTitleSplit: any = null
let storedFactsTitleSplit: any = null
let storedWhatIDoTitleSplit: any = null

// Store actual DOM elements for exit animation
let storedElements: {
  aboutBox: HTMLElement | null
  socialBox: HTMLElement | null
  factsBox: HTMLElement | null
  avatarBox: HTMLElement | null
  whatIDoBox: HTMLElement | null
  aboutContent: HTMLElement | null
  aboutMeta: HTMLElement | null
  aboutLinks: HTMLElement | null
  factsContent: HTMLElement | null
  innerBox1: HTMLElement | null
  innerBox2: HTMLElement | null
  avatarImage: HTMLElement | null
  whatIDoControls: HTMLElement | null
  whatIDoCarousel: HTMLElement | null
  whatIDoDots: HTMLElement | null
} = {
  aboutBox: null,
  socialBox: null,
  factsBox: null,
  avatarBox: null,
  whatIDoBox: null,
  aboutContent: null,
  aboutMeta: null,
  aboutLinks: null,
  factsContent: null,
  innerBox1: null,
  innerBox2: null,
  avatarImage: null,
  whatIDoControls: null,
  whatIDoCarousel: null,
  whatIDoDots: null
}

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
      // Animate Social Box below About Me
      createSocialBoxReveal()
      // Animate Quick Facts box with inner content (slightly later)
      createQuickFactsReveal()
      // Animate Avatar Box with inner content
      createAvatarBoxReveal()
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
    
    // Store title split for exit
    storedAboutTitleSplit = titleSplit
    
    // Store DOM elements
    storedElements.aboutBox = box
    storedElements.aboutContent = content
    storedElements.aboutMeta = meta
    storedElements.aboutLinks = links
    console.log('[WhatIDo] Stored About Me elements for exit')
    
    scrollTriggers.push(tl)
  })
}

function createSocialBoxReveal() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap) return
  
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (!ScrollTrigger) return
    
    const box = resolveEl(socialBoxRef.value)
    
    if (!box) return
    
    // Get all social icon links
    const socialLinks = box.querySelectorAll('.social-icon-link')
    
    // Create timeline with ScrollTrigger
    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: 'top 80%',
        once: true
      }
    })
    
    // 1. Box comes in with scale, rotation, opacity
    tl.fromTo(box, 
      { opacity: 0, scale: 0.95, y: 24, rotation: 2 },
      { opacity: 1, scale: 1, y: 0, rotation: 0, duration: 0.6, ease: 'power3.out' }
    )
    
    // 2. Social icons stagger in
    if (socialLinks && socialLinks.length > 0) {
      tl.fromTo(Array.from(socialLinks),
        { opacity: 0, scale: 0.8, y: 12 },
        { 
          opacity: 1, 
          scale: 1, 
          y: 0, 
          duration: 0.4, 
          stagger: 0.08, 
          ease: 'power2.out' 
        },
        '-=0.3'
      )
    }
    
    // Store DOM element
    storedElements.socialBox = box
    console.log('[WhatIDo] Stored Social Box element for exit')
    
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
    
    // Store title split for exit
    storedFactsTitleSplit = titleSplit
    
    // Store DOM elements
    storedElements.factsBox = box
    storedElements.factsContent = factsContainer
    console.log('[WhatIDo] Stored Quick Facts elements for exit')
    
    scrollTriggers.push(tl)
  })
}

function createAvatarBoxReveal() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) return
  
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (!ScrollTrigger) return
    
    const box = resolveEl(avatarBoxRef.value)
    const innerBox1 = resolveEl(innerBox1Ref.value)
    const innerBox2 = resolveEl(innerBox2Ref.value)
    const image = avatarImageRef.value
    
    if (!box) return
    
    // Create timeline with ScrollTrigger (slightly later start for stagger effect)
    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: 'top 78%',
        once: true
      }
    })
    
    // 1. Box comes in with scale, rotation (opposite direction from Quick Facts), opacity
    tl.fromTo(box, 
      { opacity: 0, scale: 0.95, y: 24, rotation: -2 },
      { opacity: 1, scale: 1, y: 0, rotation: 0, duration: 0.6, ease: 'power3.out' }
    )
    
    // 2. Stars bento, Contributions bento, and Avatar stagger in
    const elementsToAnimate = [innerBox1, innerBox2, image].filter(Boolean)
    if (elementsToAnimate.length > 0) {
      tl.fromTo(elementsToAnimate,
        { opacity: 0, y: 12, scale: 0.9 },
        { 
          opacity: 1, 
          y: 0, 
          scale: 1, 
          duration: 0.5, 
          stagger: 0.15, 
          ease: 'power2.out' 
        },
        '-=0.3'
      )
    }
    
    // Store DOM elements
    storedElements.avatarBox = box
    storedElements.innerBox1 = innerBox1
    storedElements.innerBox2 = innerBox2
    storedElements.avatarImage = image
    console.log('[WhatIDo] Stored Avatar Box elements for exit')
    
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
    
    // Store title split for exit
    storedWhatIDoTitleSplit = titleSplit
    
    // Store DOM elements
    storedElements.whatIDoBox = box
    storedElements.whatIDoControls = controls
    storedElements.whatIDoCarousel = carousel
    storedElements.whatIDoDots = dots
    console.log('[WhatIDo] Stored What I Do elements for exit')
    
    scrollTriggers.push(tl)
  })
}

// Exit animation - mirrors the enter animation for all boxes
function animateExit(): Promise<void> {
  console.log('[WhatIDo] animateExit called')
  
  return new Promise((resolve) => {
    if (import.meta.server) {
      console.log('[WhatIDo] SSR guard - resolving immediately')
      resolve()
      return
    }

    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    const SplitText = nuxtApp.$SplitText as any

    if (!gsap) {
      console.log('[WhatIDo] No GSAP - resolving immediately')
      resolve()
      return
    }

    // Use stored DOM elements (Vue refs are already cleaned up)
    const aboutBox = storedElements.aboutBox
    const socialBox = storedElements.socialBox
    const factsBox = storedElements.factsBox
    const avatarBox = storedElements.avatarBox
    const whatIDoBox = storedElements.whatIDoBox

    console.log('[WhatIDo] Using stored elements:', {
      aboutBox: !!aboutBox,
      socialBox: !!socialBox,
      factsBox: !!factsBox,
      avatarBox: !!avatarBox,
      whatIDoBox: !!whatIDoBox,
      storedAboutTitleSplit: !!storedAboutTitleSplit,
      storedFactsTitleSplit: !!storedFactsTitleSplit,
      storedWhatIDoTitleSplit: !!storedWhatIDoTitleSplit
    })

    // Get stored content elements
    const aboutContent = storedElements.aboutContent
    const aboutMeta = storedElements.aboutMeta
    const aboutLinks = storedElements.aboutLinks
    const factsContent = storedElements.factsContent
    const innerBox1 = storedElements.innerBox1
    const innerBox2 = storedElements.innerBox2
    const avatarImage = storedElements.avatarImage
    const whatIDoControls = storedElements.whatIDoControls
    const whatIDoCarousel = storedElements.whatIDoCarousel
    const whatIDoDots = storedElements.whatIDoDots

    const tl = gsap.timeline({ 
      onComplete: () => {
        console.log('[WhatIDo] Exit animation timeline complete')
        resolve()
      }
    })

    // Stop carousel autoplay
    stopAutoplay()

    const contentDuration = 0.5
    const contentStagger = 0.2

    console.log('[WhatIDo] Starting content exit animations')

    // 1. All text content and inner elements animate out first
    
    // About Me content
    if (aboutContent) {
      console.log('[WhatIDo] Animating aboutContent')
      tl.to(aboutContent, { y: -25, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    if (aboutMeta) {
      tl.to(aboutMeta, { y: -20, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    if (aboutLinks) {
      tl.to(aboutLinks, { y: -20, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    // About title (SplitText)
    if (storedAboutTitleSplit && storedAboutTitleSplit.chars) {
      console.log('[WhatIDo] Animating about title with SplitText')
      tl.to(storedAboutTitleSplit.chars, {
        yPercent: -120, rotationX: 90, autoAlpha: 0,
        duration: contentDuration, stagger: { amount: contentStagger, from: 'end' }, ease: 'power2.in'
      }, 0)
    }

    // Social icons scale out
    if (socialBox) {
      const socialLinks = socialBox.querySelectorAll('.social-icon-link')
      console.log('[WhatIDo] Found social links:', socialLinks.length)
      if (socialLinks.length > 0) {
        tl.to(Array.from(socialLinks), {
          scale: 0, autoAlpha: 0, duration: contentDuration, stagger: 0.05, ease: 'back.in(1.7)'
        }, 0)
      }
    }

    // Facts content
    if (factsContent) {
      const factRows = factsContent.querySelectorAll('.fact-row')
      console.log('[WhatIDo] Found fact rows:', factRows.length)
      if (factRows.length > 0) {
        tl.to(factRows, { y: -20, autoAlpha: 0, duration: contentDuration, stagger: 0.05, ease: 'power2.in' }, 0)
      }
    }

    // Facts title (SplitText)
    if (storedFactsTitleSplit && storedFactsTitleSplit.chars) {
      console.log('[WhatIDo] Animating facts title with SplitText')
      tl.to(storedFactsTitleSplit.chars, {
        yPercent: -120, rotationX: 90, autoAlpha: 0,
        duration: contentDuration, stagger: { amount: contentStagger, from: 'end' }, ease: 'power2.in'
      }, 0)
    }

    // Avatar box inner elements scale out
    const avatarInners = [innerBox1, innerBox2, avatarImage].filter(Boolean)
    console.log('[WhatIDo] Avatar inners:', avatarInners.length)
    if (avatarInners.length > 0) {
      tl.to(avatarInners, {
        scale: 0, autoAlpha: 0, duration: contentDuration, stagger: 0.08, ease: 'back.in(1.7)'
      }, 0)
    }

    // What I Do content
    if (whatIDoControls) {
      tl.to(whatIDoControls, { y: -15, autoAlpha: 0, scale: 0.9, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    if (whatIDoCarousel) {
      const carouselItemEls = whatIDoCarousel.querySelectorAll('.carousel-item')
      console.log('[WhatIDo] Found carousel items:', carouselItemEls.length)
      if (carouselItemEls.length > 0) {
        tl.to(carouselItemEls, {
          scale: 0.85, autoAlpha: 0, y: -25, duration: contentDuration, stagger: 0.03, ease: 'power2.in'
        }, 0)
      }
    }
    if (whatIDoDots) {
      tl.to(whatIDoDots, { y: -15, autoAlpha: 0, duration: contentDuration - 0.1, ease: 'power2.in' }, 0)
    }

    // What I Do title (SplitText)
    if (storedWhatIDoTitleSplit && storedWhatIDoTitleSplit.chars) {
      console.log('[WhatIDo] Animating whatIDo title with SplitText')
      tl.to(storedWhatIDoTitleSplit.chars, {
        yPercent: -120, rotationX: 90, autoAlpha: 0,
        duration: contentDuration, stagger: { amount: contentStagger, from: 'end' }, ease: 'power2.in'
      }, 0)
    }

    // 2. All containers fade and scale - starts after content is mostly done
    const containerDelay = contentDuration * 0.85
    const allBoxes = [aboutBox, socialBox, factsBox, avatarBox, whatIDoBox].filter(Boolean) as HTMLElement[]
    console.log('[WhatIDo] Found boxes:', allBoxes.length, 'will animate at:', containerDelay)
    
    if (allBoxes.length > 0) {
      tl.to(allBoxes, {
        autoAlpha: 0,
        scale: 0.85,
        y: -40,
        rotation: (i: number) => (i % 2 === 0 ? 5 : -5), // Alternate rotation
        duration: 0.5,
        stagger: 0.05,
        ease: 'power2.in'
      }, containerDelay)
    }

    // Cleanup splits
    tl.call(() => {
      if (storedAboutTitleSplit?.revert) {
        try { storedAboutTitleSplit.revert() } catch (e) {}
      }
      if (storedFactsTitleSplit?.revert) {
        try { storedFactsTitleSplit.revert() } catch (e) {}
      }
      if (storedWhatIDoTitleSplit?.revert) {
        try { storedWhatIDoTitleSplit.revert() } catch (e) {}
      }
      storedAboutTitleSplit = null
      storedFactsTitleSplit = null
      storedWhatIDoTitleSplit = null
    })
  })
}

// Expose the exit animation method for page transitions
defineExpose({ animateExit })

onUnmounted(() => {
  unregisterExitAnimation('whatIDo')
  
  window.removeEventListener('resize', updateItemsPerView)
  stopAutoplay()
  // Cleanup ScrollTriggers
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
  
  // Reset state for clean re-mount
  storedAboutTitleSplit = null
  storedFactsTitleSplit = null
  storedWhatIDoTitleSplit = null
  storedElements = {
    aboutBox: null,
    socialBox: null,
    factsBox: null,
    avatarBox: null,
    whatIDoBox: null,
    aboutContent: null,
    aboutMeta: null,
    aboutLinks: null,
    factsContent: null,
    innerBox1: null,
    innerBox2: null,
    avatarImage: null,
    whatIDoControls: null,
    whatIDoCarousel: null,
    whatIDoDots: null
  }
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

