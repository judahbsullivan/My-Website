<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 ">
      <!-- Header -->
      <div class="mb-12">
        <UiTitle as="h1" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl mb-4">
          Animation Playground
        </UiTitle>
        <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed">
          Explore interactive components, animations, and code snippets. Copy, paste, and customize to your heart's
          content.
        </p>
      </div>

      <!-- Featured Playgrounds -->
      <div v-if="featuredPlaygrounds && featuredPlaygrounds.length > 0" class="mb-12">
        <h2 class="text-2xl font-bold text-foreground mb-6">Featured</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <NuxtLink v-for="item in featuredPlaygrounds" :key="item.id" :to="`/playground/${item.slug}`" class="group">
            <div
              class="bg-card/80 backdrop-blur-xl rounded-2xl border-2 border-border overflow-hidden hover:border-primary/50 transition-all duration-300">
              <!-- Preview Image or Placeholder -->
              <div class="aspect-video bg-muted relative overflow-hidden">
                <img v-if="item.previewImageUrl" :src="getImageUrl(item.previewImageUrl)" :alt="item.title"
                  class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300" />
                <div v-else class="w-full h-full flex items-center justify-center">
                  <Icon name="heroicons:code-bracket" class="w-12 h-12 text-muted-foreground" />
                </div>
                <div class="absolute top-3 right-3 flex gap-2">
                  <span :class="[
                    'px-2 py-1 text-xs font-medium rounded-full',
                    item.isVueComponent ? 'bg-primary/20 text-primary' : 'bg-secondary/20 text-blue-500'
                  ]">
                    {{ item.codeType }}
                  </span>
                  <span v-if="item.requiresSubscription" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500 flex items-center gap-1">
                    <Icon name="heroicons:lock-closed" class="w-3 h-3" />
                    Premium
                  </span>
                </div>
              </div>
              <!-- Content -->
              <div class="p-6">
                <h3 class="text-lg font-semibold text-foreground mb-2 group-hover:text-primary transition-colors">
                  {{ item.title }}
                </h3>
                <p class="text-sm text-muted-foreground mb-4 line-clamp-2">
                  {{ item.description }}
                </p>
                <div class="flex items-center gap-4 text-xs text-muted-foreground">
                  <span class="flex items-center gap-1">
                    <Icon name="heroicons:eye" class="w-4 h-4" />
                    {{ item.viewCount }}
                  </span>
                  <span class="flex items-center gap-1">
                    <Icon name="heroicons:heart" class="w-4 h-4" />
                    {{ item.likeCount }}
                  </span>
                </div>
              </div>
            </div>
          </NuxtLink>
        </div>
      </div>

      <!-- All Playgrounds -->
      <div>
        <h2 class="text-2xl font-bold text-foreground mb-6">All Snippets</h2>
        <div v-if="pending" class="text-center py-12">
          <Icon name="heroicons:arrow-path" class="w-8 h-8 text-muted-foreground animate-spin mx-auto mb-4" />
          <p class="text-muted-foreground">Loading playground items...</p>
        </div>
        <div v-else-if="playgrounds && playgrounds.length > 0"
          class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <NuxtLink v-for="item in playgrounds" :key="item.id" :to="`/playground/${item.slug}`" class="group">
            <div
              class="bg-card/80 backdrop-blur-xl rounded-2xl border-2 border-border overflow-hidden hover:border-primary/50 transition-all duration-300">
              <!-- Preview Image or Placeholder -->
              <div class="aspect-video bg-muted relative overflow-hidden">
                <img v-if="item.previewImageUrl" :src="getImageUrl(item.previewImageUrl)" :alt="item.title"
                  class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300" />
                <div v-else class="w-full h-full flex items-center justify-center">
                  <Icon name="heroicons:code-bracket" class="w-12 h-12 text-muted-foreground" />
                </div>
                <div class="absolute top-3 right-3 flex gap-2">
                  <span :class="[
                    'px-2 py-1 text-xs font-medium rounded-full',
                    item.isVueComponent ? 'bg-primary/20 text-primary' : 'bg-secondary/20 text-blue-500'
                  ]">
                    {{ item.codeType }}
                  </span>
                  <span v-if="item.requiresSubscription" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500 flex items-center gap-1">
                    <Icon name="heroicons:lock-closed" class="w-3 h-3" />
                    Premium
                  </span>
                </div>
              </div>
              <!-- Content -->
              <div class="p-6">
                <h3 class="text-lg font-semibold text-foreground mb-2 group-hover:text-primary transition-colors">
                  {{ item.title }}
                </h3>
                <p class="text-sm text-muted-foreground mb-4 line-clamp-2">
                  {{ item.description }}
                </p>
                <div class="flex items-center gap-4 text-xs text-muted-foreground">
                  <span class="flex items-center gap-1">
                    <Icon name="heroicons:eye" class="w-4 h-4" />
                    {{ item.viewCount }}
                  </span>
                  <span class="flex items-center gap-1">
                    <Icon name="heroicons:heart" class="w-4 h-4" />
                    {{ item.likeCount }}
                  </span>
                </div>
              </div>
            </div>
          </NuxtLink>
        </div>
        <div v-else class="text-center py-12">
          <Icon name="heroicons:code-bracket" class="w-16 h-16 text-muted-foreground mx-auto mb-4" />
          <p class="text-muted-foreground">No playground items yet. Check back soon!</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Playground } from '~/../../shared/types'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

useHead({
  title: 'Playground | Judah Sullivan',
})

// Fetch featured playgrounds
const { data: featuredPlaygrounds } = await useFetch<Playground[]>(
  `${apiBase}/api/playground/featured`,
  {
    default: () => []
  }
)

// Fetch all playgrounds
const { data: playgrounds, pending } = await useFetch<Playground[]>(
  `${apiBase}/api/playground`,
  {
    default: () => []
  }
)

// Get full image URL
const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  return `${apiBase}${url}`
}
</script>
